using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB;
using MusimilarApi.Models.MongoDB.Entities;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class UserService: EntityService<User>, IUserService
    {
        public readonly IMongoCollection<User> _users;
        public readonly IConfiguration _configuration;
        public readonly UserManager<User> _userManager;

        //private readonly string key;

        public UserService(IDatabaseSettings settings, 
                          IConfiguration config, 
                          ILogger<UserService> logger)
        :base(settings, settings.UsersCollectionName, logger){
            
            this._configuration = config;
        }


        public async Task<string> LogIn(string email, string password)
        {
            var user = await _collection.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();

            if(user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtKey").ToString());
            var tokenDescriptor = new SecurityTokenDescriptor(){
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Email, email),
                }),

                Expires = DateTime.Now.AddHours(6),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);            
        }

    }
}