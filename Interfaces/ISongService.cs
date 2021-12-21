using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Interfaces
{
    public interface ISongService : IEntityService<Song>{
        Task<Song> InsertSongAsync(SongRequest song);
        Task<Playlist> RecommendPlaylistAsync(PlaylistRequest request);
        Task<Song> GetSongByNameAsync(string name);
        Task<List<Song>> GetSongsByGenreAsync(string genre);

    }
}
