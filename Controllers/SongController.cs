using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.Responses;

namespace MusimilarApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly ISongService _songService;
        private readonly IMapper _autoMapper;

        public SongController(ISongService songService, 
                              ILogger<SongController> logger,
                              IMapper autoMapper)
        {
            _songService = songService;
            _logger = logger;
            _autoMapper = autoMapper;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IEnumerable<SongResponse>> GetSongs(){
            
            return _autoMapper.Map<IEnumerable<SongResponse>>(await _songService.GetAllAsync());
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SongResponse>> GetSong(string id)
        {
           return _autoMapper.Map<SongResponse>(await _songService.GetAsync(id));
        }

        // [HttpGet("playlist")]
        // public async Task<Playlist> RecommendPlaylist(PlaylistRequest request)
        // {
        //    return await _songService.RecommendPlaylistAsync(request);
        // }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id:length(24)}")]
        public async Task DeleteSong(string id)
        {
           await _songService.DeleteAsync(id);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<Song> Insert(SongRequest request){

            return _autoMapper.Map<Song>(await _songService.InsertSongAsync(request));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("many")]
        public async Task<IEnumerable<SongResponse>> InsertMany(IEnumerable<SongRequest> request){

            IEnumerable<Song> songs = _autoMapper.Map<IEnumerable<Song>>(request);
            return _autoMapper.Map<IEnumerable<SongResponse>>(await _songService.InsertManyAsync(songs));
        }

    }
}
