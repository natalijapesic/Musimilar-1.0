using System.Collections.Generic;

namespace MusimilarApi.Models.DTOs
{
    public class PlaylistDTO
    {
        public string Name { get; set; }
        public List<SongInfoDTO> Songs { get; set; }

        public string Example { get; set; }

        // public PlaylistDTO(string name, List<SongInfoDTO> songs, SongInfoDTO info){
            
        //     this.Name = name;
        //     this.Songs = songs;  
        //     this.Example = info;          
        // }

    }
}