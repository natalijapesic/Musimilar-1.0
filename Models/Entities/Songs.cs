using MongoDB.Bson.Serialization.Attributes;

namespace PComputerApi.Models.Entities{

    public class Track : Entity{

        [BsonElement("artist")]
        public string Artist { get; set; }

        [BsonElement("audioFeatures")]
        public AudioFeatures AudioFeatures { get; set; }

    }

    public class AudioFeatures{

        [BsonElement("acounsticness")]
        public double Acounsticness { get; set; }

        [BsonElement("acounsticness")]
        public double Energy { get; set; }

        [BsonElement("timeSignature")]
        public string TimeSignature { get; set; }

        [BsonElement("valence")]
        public string Valence { get; set; }
    }
    
}