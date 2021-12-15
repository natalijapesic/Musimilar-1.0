using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.Neo4j;
using MusimilarApi.Interfaces;
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

    

        [HttpDelete("{name}")]
        public async Task DeleteArtistAsync(string name)
        {
           await _artistService.DeleteAsync(name);
        }

        [HttpGet("{name}")]
        public async Task<ArtistNode> GetArtistAsync(string name)
        {
           return await _artistService.GetArtistAsync(name);
        }

        [HttpGet("genre/{genre}")]
        public async Task<List<ArtistNode>> GetArtistByGenreAsync(string genre)
        {
           return await _artistService.GetArtistNodesByGenreAsync(genre);
        }

        [HttpGet("similar/{artistName}")]
        public async Task<List<ArtistNode>> GetSimilarArtistsAsync(string artistName)
        {
           return await _artistService.GetSimilarArtistsAsync(artistName);
        }


        [HttpPost]
        public async Task<List<string>> InsertAsync(ArtistNode artist){

            await _artistService.InserNodeAsync(artist);
            this._logger.LogInformation($"{artist.Name} was inserted in Neo4j");

            List<string> result = await _artistService.ConnectArtistWithGenresAsync(artist.Name, artist.Genres);
            this._logger.LogInformation($"{result} was inserted in Neo4j");

            return result;
            
        }


    }
}
