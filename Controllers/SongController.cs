using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly ISongService _songService;

        public SongController(ISongService songService, ILogger<SongController> logger)
        {
            this._songService = songService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> GetSongs(){

            return await _songService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Song>> GetSong(string id)
        {
           return await _songService.GetAsync(id);
        }

        [HttpGet("playlist")]
        public async Task<Playlist> RecommendPlaylist(PlaylistRequest request)
        {
           return await _songService.RecommendPlaylistAsync(request);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteSong(string id)
        {
           await _songService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<Song> Insert(Song Song){
            return await _songService.InsertSongAsync(Song);
        }

        [HttpPost("many")]
        public async Task<IEnumerable<Song>> InsertMany(IEnumerable<Song> songs){
            return await _songService.InsertManyAsync(songs);
        }




    }
}
