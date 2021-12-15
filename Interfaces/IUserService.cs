using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;

namespace MusimilarApi.Interfaces
{
    public interface IUserService : IEntityService<User>{
        Task<User> Authenticate(string email, string password);
        Task<User> GetById(string id);
    }
}
