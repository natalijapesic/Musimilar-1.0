using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Models.MongoDB.Entities
{
    public class User : Entity
    {
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("playlists")]
        public IEnumerable<Playlist> Playlists { get; set; }
        
    }

    public class Playlist {
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("songs")]
        public IEnumerable<ObjectId> Songs { get; set; }

    }



}