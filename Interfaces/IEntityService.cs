using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Interfaces
{
    public interface IEntityService<T1, T2> where T1 : Entity where T2 : DTO
    {
        Task<IEnumerable<T2>> GetAllAsync();
        Task<T2> GetAsync(string id);
        Task<T2> InsertAsync(T2 obj);
        Task<ICollection<T2>> InsertManyAsync(ICollection<T2> obj);
        Task DeleteAsync(string id);
    }
}