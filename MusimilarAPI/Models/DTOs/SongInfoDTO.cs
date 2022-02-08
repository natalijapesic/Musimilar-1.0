namespace MusimilarApi.Models.DTOs{
    //song in playlist
    public class SongInfoDTO 
    {
        public string Artist { get; set; }
        public string Name { get; set; }

        public SongInfoDTO(string artist, string name)
        {
            this.Name = name;
            this.Artist = artist; 
        }
    }
}