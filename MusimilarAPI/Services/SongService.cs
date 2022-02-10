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

            if(request.Artist == null || request.Name == null || request.AudioFeatures == null)
                return null;

            string genre = DetermineGenre(request.AudioFeatures);

            request.Genre = genre;

            await _collection.InsertOneAsync(_mapper.Map<Song>(request));

            return request;
        }

        public override async Task<List<SongDTO>> InsertManyAsync(List<SongDTO> requests){

            string genre = null;
            int i = 0;

            while(requests.Count > 0 && i < requests.Count)
            {
                if(requests[i].Artist == null || requests[i].Name == null || requests[i].AudioFeatures == null)
                {
                    requests.RemoveAt(i);
                    continue;
                }    

                genre = DetermineGenre(requests[i].AudioFeatures);

                requests[i].Genre = genre;
                i++;
            }

            if(requests.Count == 0)
                return null;

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

        public async Task<List<SongInfoDTO>> RecommendPlaylistAsync(SongDTO songExample)
        {
            List<SongDTO> songs = await GetSongsByGenreAsync(songExample.Genre);

            if(songs == null)
                return null;
                
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
            try{
                Song song = await _collection.Find<Song>(s => s.Name == name && s.Artist == artist).FirstAsync();
                return _mapper.Map<SongDTO>(song);

            }catch(Exception exception){

                this._logger.LogError($"GetSongBy name {name} and artistName {artist} throws {exception}");
                return null;
            }
            
        }

        public async Task<List<SongDTO>> GetSongsByGenreAsync(string genre)
        {
            try{

                List<Song> songs = await _collection.Find<Song>(song => song.Genre == genre).ToListAsync();
                return _mapper.Map<List<SongDTO>>(songs);

            }catch(Exception exception){

                this._logger.LogError($"GetSongsBy genre {genre} throws {exception}");
                return null;
            }
        }

    }
}