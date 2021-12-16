using System.Collections.Generic;
using System.Threading.Tasks;
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

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this._userService = userService;
            this._logger = logger;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers(){

            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var currentUser = User.Identity.Name;
            if (id != currentUser && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = await _userService.GetAsync(id);

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
        public async Task<User> Insert(User user){
            return await _userService.InsertAsync(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LoginRequest request){
            var user = await this._userService.Authenticate(request.Email, request.Password);

            if(user.Token == null)
                return Unauthorized();

            return Ok(user);
        }


    }
}
