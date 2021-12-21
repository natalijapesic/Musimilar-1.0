using System.Collections.Generic;

namespace MusimilarApi.Models.Responses
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Subscriptions { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
        
    }

    public class Playlist {
        
        public string Name { get; set; }
        public ICollection<SongInfo> Songs { get; set; }
        public SongInfo Example { get; set; }

        public Playlist(string name, List<SongInfo> songs, SongInfo info){
            
            this.Name = name;
            this.Songs = songs;  
            this.Example = info;          
        }

    }

    public class SongInfo{

        public string Name { get; set; }
        public string Artist { get; set; }

        public SongInfo(string name, string artist)
        {
            this.Name = name;
            this.Artist = artist; 
        }

    }

}