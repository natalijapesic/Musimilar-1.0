using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;

namespace MusimilarApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;

        public PlaylistController(ILogger<SongController> logger,
                                  IPlaylistService playlistService,
                                  IMapper mapper)
        {
            this._playlistService = playlistService;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet("like/{id:length(24)}")]
        public async Task<ActionResult<double>> LikeAsync(string songId)
        {
            var result = await this._playlistService.LikeAsync(songId);

            return Ok(result);
        }


    }
}
