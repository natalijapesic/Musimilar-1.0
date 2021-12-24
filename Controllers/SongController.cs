using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;
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
        private readonly IMapper _mapper;

        public SongController(ISongService songService, 
                              ILogger<SongController> logger,
                              IMapper autoMapper)
        {
            _songService = songService;
            _logger = logger;
            _mapper = autoMapper;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IEnumerable<SongResponse>> GetSongs(){
            
            return _mapper.Map<IEnumerable<SongResponse>>(await _songService.GetAllAsync());
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SongResponse>> GetSong(string id)
        {
           return _mapper.Map<SongResponse>(await _songService.GetAsync(id));
        }

        [AllowAnonymous]
        [HttpPost("playlist")]
        public async Task<List<SongInfoDTO>> RecommendPlaylist(PlaylistRequest request)
        {
            PlaylistDTO playlistDTO = _mapper.Map<PlaylistDTO>(request);
            List<SongInfoDTO> songInfoDTOs = await _songService.RecommendPlaylistAsync(playlistDTO);
            return songInfoDTOs;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id:length(24)}")]
        public async Task DeleteSong(string id)
        {
           await _songService.DeleteAsync(id);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<SongResponse> Insert(SongRequest request){
            
            SongDTO songDTO = _mapper.Map<SongDTO>(request);
            songDTO = await _songService.InsertAsync(songDTO);
            return _mapper.Map<SongResponse>(songDTO);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("many")]
        public async Task<ICollection<SongResponse>> InsertMany(ICollection<SongRequest> requests){
            
            ICollection<SongDTO> songDTOs = _mapper.Map<ICollection<SongDTO>>(requests);
            songDTOs = await _songService.InsertManyAsync(songDTOs);
            return _mapper.Map<ICollection<SongResponse>>(songDTOs);
        }

    }
}
