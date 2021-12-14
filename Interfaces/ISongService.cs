using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Models.MongoDB.Entities;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Interfaces
{
    public interface ISongService : IEntityService<Song>{

        Task<Song> InsertSongAsync(Song song);
        Task<Playlist> RecommendPlaylistAsync(PlaylistRequest request);
        Task<Song> GetSongByNameAsync(string name);
        Task<List<Song>> GetSongsByGenreAsync(string genre);

    }
}
