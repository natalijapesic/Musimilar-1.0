using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;

namespace MusimilarApi.Interfaces
{
    public interface IUserService : IEntityService<User>{
        Task<string> LogIn(string email, string password);
    }
}
