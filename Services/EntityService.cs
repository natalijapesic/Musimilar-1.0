
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB;
using MusimilarApi.Models.MongoDB.Entities;

namespace MusimilarApi.Services
{
    public class EntityService<T> : IEntityService<T> where T : Entity
    {
        public readonly IMongoCollection<T> collection;
        public readonly ILogger<EntityService<T>> logger;

        public EntityService(   IDatabaseSettings settings, 
                                string collectionName, 
                                ILogger<EntityService<T>> logger 
                            ){

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            if (! database.ListCollectionNames().ToList().Contains(collectionName))
                database.CreateCollection(collectionName); 

            this.collection = database.GetCollection<T>(collectionName);

            this.logger = logger;
        }


        public async Task<IEnumerable<T>> GetAllAsync() =>
            await collection.Find(obj => true).ToListAsync();

        public async Task<T> GetAsync(string id)
        {
            return await collection.Find<T>(obj => obj.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> InsertAsync(T obj)
        {
            await collection.InsertOneAsync(obj);
            return obj;
        }

        public async Task DeleteAsync(string id)
        {
            await collection.DeleteOneAsync(obj => obj.Id == id);
        }

        public async Task<IEnumerable<T>> InsertManyAsync(IEnumerable<T> obj)
        {
            await collection.InsertManyAsync(obj);
            return obj;
        }
    }
}