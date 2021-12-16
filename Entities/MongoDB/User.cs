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
        public string Password { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }
        
        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("playlists")]
        public IEnumerable<Playlist> Playlists { get; set; }
        
    }

    public class Playlist {
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("songs")]
        public List<string> Songs { get; set; }

        [BsonElement("songId")]
        public string SongId { get; set; }

        public Playlist(string name, List<string> songs, string songId){
            
            this.Name = name;
            this.Songs = songs;
            this.SongId = songId;
            
        }

    }



}