using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities{

    public class Artist : Entity{

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("genres")]
        public IEnumerable<string> Genres { get; set; }

        [BsonElement("albums")]
        public IEnumerable<Album> Albums { get; set; }

    }

    public class Album{

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("releaseDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReleaseDate { get; set; }

        [BsonElement("totalTracks")]
        public int? TotalTracks { get; set; }
    }
    
}