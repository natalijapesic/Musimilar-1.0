using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.DTOs;
using StackExchange.Redis;

namespace MusimilarApi.Service
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IDatabase _redisClient;
        private readonly ISongService _songService;
        private readonly ILogger<ArtistService> _logger;

        public PlaylistService( IConnectionMultiplexer redis, 
                                ILogger<ArtistService> logger,
                                ISongService songService
                                ){

            _redisClient = redis.GetDatabase();
            _logger = logger;
            this._songService = songService;
        }

        public async Task AddNewAsync(string genre, string songId)
        {
            string key = $"playlist:{genre}";
            string data = $"playlist:{songId}";

            await _redisClient.SortedSetAddAsync(key, data, DateTime.Now.ToOADate());

            await _redisClient.SortedSetRemoveRangeByRankAsync(key, -1, -50); //proveriti ovo
        }

        public async Task<long> CreateSetOfSongsAsync(List<SongInfoDTO> songs, string genre, string songId)
        {
            int length = songs.Count;
            string songName = null;
            SortedSetEntry[] playlist= new SortedSetEntry[length];

            for (int i = length - 1; i >= 0; i--)
            { 
                songName = $"{songs[i].Artist.ToLower()} - {songs[i].Name.ToLower()}"; 

                playlist[i] = new SortedSetEntry(songName, i); 
            }
            string key = $"playlist:{songId}";

            await _redisClient.KeyExpireAsync(key, DateTime.Today.AddDays(1));

            var result =  await _redisClient.SortedSetAddAsync(key, playlist);

            await _redisClient.SortedSetRemoveRangeByRankAsync(key, -1, -100); //proveri ovo 0, -100

            if(result > 0)
                await AddNewAsync(genre, songId);

            return result;
        }


        public async Task<double> LikeAsync(string songId)
        {
            SongDTO song = await this._songService.GetAsync(songId);

            if(song == null)
                return -1;
            string key = "playlist:leadboard";
            string data = $"playlist:{songId}";
            var result = await this._redisClient.SortedSetIncrementAsync(key, data, 1);

            //cuvam njih 50, prikazujem njih 10
            await _redisClient.SortedSetRemoveRangeByRankAsync(key, 0, -50); //proveri ovo 0, -100

            return result;
        }

        public async Task<List<PlaylistFeedDTO>> UsersFeedAsync(List<string> subscriptions)
        {
            var len = subscriptions.Count;
            SortedSetEntry[] playlists = null;

            if(len == 1)
                playlists =  await this._redisClient.SortedSetRangeByRankWithScoresAsync($"playlist:{subscriptions[0]}", 0, 50, Order.Descending);
            else if(len > 1)
            {
                RedisKey[] keys = subscriptions.Select(key => (RedisKey)$"playlist:{key}").ToArray();

                await this._redisClient.SortedSetCombineAndStoreAsync(SetOperation.Union, "union", keys);

                playlists = await this._redisClient.SortedSetRangeByRankWithScoresAsync("union", 0, -1, Order.Descending);
                
                await this._redisClient.SortedSetRemoveRangeByRankAsync("union", 0, -1);
            }
            else playlists = null;

            List<PlaylistFeedDTO> playlistFeedDTOs = new List<PlaylistFeedDTO>();
            PlaylistFeedDTO playlistFeed = null;

            foreach (var playlist in playlists)
            {
                RedisValue[] songs = await this._redisClient.SortedSetRangeByRankAsync(playlist.Element.ToString(), 0, -1, Order.Descending);

                if(songs.Length == 0)
                    continue; 

                string[] songsString = ToStringArray(songs);

                string[] exampleId = playlist.Element.ToString().Split(":"); // $"playlist:{songId}"

                playlistFeed = new PlaylistFeedDTO(DateTime.FromOADate(playlist.Score), ConvertStringSongs(songsString), exampleId[1]);

                playlistFeedDTOs.Add(playlistFeed);
            }

            return playlistFeedDTOs;
                
        }

        private string[] ToStringArray(RedisValue[] values){

            if (values == null) return null;
            if (values.Length == 0) return null;
            return Array.ConvertAll(values, x => x.ToString());
        }


        private List<SongInfoDTO> ConvertStringSongs(string[] songs){

            if (songs == null) return null;
            if (songs.Length == 0) return null;

            List<SongInfoDTO> convertedSongs = new List<SongInfoDTO>();
            foreach (var song in songs)
            {
                string[] s = song.Split(" "); // $"{songs[i].Artist} - {songs[i].Name}"
                SongInfoDTO convertedSong = new SongInfoDTO(s[0], s[2]);
                convertedSongs.Add(convertedSong);
            }

            return convertedSongs;
        }

        public async Task<List<SongInfoDTO>> GetAsync(string songId)
        {
            string key = $"playlist:{songId}";
            RedisValue[] songs =  await this._redisClient.SortedSetRangeByRankAsync(key, 0, -1, Order.Descending);

            if(songs.Length > 0)
            {
                var result = await _redisClient.KeyExpireAsync(key, DateTime.Today.AddDays(2));
                string[] songsString = ToStringArray(songs);
                return ConvertStringSongs(songsString);
            }
            
            return null;
        }
    }
}