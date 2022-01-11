using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MusimilarApi.Entities.MongoDB
{
    public class Playlist {
        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("songs")]
        public ICollection<SongInfo> Songs { get; set; }

        [BsonElement("example")]
        public SongInfo Example { get; set; }

        // public Playlist(string name, List<SongInfo> songs, SongInfo info){
            
        //     this.Name = name;
        //     this.Songs = songs;  
        //     this.Example = info;          
        // }

    }
}