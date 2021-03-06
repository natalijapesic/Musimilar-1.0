using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        private readonly IMapper _mapper;

        public PlaylistController(ILogger<SongController> logger,
                                  IPlaylistService playlistService,
                                  IMapper mapper,
                                  ISongService songService)
        {
            this._playlistService = playlistService;
            this._songService = songService;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet("like/{id:length(24)}")]
        public async Task<ActionResult<double>> LikeAsync(string songId)
        {
            var result = await this._playlistService.LikeAsync(songId);

            SongDTO song = await this._songService.GetAsync(songId);

            if(song == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<double>> GetAsync(string songId)
        {
            var result = await this._playlistService.GetAsync(songId);
            if(result == null)
                return BadRequest();
                
            return Ok(result);
        }


    }
}
