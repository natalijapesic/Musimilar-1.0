
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Helpers;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Services
{
    public class EntityService<T1, T2> : IEntityService<T1, T2> where T1 : Entity where T2 : DTO
    {
        public readonly IMongoCollection<T1> _collection;
        public readonly ILogger<EntityService<T1, T2>> _logger;
        public readonly IMapper _mapper;

        public EntityService(   IDatabaseSettings settings, 
                                string collectionName, 
                                ILogger<EntityService<T1, T2>> logger,
                                IMapper mapper 
                            ){

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            if (! database.ListCollectionNames().ToList().Contains(collectionName))
                database.CreateCollection(collectionName); 

            _collection = database.GetCollection<T1>(collectionName);

            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<T2>> GetAllAsync(){

            IEnumerable<T1> entity = await _collection.Find<T1>(obj => true).ToListAsync();
            return _mapper.Map<IEnumerable<T2>>(entity);
        }

        public async Task<T2> GetAsync(string id)
        {
            try{
                T1 entity = await _collection.Find<T1>(obj => obj.Id == id).FirstOrDefaultAsync();
                return _mapper.Map<T2>(entity);
            }
            catch(Exception exception){

                this._logger.LogError($"Get by id: {id} throws {exception}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try{
                await _collection.DeleteOneAsync(obj => obj.Id == id);
                return true;
            }
            catch(Exception exception){

                this._logger.LogError($"Delete by id: {id} throws {exception}");
                return false;
            }
        }

        public virtual Task<List<T2>> InsertManyAsync(List<T2> obj)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<T2> InsertAsync(T2 obj)
        {
            throw new System.NotImplementedException();
        }
    }
}