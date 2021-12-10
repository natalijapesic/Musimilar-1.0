using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.MongoDB.Models.Entities;

namespace MusimilarApi.Interfaces
{
    public interface IEntityService<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task<T> InsertAsync(T obj);
        Task<IEnumerable<T>> InsertManyAsync(IEnumerable<T> obj);
        Task DeleteAsync(string id);
    }
}