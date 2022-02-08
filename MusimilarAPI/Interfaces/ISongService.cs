using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Interfaces
{
    public interface ISongService : IEntityService<Song, SongDTO>{
        Task<List<SongInfoDTO>> RecommendPlaylistAsync(SongInfoDTO request, SongDTO songExample);
        Task<SongDTO> GetSongByNameAsync(string name, string artist);
        Task<List<SongDTO>> GetSongsByGenreAsync(string genre);

    }
}
