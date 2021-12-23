using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;

namespace MusimilarApi.Interfaces
{
    public interface IPlaylistService {

        Task<long> CreateSetOfSongs(List<SongInfo> playlist, SongInfo example);
        Task<bool> DoesKeyExist(string key);
        Task<int> ExtendTTL(string key);
        Task AddNewPlaylist(string genre, Playlist playlist);
        Task<bool> DeleteKey(string key);
        Task<bool> LikePlaylist(string key);
        Task<int> GenerateId(string key);




    }
}
