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
        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;

        public SongController(ISongService songService, 
                              IPlaylistService playlistService,
                              ILogger<SongController> logger,
                              IMapper autoMapper
                              )
        {
            _songService = songService;
            _logger = logger;
            _mapper = autoMapper;
            _playlistService = playlistService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IEnumerable<SongDTO>> GetSongs(){
            
            return await _songService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SongResponse>> GetSong(string id)
        {
            SongDTO songDTO = await _songService.GetAsync(id);
            if(songDTO == null)
                return BadRequest();

            return Ok(_mapper.Map<SongResponse>(songDTO));
        }

        [AllowAnonymous]
        [HttpPost("playlist")]
        public async Task<ActionResult<List<SongInfoResponse>>> RecommendPlaylistAsync(SongInfoRequest request)
        {
            SongDTO songExample = await this._songService.GetSongByNameAsync(request.Name, request.Artist);

            if(songExample == null)
                return BadRequest("Example song doesnt exist");

            List<SongInfoDTO> songInfoDTOs = await this._playlistService.GetAsync(songExample.Id);
            if(songInfoDTOs != null)
                return Ok(songInfoDTOs);

            songInfoDTOs = await _songService.RecommendPlaylistAsync(songExample);
            if(songInfoDTOs == null)
                return BadRequest("Similar song doesnt exist");
            
            long numberInput = await _playlistService.CreateSetOfSongsAsync(songInfoDTOs, songExample.Genre, songExample.Id);
            _logger.LogInformation($"Set has {numberInput} songs");

            return Ok(this._mapper.Map<List<SongInfoResponse>>(songInfoDTOs));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteSong(string id)
        {
            bool result = await _songService.DeleteAsync(id);
            if(result)
                return Ok();
            else
                return BadRequest();
        
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<SongResponse>> InsertAsync(SongRequest request){
            
            SongDTO songDTO = _mapper.Map<SongDTO>(request);
            songDTO = await _songService.InsertAsync(songDTO);
            if(songDTO == null)
                return BadRequest();

            return Ok(_mapper.Map<SongResponse>(songDTO));
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("many")]
        public async Task<ActionResult<ICollection<SongResponse>>> InsertManyAsync(ICollection<SongRequest> requests){
            
            List<SongDTO> songDTOs = _mapper.Map<List<SongDTO>>(requests);
            songDTOs = await _songService.InsertManyAsync(songDTOs);
            if(songDTOs == null)
                return BadRequest();
            return Ok(_mapper.Map<ICollection<SongResponse>>(songDTOs));
        }

    }
}
