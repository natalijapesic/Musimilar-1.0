using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    //song in playlist
    public class SongInfo{

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("artist")]
        public string Artist { get; set; }

        public SongInfo(string name, string artist)
        {
            this.Name = name;
            this.Artist = artist; 
        }

    }
}