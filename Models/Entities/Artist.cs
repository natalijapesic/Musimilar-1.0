using MongoDB.Bson.Serialization.Attributes;

namespace PComputerApi.Models.Entities{

    public class Motherboard : Entity{

        [BsonElement("socket_/_cpu")]
        public string SocketCpu { get; set; }

        [BsonElement("modules")]
        public string Modules { get; set; }

        [BsonElement("form_factor")]
        public string FormFactor { get; set; }

        [BsonElement("memory_max")]
        public string MemoryMaxGB { get; set; }

        [BsonElement("memory_slots")]
        public int MemorySlots { get; set; }

        [BsonElement("color")]
        public string Color { get; set; }

    }
    
}