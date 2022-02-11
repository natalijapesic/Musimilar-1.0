using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Entities.Neo4j;
using MusimilarApi.Interfaces;
using Neo4j.Driver;
using Newtonsoft.Json;

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
            var session = _driver.AsyncSession();
            try
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
            catch(Exception example){
                this._logger.LogError($"ConnectArtistWithGenres exception: {example}");
                return null;
            }
            finally{
                await session.CloseAsync();
            }
        }

        public Task DeleteAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<ArtistNode>> GetSimilarArtistsAsync(string artistName)
        {
            ArtistNode artist = await GetArtistAsync(artistName);

            if(artist == null)
            {
                _logger.LogError($"Artist {artistName} doesnt exist");
                return null;
            }    
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx => 
                {
                    var cursor = await tx.RunAsync(@"MATCH (a1:ArtistNode), (a:ArtistNode)
                                                    WHERE toLower(a1.name) CONTAINS toLower($name)
                                                    AND
                                                    (a1)-[:PLAYS*2]-(a)
                                                    RETURN DISTINCT a.name AS name,
                                                                    a.genres AS genres,
                                                                    a.image AS image",
                                                new {name = artistName}
                                                );                      

                    List<ArtistNode> similarArtists = await cursor.ToListAsync(record => new ArtistNode(
                        record["name"].As<string>(), record["genres"].As<List<string>>(), record["image"].As<string>()
                    ));

                    return similarArtists.OrderByDescending(a => a.Genres.Intersect(artist.Genres).Count()).ToList();

                });
            }
            catch(Exception example){
                this._logger.LogError($"GetSimilarArtists exception: {example}");
                
                return null;
            }
            finally{
                await session.CloseAsync();
            }
        }

        public async Task<List<ArtistNode>> GetArtistNodesByGenreAsync(string genre)
        {                                                                                         
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx => 
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
            catch(Exception example){
                this._logger.LogError($"GetArtistNodesByGenre exception: {example}");
                return null;
            }
            finally{
                await session.CloseAsync();
            }
        }

        public async Task<ArtistNode> InserNodeAsync(ArtistNode obj)
        {
            var session = _driver.AsyncSession();
            try
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
            catch(Exception example){
                this._logger.LogError($"InsertNode exception: {example}");
                return null;
            }
            finally{
                await session.CloseAsync();
            }
        }

        public async Task<ArtistNode> GetArtistAsync(string artistName)
        {
            var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx => 
                {
                    var cursor = await tx.RunAsync(@"MATCH (a:ArtistNode) 
                                                    WHERE toLower(a.name) CONTAINS toLower($name)
                                                    RETURN a.name AS name,
                                                            a.genres AS genres,
                                                            a.image AS image",
                                                new {name = artistName}
                                                );

                    var result = await cursor.SingleAsync(record => new ArtistNode(
                    record["name"].As<string>(), record["genres"].As<List<string>>(), record["image"].As<string>() 
                    ));

                    return result;
                        
                });
            }
            catch(Exception example){
                this._logger.LogError($"GetArtist exception: {example}");

                ArtistNode artistNode = await GetArtistFromSpotify(artistName);
                if(artistNode != null)
                    artistNode = await InserNodeAsync(artistNode);
                return artistNode;
            }
            finally{
                await session.CloseAsync();
            }

        }

        public async Task<ArtistNode> GetArtistFromSpotify(string artistName)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\Nand\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";

            string path = Directory.GetCurrentDirectory();

            start.Arguments = string.Format("{0} {1}", $"{path}/Helpers/spotify_client.py", artistName);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if(!result.Contains("id"))
                        return null;

                    ArtistNode artistNode = JsonConvert.DeserializeObject<ArtistNode>(result);
                    artistNode.Name = artistName;

                    await InserNodeAsync(artistNode);
                    await ConnectArtistWithGenresAsync(artistNode.Name, artistNode.Genres);

                    return artistNode;
                }
            }
        }

    }
}