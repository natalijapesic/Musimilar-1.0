using System.Collections.Generic;

namespace MusimilarApi.Models.Responses{
    public class PlaylistResponse {
        public string Name { get; set; }
        public List<SongInfoResponse> Songs { get; set; }
        public SongInfoResponse Example { get; set; }

        public PlaylistResponse(string name, List<SongInfoResponse> songs, SongInfoResponse info){
            
            this.Name = name;
            this.Songs = songs;  
            this.Example = info;          
        }

    }

}