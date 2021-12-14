using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB;
using MusimilarApi.Models.MongoDB.Entities;
using MusimilarApi.Models.Requests;
using MusimilarApi.Services;

namespace MusimilarApi.Service
{
    public class SongService: EntityService<Song>, ISongService
    {

        public SongService(IDatabaseSettings settings, ILogger<SongService> logger)
        :base(settings, settings.SongsCollectionName, logger){}

        public async Task<Song> InsertSongAsync(Song song){

            string genre = DetermineGenre(song.AudioFeatures);
            song.Genre = genre;

            await _collection.InsertOneAsync(song);
            return song;
        }

        public string DetermineGenre(AudioFeatures song){

            if(song.Speechiness >= 0.22)
                return "rap";

            if(song.Danceability < 0.65)
            {
                return "rock";
            }    
            else if(song.Tempo >= 0.04)
            {
                return "edm";
            }        
            else if(song.DurationMS >= 0.36)
            {
                return "r&b";
            }
            else if(song.Danceability < 0.38)
            {
                return "pop";
            }    
            else
                return "latin";        
        }

        public async Task<Playlist> RecommendPlaylistAsync(PlaylistRequest request)
        {
            Song songExample = await GetSongByNameAsync(request.SongExample);
            if(songExample == null)
            {
                this._logger.LogError("Song doesnt exist in database");
                return null;
            }  

            List<Song> recommendedSongs = await GetSongsByGenreAsync(songExample.Genre);

            List<string> recommendedNames = recommendedSongs.OrderBy(s => Math.Abs(s.AudioFeatures.Energy - songExample.AudioFeatures.Energy) + 
                                                                          Math.Abs(s.AudioFeatures.Valence - songExample.AudioFeatures.Valence))
                                                            .Take(request.NumberOfSongs)
                                                            .Select(song => song.Name)
                                                            .ToList();

             

            Playlist newPlaylist = new Playlist(request.PlaylistName, recommendedNames);

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