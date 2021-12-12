using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Models.MongoDB.Entities{

    public class Artist : Entity{

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("albums")]
        public List<Album> Albums { get; set; }

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