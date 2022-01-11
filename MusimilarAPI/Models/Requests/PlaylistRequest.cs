using System.Collections.Generic;

namespace MusimilarApi.Models.Requests
{
    public class PlaylistRequest {
    
        public string Name { get; set; }
        public ICollection<SongInfoRequest> Songs { get; set; }

        public SongInfoRequest Example { get; set; }

        public PlaylistRequest(string name, List<SongInfoRequest> songs, SongInfoRequest info){
            
            this.Name = name;
            this.Songs = songs;  
            this.Example = info;          
        }

    }
}