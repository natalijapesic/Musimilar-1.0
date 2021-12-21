using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Interfaces
{
    public interface IUserService : IEntityService<User>{
        Task<User> LogInAsync(string email, string password);
        Task<User> GetById(string id);
        Task<User> RegisterAsync(RegisterRequest model);

        //Task<string> SubscribeToChannel(string genre, string id);
    }
}
