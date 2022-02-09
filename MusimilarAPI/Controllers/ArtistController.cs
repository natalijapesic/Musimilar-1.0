using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Entities.Neo4j;
using MusimilarApi.Interfaces;
using Neo4j.Driver;

namespace MusimilarApi.Controllers
{
    [Authorize]
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

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{name}")]
        public async Task DeleteArtistAsync(string name)
        {
           await _artistService.DeleteAsync(name);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ArtistNode>> GetArtistAsync(string name)
        {
            ArtistNode artistNode = await _artistService.GetArtistAsync(name);
            if(artistNode == null)
            {
                artistNode = await this._artistService.GetArtistFromSpotify(name);
            } 

            if(artistNode != null)   
                return Ok(artistNode);
            else
                return BadRequest(artistNode);
        }

        [HttpGet("genre/{genreName}")]
        public async Task<ActionResult<List<ArtistNode>>> GetArtistByGenreAsync(string genre)
        {
            var result = await _artistService.GetArtistNodesByGenreAsync(genre);
            if(result == null)
                return BadRequest();
            else
                return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("similar/{artistName}")]
        public async Task<ActionResult<List<ArtistNode>>> GetSimilarArtistsAsync(string artistName)
        {
            List<ArtistNode> artistNodes = await _artistService.GetSimilarArtistsAsync(artistName);
            if(artistNodes == null)
                return BadRequest();
            
            return Ok(artistNodes);

        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<ArtistNode>> InsertAsync(ArtistNode artist){

            ArtistNode artistNode = await _artistService.InserNodeAsync(artist);
            if(artistNode == null)
                return BadRequest();
            this._logger.LogInformation($"{artist.Name} was inserted in Neo4j");

            List<string> result = await _artistService.ConnectArtistWithGenresAsync(artist.Name, artist.Genres);
            this._logger.LogInformation($"{result} was inserted in Neo4j");

            return Ok(artistNode);
            
        }


    }
}
