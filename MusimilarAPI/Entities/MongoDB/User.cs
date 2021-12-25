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

}