using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Helpers;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;
using MongoDB.Driver.Linq;
using MusimilarApi.Services;
using StackExchange.Redis;
using BCryptNet = BCrypt.Net.BCrypt;

namespace MusimilarApi.Service
{
    public class UserService: EntityService<User, UserDTO>, IUserService
    {
        private readonly IConnectionMultiplexer _redis;
        public readonly IConfiguration _configuration;
        public readonly ISongService _songService;

        public UserService(IDatabaseSettings settings, 
                          IConfiguration config, 
                          ILogger<UserService> logger,
                          IConnectionMultiplexer redis,
                          IMapper mapper,
                          ISongService songService)
        :base(settings, settings.UsersCollectionName, logger, mapper){

            this._configuration = config;
            this._redis = redis;
            this._songService = songService;
        }


        public async Task<UserDTO> LogInAsync(string email, string password)
        {
            try{

                User user = await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
                if(user == null || !BCryptNet.Verify(password, user.PasswordHash))
                    throw new AppException("Username or password is incorrect");
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtKey").ToString());
                var tokenDescriptor = new SecurityTokenDescriptor(){
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),

                    Expires = DateTime.Now.AddHours(6),

                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey), 
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);     
                user.WithoutPassword();

                return _mapper.Map<UserDTO>(user);

            }catch(Exception exception){

                this._logger.LogError($"LogIn email {email} and password {password} throws exception {exception}");
                return null;
            } 
        }

        public override async Task<UserDTO> GetAsync(string id) 
        {
            try{
                var user = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                user.WithoutPassword();
            
                return _mapper.Map<UserDTO>(user);

            }catch(Exception exception){

                this._logger.LogError($"Get id {id} throws exception {exception}");
                return null;
            }
        }

        public override async Task<IEnumerable<UserDTO>> GetAllAsync() 
        {
            try{
                IEnumerable<User> users = await _collection.Find<User>(obj => true).ToListAsync();
                foreach (var user in users)
                    user.WithoutPassword();

                return _mapper.Map<IEnumerable<UserDTO>>(users);

            }catch(Exception exception){

                this._logger.LogError($"GetAll throws exception {exception}");
                return null;
            }
        }
        

        public async Task<UserDTO> RegisterAsync(UserDTO model)
        {
            if(!ValidateModel(model))
                return null;

            try{
                if(_collection.Find(x => x.Email == model.Email).Any())
                throw new AppException($"User with this email already exist");

                User user = _mapper.Map<User>(model);
                user.PasswordHash = BCryptNet.HashPassword(model.Password);
                user.Role = model.Role;

                await _collection.InsertOneAsync(user);   
                
                user.WithoutPassword();
                return _mapper.Map<UserDTO>(user);
            }
            catch(Exception exception)
            {
                this._logger.LogError($"Register throws exception {exception}");
                return null;
            }
            
        }

        private bool ValidateModel(UserDTO model){

            if(model.Password == null || model.Email == null || model.Name == null)
                return false;

            try{
                var addr = new System.Net.Mail.MailAddress(model.Email);
                return addr.Address == model.Email;
            }
            catch{
                return false;
            }
        }

        public async Task<ICollection<PlaylistDTO>> AddPlaylistAsync(PlaylistDTO model, UserDTO userDTO)
        {
            SongDTO song = await this._songService.GetSongByNameAsync(model.Example.Name, model.Example.Artist);
            if(song == null)
                return null;

            PlaylistDTO playlistDTO = userDTO.Playlists.Find(p => p.Example.Name == model.Example.Name && p.Example.Artist == model.Example.Artist);

            if(playlistDTO == null)
                return null;

            Playlist playlist = _mapper.Map<Playlist>(model);

            ICollection<Playlist> playlists = _mapper.Map<ICollection<Playlist>>(userDTO.Playlists);
            playlists.Add(playlist);

            var update = Builders<User>.Update.Set(p => p.Playlists, playlists);
            await _collection.UpdateOneAsync(u => u.Id == userDTO.Id, update);
            return userDTO.Playlists;
        }
    }
}