using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.MongoDB.Models;
using MusimilarApi.MongoDB.Models.Entities;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class ArtistService: EntityService<Artist>, IArtistService
    {
        public readonly IMongoCollection<Artist> Artists;
        public readonly IConfiguration configuration;
        //private readonly string key;

        public ArtistService(IDatabaseSettings settings, ILogger<ArtistService> logger)
        :base(settings, settings.ArtistsCollectionName, logger){}


    }
}