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

        public PlaylistController(ILogger<SongController> logger)
        {
            this._logger = logger;
        }


    }
}
