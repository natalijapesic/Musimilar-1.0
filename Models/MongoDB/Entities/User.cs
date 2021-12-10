using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<Playlist> Playlists { get; set; }
        
    }

    public class Playlist {

        public string Name { get; set; }
        public IEnumerable<ObjectId> Songs { get; set; }

    }



}