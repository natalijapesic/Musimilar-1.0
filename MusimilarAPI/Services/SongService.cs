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
using MusimilarApi.Models.DTOs;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song, SongDTO>, ISongService
    {   
        private readonly IPlaylistService _playlistService;
        public SongService(IDatabaseSettings settings, 
                          ILogger<SongService> logger,
                          IMapper mapper,
                          IPlaylistService playlistService)
        :base(settings, settings.SongsCollectionName, logger, mapper){

            _playlistService = playlistService;
        }

        public override async Task<SongDTO> InsertAsync(SongDTO request){

            string genre = DetermineGenre(request.AudioFeatures);
            request.Genre = genre;

            await _collection.InsertOneAsync(_mapper.Map<Song>(request));

            return request;
        }

        public override async Task<ICollection<SongDTO>> InsertManyAsync(ICollection<SongDTO> requests){

            string genre = null;

            foreach (var request in requests)
            {
                genre = DetermineGenre(request.AudioFeatures);

                request.Genre = genre;
            }

            await _collection.InsertManyAsync(_mapper.Map<IEnumerable<Song>>(requests));

            return requests;

        }

        public string DetermineGenre(AudioFeaturesDTO song){

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

        public async Task<List<SongInfoDTO>> RecommendPlaylistAsync(SongInfoDTO request, SongDTO songExample)
        {
            List<SongDTO> songs = await GetSongsByGenreAsync(songExample.Genre);

            List<SongDTO> recommendedSongs = songs.OrderBy(s => Math.Abs(s.AudioFeatures.Energy - songExample.AudioFeatures.Energy) + 
                                                            Math.Abs(s.AudioFeatures.Valence - songExample.AudioFeatures.Valence))
                                                  .Take(10)
                                                  .ToList();

            List<SongInfoDTO> songInfos = _mapper.Map<List<SongInfoDTO>>(recommendedSongs);
            long numberInput = await _playlistService.CreateSetOfSongsAsync(songInfos, songExample.Genre, songExample.Id);
            _logger.LogInformation($"Set has {numberInput} songs");
            
            return songInfos;            
        }



        public async Task<SongDTO> GetSongByNameAsync(string name, string artist)
        {
            Song song = await _collection.Find<Song>(s => s.Name == name && s.Artist == artist).FirstAsync();
            return _mapper.Map<SongDTO>(song);
        }

        public async Task<List<SongDTO>> GetSongsByGenreAsync(string genre)
        {
            List<Song> songs = await _collection.Find<Song>(song => song.Genre == genre).ToListAsync();
            return _mapper.Map<List<SongDTO>>(songs);
        }

    }
}