using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.MongoDB.Models;
using MusimilarApi.MongoDB.Models.Entities;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song>, ISongService
    {
        public readonly IMongoCollection<Song> Songs;
        public readonly IConfiguration configuration;
        //private readonly string key;

        public SongService(IDatabaseSettings settings, ILogger<SongService> logger)
        :base(settings, settings.SongsCollectionName, logger){}


    }
}