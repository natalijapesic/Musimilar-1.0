using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Models.MongoDB.Entities{

    public class Song : Entity{

        [BsonElement("artist")]
        public string Artist { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("audioFeatures")]
        public AudioFeatures AudioFeatures { get; set; }

    }

    public class AudioFeatures{

        [BsonElement("tempo")]
        public double Tempo { get; set; }

        [BsonElement("energy")]
        public double Energy { get; set; }

        [BsonElement("speechiness")]
        public double Speechiness { get; set; }

        [BsonElement("danceability")]
        public double Danceability { get; set; }

        [BsonElement("durationMS")]
        public double DurationMS { get; set; }

        [BsonElement("valence")]
        public double Valence { get; set; }

    }
    
}