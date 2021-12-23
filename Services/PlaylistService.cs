using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Interfaces;
using StackExchange.Redis;

namespace MusimilarApi.Service
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IDatabase _redisClient;
        private readonly ILogger<ArtistService> _logger;

        public PlaylistService(IConnectionMultiplexer redis, ILogger<ArtistService> logger){

            _redisClient = redis.GetDatabase();
            _logger = logger;
        }

        public Task AddNewPlaylist(string genre, Playlist playlist)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> CreateSetOfSongs(List<SongInfo> songs, string songId)
        {
            int length = songs.Count;
            string songName = null;
            SortedSetEntry[] playlist= new SortedSetEntry[length];

            for (int i = 0; i < length; i++)
            {
                songName = $"{songs[i].Artist} - {songs[i].Name}";

                playlist[i] = new SortedSetEntry(songName, DateTime.Now.ToOADate());
            }
            string key = $"playlist:{songId}";

            return await _redisClient.SortedSetAddAsync(key, playlist);
        }

        public async Task<bool> DeleteKey(string key)
        {
            return await _redisClient.KeyDeleteAsync(key);
        }

        public async Task<bool> DoesKeyExist(string key)
        {
            return await _redisClient.KeyExistsAsync(key);
        }

        public Task<int> ExtendTTL(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GenerateId(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> LikePlaylist(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}