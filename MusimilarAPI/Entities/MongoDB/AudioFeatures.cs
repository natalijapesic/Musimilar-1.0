using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    public class AudioFeatures{

        [BsonElement("energy")]
        public double Energy { get; set; }

        [BsonElement("valence")]
        public double Valence { get; set; }

    }
}