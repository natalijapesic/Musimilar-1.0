using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities{

    public class Artist : Entity{

        public string Image { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<Album> Albums { get; set; }

    }

    public class Album{

        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReleaseDate { get; set; }

        public int? TotalTracks { get; set; }
    }
    
}