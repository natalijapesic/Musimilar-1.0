using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Interfaces
{
    public interface IUserService : IEntityService<User, UserDTO>{
        Task<UserDTO> LogInAsync(string email, string password);
        Task<UserDTO> RegisterAsync(UserDTO model);
        Task<ICollection<PlaylistDTO>> AddPlaylistAsync(PlaylistDTO model, UserDTO user);

    }
}
