using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB{

    public class Song : Entity{

        [BsonElement("artist")]
        public string Artist { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("audioFeatures")]
        public AudioFeatures AudioFeatures { get; set; }

    }
    
}