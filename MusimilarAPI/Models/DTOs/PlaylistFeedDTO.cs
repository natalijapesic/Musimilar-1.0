using System;
using System.Collections.Generic;

namespace MusimilarApi.Models.DTOs
{
    public class PlaylistFeedDTO {
        
        public DateTime Date { get; set; }
        public List<SongInfoDTO> Songs { get; set; }

        public string Example { get; set; }

        public PlaylistFeedDTO(DateTime date, List<SongInfoDTO> songs, string example){
            
            this.Date = date;
            this.Songs = songs;  
            this.Example = example;          
        }

    }
}