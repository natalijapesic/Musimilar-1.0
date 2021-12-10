using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.MongoDB.Models.Entities;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> logger;
        private readonly IArtistService ArtistService;

        public ArtistController(IArtistService ArtistService, ILogger<ArtistController> logger)
        {
            this.ArtistService = ArtistService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Artist>> GetArtists(){
            //this.logger.LogInformation("Get Artist");

            return await ArtistService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Artist>> GetArtist(string id)
        {
           return await ArtistService.GetAsync(id);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteArtist(string id)
        {
           await ArtistService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<Artist> Insert(Artist Artist){
            return await ArtistService.InsertAsync(Artist);
        }

        [HttpPost("many")]
        public async Task<IEnumerable<Artist>> InsertMany(IEnumerable<Artist> Artists){
            return await ArtistService.InsertManyAsync(Artists);
        }


    }
}
