using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task<IEnumerable<Song>> InsertSongManyAsync(IEnumerable<SongRequest> requests){

            string genre = null;
            Song song = null;
            List<Song> songs = new List<Song>();

            foreach (var request in requests)
            {
                genre = DetermineGenre(request.AudioFeatures);

                song = _mapper.Map<Song>(request);
                song.Genre = genre;

                songs.Add(song);
            }

            await _collection.InsertManyAsync(songs);

            return songs;

        }

        public string DetermineGenre(AudioFeaturesRequest song){

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

        public async Task<Playlist> RecommendPlaylistAsync(PlaylistRequest request)
        {
            //ovde proveri da li vec postoji generisana pl u Redisu na osnovu pesme
            Song songExample = await GetSongByNameAsync(request.SongExample);
            
            if(songExample == null)
            {
                this._logger.LogError("Song doesnt exist in database");
                return null;
            }  

            IEnumerable<Song> recommendedSongs = await GetSongsByGenreAsync(songExample.Genre);

            List<SongInfo> recommendedNames = await _collection.AsQueryable()
                                                            .Where<Song>(songs => songs.Genre == songExample.Genre) 
                                                            .OrderBy(s => Math.Abs(s.AudioFeatures.Energy - songExample.AudioFeatures.Energy) + 
                                                                          Math.Abs(s.AudioFeatures.Valence - songExample.AudioFeatures.Valence))
                                                            .Take(request.NumberOfSongs)
                                                            .Select(song => new SongInfo(song.Id, song.Name, song.Artist))
                                                            .ToListAsync();


            Playlist newPlaylist = new Playlist(request.PlaylistName, recommendedNames, new SongInfo(songExample.Id, songExample.Name, songExample.Artist));

            return newPlaylist;            
        }

        public async Task<Song> GetSongByNameAsync(string name)
        {
            return await _collection.Find<Song>(s => s.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<Song>> GetSongsByGenreAsync(string genre)
        {
            return await _collection.Find<Song>(song => song.Genre == genre).ToListAsync();
        }
    }
}