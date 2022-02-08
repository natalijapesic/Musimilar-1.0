using System;
using System.Collections.Generic;

namespace MusimilarApi.Models.Responses
{
    public class PlaylistFeedResponse {
        
        public DateTime Date { get; set; }
        public List<SongInfoResponse> Songs { get; set; }

        public string Example { get; set; }

        public PlaylistFeedResponse(DateTime date, List<SongInfoResponse> songs, string example){
            
            this.Date = date;
            this.Songs = songs;  
            this.Example = example;          
        }

    }
}