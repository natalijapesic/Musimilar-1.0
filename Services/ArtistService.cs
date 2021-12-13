using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using Neo4j.Driver;

namespace MusimilarApi.Service
{
    public class ArtistService: IArtistService
    {
        private readonly IDriver _driver;
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IDriver driver, ILogger<ArtistService> logger){

            this._driver = driver;
            this._logger = logger;
        }

        public async Task<List<string>> ConnectArtistWithGenresAsync(string name, List<string> genres)
        {
            List<string> result = new List<string>();
            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                    foreach(var genre in genres){

                        var cursor = await tx.RunAsync(@"MATCH (a:ArtistNode) 
                                                        WHERE a.name = $name
                                                        MERGE (g:GenreNode {name: $genre})
                                                        CREATE (a)<-[:PLAYS]-(g) 
                                                        RETURN g.name AS name",
                                                    new {name = name, genre = genre}
                                                    );

                        result.Add(await cursor.SingleAsync<string>(record => 
                            record["name"].As<string>() ));
                    }
                    return result;
                });
            }
        }

        public Task DeleteAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ArtistNode> GetArtistAsync(string artistName)
        {
            if(string.IsNullOrWhiteSpace(artistName))
            {
                this._logger.LogError("String is null or white space");
                return null;
            }

            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                        var cursor = await tx.RunAsync(@"MATCH (a:ArtistNode) 
                                                        WHERE a.name = $artistName
                                                        RETURN a.name AS name,
                                                               a.genres AS genres,
                                                               a.image AS image",
                                                    new {artistName}
                                                    );

                        return await cursor.SingleAsync(record => new ArtistNode(
                            record["name"].As<string>(), record["genres"].As<List<string>>(), record["image"].As<string>()
                        ));
                });
            }
        }

        public async Task<List<ArtistNode>> GetArtistNodesByGenreAsync(string genre)
        {                                                                                          

            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                        var cursor = await tx.RunAsync(@"MATCH (g:GenreNode), (a:ArtistNode) 
                                                        WHERE (a)<-[:PLAYS]-(g) AND g.name = $genre
                                                        RETURN a.name AS name,
                                                               a.genres AS genres,
                                                               a.image AS image",
                                                    new {genre = genre}
                                                    );

                        return await cursor.ToListAsync(record => new ArtistNode(
                            record["name"].As<string>(), record["genres"].As<List<string>>(), record["image"].As<string>()
                        ));
                });
            }
        }

        public async Task<List<ArtistNode>> GetSimilarArtistsAsync(string artistName)
        {
            List<ArtistNode> union = new List<ArtistNode>();
            ArtistNode artist = await GetArtistAsync(artistName);
            if(artist != null)
            {
                foreach (var genre in artist.Genres)
                    union = union.Union(await GetArtistNodesByGenreAsync(genre)).ToList();
                
                union.OrderByDescending(a => a.Genres.Intersect(artist.Genres).Count()).ToList().Remove(artist);
                return union;
            }
            return null;
        }

        public async Task<ArtistNode> InserNodeAsync(ArtistNode obj)
        {
            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                    var cursor = await tx.RunAsync(@"MERGE (a:ArtistNode 
                                                    {name: $name, genres: $genres, image: $image})
                                                    RETURN a.name AS name,
                                                           a.genres AS genres,
                                                           a.image AS image",
                                                    new {name = obj.Name, genres = obj.Genres, image = obj.Image}
                                                   );

                    return await cursor.SingleAsync<ArtistNode>(record => new ArtistNode(
                            record["name"].As<string>(), record["genres"].As<List<string>>(), record["image"].As<string>()));

                });
            }
        }
    }
}