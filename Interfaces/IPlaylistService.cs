using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Interfaces
{
    public interface IPlaylistService {

        Task<long> CreateSetOfSongs(List<SongInfoDTO> playlist, string songId);
        Task<bool> DoesKeyExist(string key);
        Task<int> ExtendTTL(string key);
        Task AddNewPlaylist(string genre, PlaylistDTO playlist);
        Task<bool> DeleteKey(string key);
        Task<bool> LikePlaylist(string key);
        Task<int> GenerateId(string key);




    }
}
