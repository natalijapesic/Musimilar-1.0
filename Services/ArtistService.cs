using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB;
using MusimilarApi.Models.MongoDB.Entities;
using MusimilarApi.Services;
using Neo4j.Driver;

namespace MusimilarApi.Service
{
    public class ArtistService: EntityService<Artist>, IArtistService
    {
        //private readonly IMongoCollection<Artist> artists;
        private readonly IDriver _driver;
        //private readonly IConfiguration _configuration;

        public ArtistService(IDatabaseSettings settings, IDriver driver, ILogger<ArtistService> logger)
        :base(settings, settings.ArtistsCollectionName, logger){

            this._driver = driver;
        }

        public async Task<List<string>> ConnectArtistWithGenresAsync(string id, List<string> genres)
        {
            List<string> result = new List<string>();
            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                    foreach(var genre in genres){

                        var cursor = await tx.RunAsync(@"MATCH (a:ArtistNode) 
                                                        WHERE a.mongoId = $id
                                                        MERGE (g:GenreNode {name: $genre})
                                                        CREATE (a)<-[:PLAYS]-(g) 
                                                    RETURN g.name AS name",
                                                    new {id = id, genre = genre}
                                                   );

                        result.Add(await cursor.SingleAsync<string>(record => 
                            record["name"].As<string>() ));
                    }
                    return result;
                });
            }
        }

        public async Task<ArtistNode> InserNodeAsync(Artist obj)
        {
            using(var session = _driver.AsyncSession())
            {
                return await session.WriteTransactionAsync(async tx => 
                {
                    var cursor = await tx.RunAsync(@"MERGE (a:ArtistNode {mongoId: $id})
                                                    RETURN a.mongoId AS mongoId",
                                                    new {id = obj.Id.ToString()}
                                                   );

                    return await cursor.SingleAsync<ArtistNode>(artist => new ArtistNode(
                                                            artist["mongoId"].As<string>()));

                });
            }
        }
    }
}