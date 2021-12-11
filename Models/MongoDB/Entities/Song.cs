using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities{

    public class Song : Entity{

        [BsonElement("artist")]
        public string Artist { get; set; }

        [BsonElement("audioFeatures")]
        public AudioFeatures AudioFeatures { get; set; }

    }

    public class AudioFeatures{

        [BsonElement("acounsticness")]
        public double? Acounsticness { get; set; }

        [BsonElement("energy")]
        public double? Energy { get; set; }

        [BsonElement("timeSignature")]
        public double? TimeSignature { get; set; }

        [BsonElement("valence")]
        public double? Valence { get; set; }
    }
    
}