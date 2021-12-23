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

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song>, ISongService
    {   
        private readonly IPlaylistService _playlistService;
        public SongService(IDatabaseSettings settings, 
                          ILogger<SongService> logger,
                          IMapper mapper,
                          IPlaylistService playlistService)
        :base(settings, settings.SongsCollectionName, logger, mapper){

            _playlistService = playlistService;
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

        public async Task<List<SongInfo>> RecommendPlaylistAsync(PlaylistRequest request)
        {
            Song songExample = await GetSongByNameAsync(request.Name, request.Artist);

            List<Song> songs = await GetSongsByGenreAsync(songExample.Genre);

            List<Song> recommendedSongs = songs.OrderBy(s => Math.Abs(s.AudioFeatures.Energy - songExample.AudioFeatures.Energy) + 
                                                            Math.Abs(s.AudioFeatures.Valence - songExample.AudioFeatures.Valence))
                                                .Take(10)
                                                .ToList();

            List<SongInfo> songInfos = _mapper.Map<List<SongInfo>>(recommendedSongs);
            long numberInput = await _playlistService.CreateSetOfSongs(songInfos, songExample.Id);
            _logger.LogInformation($"Set has {numberInput} songs");
            

            return songInfos;            
        }



        public async Task<Song> GetSongByNameAsync(string name, string artist)
        {
            return await _collection.Find<Song>(s => s.Name == name && s.Artist == artist).FirstAsync();
        }

        public async Task<List<Song>> GetSongsByGenreAsync(string genre)
        {
            return await _collection.Find<Song>(song => song.Genre == genre).ToListAsync();
        }
    }
}