using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Interfaces
{
    public interface IPlaylistService {
        Task<long> CreateSetOfSongsAsync(List<SongInfoDTO> playlist, string genre, string songId); 
        Task AddNewAsync(string genre, string songId); 
        Task<double> LikeAsync(string key); 
        Task<List<PlaylistFeedDTO>> UsersFeedAsync(List<string> subscriptions);
        Task<List<SongInfoDTO>> GetAsync(string songId);

    }
}
