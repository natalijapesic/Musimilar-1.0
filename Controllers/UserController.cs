using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.Requests;

namespace MusimilarApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, 
                              ILogger<UserController> logger,
                              IMapper mapper)
        {
            this._userService = userService;
            this._logger = logger;
            this._mapper = mapper;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(){

            IEnumerable<User> users = _mapper.Map<IEnumerable<User>>(await _userService.GetAllAsync());

            return Ok(users);
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var currentUser = User.Identity.Name;
            if (id != currentUser && !User.IsInRole(Role.Admin))
                return Forbid();

            User user = _mapper.Map<User>(await _userService.GetAsync(id));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteUser(string id)
        {
           await _userService.DeleteAsync(id);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<User> Insert(UserRequest request){

            User user = _mapper.Map<User>(request);
            return await _userService.InsertAsync(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> LogIn([FromBody] LoginRequest request){
            User user = await _userService.LogInAsync(request.Email, request.Password);

            if(user.Token == null)
                return Unauthorized();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<User>> Registration([FromBody] RegisterRequest request)
        {
            User user = await _userService.RegisterAsync(request);

            User response = _mapper.Map<User>(user);

            return Ok(user);
        }

    }
}
