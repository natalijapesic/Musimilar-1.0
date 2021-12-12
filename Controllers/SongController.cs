using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB.Entities;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> logger;
        private readonly ISongService SongService;

        public SongController(ISongService SongService, ILogger<SongController> logger)
        {
            this.SongService = SongService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> GetSongs(){
            //this.logger.LogInformation("Get Song");

            return await SongService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Song>> GetSong(string id)
        {
           return await SongService.GetAsync(id);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteSong(string id)
        {
           await SongService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<Song> Insert(Song Song){
            return await SongService.InsertAsync(Song);
        }

        [HttpPost("many")]
        public async Task<IEnumerable<Song>> InsertMany(IEnumerable<Song> songs){
            return await SongService.InsertManyAsync(songs);
        }


    }
}
