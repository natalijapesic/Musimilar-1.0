using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB.Entities;
using Neo4j.Driver;

namespace MusimilarApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService _artistService, IDriver driver, ILogger<ArtistController> logger)
        {
            this._artistService = _artistService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Artist>> GetArtists(){
            //this.logger.LogInformation("Get Artist");

            return await _artistService.GetAllAsync();
        }

        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Artist>> GetArtist(string id)
        {
           return await _artistService.GetAsync(id);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task DeleteArtist(string id)
        {
           await _artistService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<Artist> Insert(Artist Artist){
            return await _artistService.InsertAsync(Artist);
        }

        [HttpPost("many")]
        public async Task<List<string>> InsertMany(Artist artist){

            Artist obj = await _artistService.InsertAsync(artist);
            await _artistService.InserNodeAsync(obj);
            return await _artistService.ConnectArtistWithGenresAsync(obj.Id.ToString(), obj.Genres);
            
        }


    }
}
