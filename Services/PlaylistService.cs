using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using StackExchange.Redis;

namespace MusimilarApi.Service
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<ArtistService> _logger;

        public PlaylistService(IConnectionMultiplexer redis, ILogger<ArtistService> logger){

            this._redis = redis;
            this._logger = logger;
        }


    }
}