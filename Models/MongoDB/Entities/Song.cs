using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities{

    public class Song : Entity{

        public string Artist { get; set; }

        public AudioFeatures AudioFeatures { get; set; }

    }

    public class AudioFeatures{

        public double? Acounsticness { get; set; }

        public double? Energy { get; set; }

        public double? TimeSignature { get; set; }

        public double? Valence { get; set; }
    }
    
}