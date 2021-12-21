using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly ISongService _songService;

        public PlaylistController(ISongService songService, ILogger<SongController> logger)
        {
            this._songService = songService;
            this._logger = logger;
        }


    }
}
