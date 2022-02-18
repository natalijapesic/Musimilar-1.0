using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    public class Playlist {
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("songs")]
        public ICollection<SongInfo> Songs { get; set; }

        [BsonElement("example")]
        public string Example { get; set; }


    }
}