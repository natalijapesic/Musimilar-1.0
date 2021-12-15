
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Helpers;
using MusimilarApi.Interfaces;

namespace MusimilarApi.Services
{
    public class EntityService<T> : IEntityService<T> where T : Entity
    {
        public readonly IMongoCollection<T> _collection;
        public readonly ILogger<EntityService<T>> _logger;

        public EntityService(   IDatabaseSettings settings, 
                                string collectionName, 
                                ILogger<EntityService<T>> logger 
                            ){

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            if (! database.ListCollectionNames().ToList().Contains(collectionName))
                database.CreateCollection(collectionName); 

            this._collection = database.GetCollection<T>(collectionName);

            this._logger = logger;
        }


        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _collection.Find(obj => true).ToListAsync();

        public async Task<T> GetAsync(string id)
        {
            return await _collection.Find<T>(obj => obj.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> InsertAsync(T obj)
        {
            await _collection.InsertOneAsync(obj);
            return obj;
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(obj => obj.Id == id);
        }

        public async Task<IEnumerable<T>> InsertManyAsync(IEnumerable<T> obj)
        {
            await _collection.InsertManyAsync(obj);
            return obj;
        }
    }
}