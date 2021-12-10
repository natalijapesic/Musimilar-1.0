using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.MongoDB.Models.Entities{
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

    }
}

    