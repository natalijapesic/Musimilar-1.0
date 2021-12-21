using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Helpers;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.Requests;
using MusimilarApi.Services;
using StackExchange.Redis;

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song>, ISongService
    {
        private readonly IConnectionMultiplexer _redis;
        public SongService(IDatabaseSettings settings, 
                          ILogger<SongService> logger, 
                          IConnectionMultiplexer redis,
                          IMapper mapper)
        :base(settings, settings.SongsCollectionName, logger, mapper){
            _redis = redis;
        }

        public async Task<Song> InsertSongAsync(SongRequest request){

            string genre = DetermineGenre(request.AudioFeatures);

            Song song = _mapper.Map<Song>(request);

            song.Genre = genre;

            await _collection.InsertOneAsync(song);
            return song;
        }

        public string DetermineGenre(AudioFeature song){

            if(song.Speechiness >= 0.22)
            {    
                return SongGenre.Rap;
            }

            if(song.Danceability < 0.65)
            {
                return SongGenre.Rock;
            }    
            else if(song.Tempo >= 0.04)
            {
                return SongGenre.Edm;
            }        
            else if(song.DurationMS >= 0.36)
            {
                return SongGenre.Rb;
            }
            else if(song.Danceability < 0.38)
            {
                return SongGenre.Pop;
            }    
            else
                return SongGenre.Latin;        
        }

        // public async Task<Playlist> RecommendPlaylistAsync(PlaylistRequest request)
        // {
        //     Song songExample = await GetSongByNameAsync(request.SongExample);
            
        //     if(songExample == null)
        //     {
        //         this._logger.LogError("Song doesnt exist in database");
        //         return null;
        //     }  

        //     List<Song> recommendedSongs = await GetSongsByGenreAsync(songExample.Genre);

        //     List<SongInfo> recommendedNames = recommendedSongs.OrderBy(s => Math.Abs(s.AudioFeatures.Energy - songExample.AudioFeatures.Energy) + 
        //                                                                   Math.Abs(s.AudioFeatures.Valence - songExample.AudioFeatures.Valence))
        //                                                     .Take(request.NumberOfSongs)
        //                                                     .Select(song => new SongInfo(song.Name, song.Artist))
        //                                                     .ToList();

        //     Playlist newPlaylist = new Playlist(request.PlaylistName, recommendedNames, new SongInfo(songExample.Name, songExample.Artist));

        //     ISubscriber subscriber = _redis.GetSubscriber();
        //     await subscriber.PublishAsync(songExample.Genre, JsonSerializer.Serialize<Playlist>(newPlaylist));

        //     return newPlaylist;            
        // }

        public async Task<Song> GetSongByNameAsync(string name)
        {
            return await _collection.Find<Song>(s => s.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByGenreAsync(string genre)
        {
            return await _collection.Find<Song>(song => song.Genre == genre).ToListAsync();
        }
    }
}