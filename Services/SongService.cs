using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB;
using MusimilarApi.Models.MongoDB.Entities;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song>, ISongService
    {
        public readonly IMongoCollection<Song> _songs;

        //private readonly string key;

        public SongService(IDatabaseSettings settings, ILogger<SongService> logger)
        :base(settings, settings.SongsCollectionName, logger){}


    }
}