using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    public class User : Entity
    {
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string PasswordHash { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }
        
        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("subscriptions")]
        public ICollection<string> Subscriptions { get; set; }

        [BsonElement("playlists")]
        public ICollection<Playlist> Playlists { get; set; }
        
    }

    public class Playlist {
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("songs")]
        public ICollection<SongInfo> Songs { get; set; }

        [BsonElement("example")]
        public SongInfo Example { get; set; }

        public Playlist(string name, List<SongInfo> songs, SongInfo info){
            
            this.Name = name;
            this.Songs = songs;  
            this.Example = info;          
        }

    }

    public class SongInfo{

        [BsonElement("songId")]
        public string SongId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("artist")]
        public string Artist { get; set; }

        public SongInfo(string id, string name, string artist)
        {
            this.SongId = SongId;
            this.Name = name;
            this.Artist = artist; 
        }

    }

}