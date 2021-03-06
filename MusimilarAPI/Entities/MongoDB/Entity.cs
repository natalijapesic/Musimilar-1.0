using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB{
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public virtual string Id { get; set; }

        [BsonElement("name")]
        public virtual string Name { get; set; }

    }
}

    