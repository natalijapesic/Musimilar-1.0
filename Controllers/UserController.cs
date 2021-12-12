using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.Requests;
using MusimilarApi.Models.MongoDB.Entities;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers(){
            //this.logger.LogInformation("Get user");

            return await userService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
           return await userService.GetAsync(id);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteUser(string id)
        {
           await userService.DeleteAsync(id);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<User> Insert(User user){
            return await userService.InsertAsync(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LoginRequest user){
            var token = await this.userService.LogIn(user.Email, user.Password);

            if(token == null)
                return Unauthorized();

            return Ok(new {token, user});
        }


    }
}
