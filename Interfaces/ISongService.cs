using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Interfaces
{
    public interface ISongService : IEntityService<Song>{
        Task<Song> InsertSongAsync(SongRequest song);
        Task<List<SongInfo>> RecommendPlaylistAsync(PlaylistRequest request);
        Task<Song> GetSongByNameAsync(string name, string artist);
        Task<List<Song>> GetSongsByGenreAsync(string genre);
        Task<IEnumerable<Song>> InsertSongManyAsync(IEnumerable<SongRequest> requests);

    }
}
