using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    public class SongInfo{

        [BsonElement("songId")]
        public string SongId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("artist")]
        public string Artist { get; set; }

        public SongInfo(string id, string name, string artist)
        {
            this.SongId = SongId;
            this.Name = name;
            this.Artist = artist; 
        }

    }
}