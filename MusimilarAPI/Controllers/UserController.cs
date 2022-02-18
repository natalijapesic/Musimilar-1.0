using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;
using MusimilarApi.Models.Queries;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.Responses;

namespace MusimilarApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, 
                              ILogger<UserController> logger,
                              IMapper mapper,
                              IPlaylistService playlistService,
                              ISongService songService){
            this._userService = userService;
            this._playlistService = playlistService;
            this._logger = logger;
            this._mapper = mapper;
            this._songService = songService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(){

            IEnumerable<UserDTO> userDTOs = await _userService.GetAllAsync();
            if(userDTOs == null)
                return NoContent();

            IEnumerable<User> users = _mapper.Map<IEnumerable<User>>(userDTOs);

            return Ok(users);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var currentUser = User.Identity.Name;
            if (id != currentUser && !User.IsInRole(Role.Admin))
                return Forbid();

            UserDTO userDTO = await _userService.GetAsync(id);
            if(userDTO == null)
                return NotFound();

            User user = _mapper.Map<User>(userDTO);

            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            bool result = await _userService.DeleteAsync(id);
            if(result)
                return Ok();
            else
                return BadRequest();
                            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> LogIn([FromBody] LoginRequest request){

            UserDTO userDTO = await _userService.LogInAsync(request.Email, request.Password);

            if(userDTO == null)
                return NotFound();

            if(userDTO.Token == null)
                return Unauthorized();

            UserResponse user = _mapper.Map<UserResponse>(userDTO);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<User>> RegisterAsync([FromBody] RegisterRequest request)
        {
            UserDTO userDTO = _mapper.Map<UserDTO>(request);
            userDTO = await _userService.RegisterAsync(userDTO);

            if(userDTO == null)
                return BadRequest();

            User user = _mapper.Map<User>(userDTO);

            return Ok(user);
        }

        [HttpPut("add/playlist")]
        public async Task<ActionResult<Playlist>> AddPlaylistAsync([FromBody] AddPlaylistRequest request)
        {
            PlaylistDTO playlistDTO = _mapper.Map<PlaylistDTO>(request);

            UserDTO userDTO = await _userService.GetAsync(request.UserId);

            if(userDTO == null)
                return BadRequest("User doesnt exist");
            
            SongDTO song = await this._songService.GetSongByNameAsync(request.Example.Name, request.Example.Artist);
            if(song == null)
                return BadRequest();
            
            playlistDTO = await _userService.AddPlaylistAsync(playlistDTO, userDTO);

            if(playlistDTO == null)
                return BadRequest("Playlist already exists or song-example doesnt exist");
            else
                return Ok(this._mapper.Map<Playlist>(playlistDTO));
        }

        [HttpPut("delete/playlist")]
        public async Task<ActionResult<PlaylistDTO>> DeletePlaylistAsync([FromBody] DeletePlaylistRequest request)
        {
            UserDTO userDTO = await _userService.GetAsync(request.UserId);

            if(userDTO == null)
                return BadRequest("User doesnt exist");
            
            PlaylistDTO playlistDTO = await _userService.DeletePlaylistAsync(request.PlaylistName, userDTO);

            if(playlistDTO == null)
                return BadRequest();
                
            return Ok(this._mapper.Map<Playlist>(playlistDTO));
        }

        [HttpGet("playlist/feed")]
        public async Task<ActionResult<List<PlaylistFeedResponse>>> GetPlaylistFeed([FromQuery] GetPlaylistFeed getPlaylist){

            if(getPlaylist.Subscriptions.Count == 0)
                return BadRequest();

            List<PlaylistFeedDTO> playlistFeedDTOs = await this._playlistService.UsersFeedAsync(getPlaylist.Subscriptions);

            if(playlistFeedDTOs == null || playlistFeedDTOs.Count == 0)
                return BadRequest();

            return Ok(this._mapper.Map<List<PlaylistFeedResponse>>(playlistFeedDTOs));
        }

        [HttpPut("subscribe")]
        public async Task<ActionResult> SubscribeGenreAsync([FromBody] SubscribeGenreRequest request)
        {
            UserDTO userDTO = await _userService.GetAsync(request.UserId);

            if(userDTO == null)
                return BadRequest("User doesnt exist");
            
            bool result = await _userService.AddSubscriptionAsync(request.Genres, userDTO);

            if(!result)
                return BadRequest();
                
            return Ok();
        }
    }
}
