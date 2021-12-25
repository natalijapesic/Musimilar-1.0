    namespace MusimilarApi.Models.Responses{
    public class SongInfoResponse{

        public string Name { get; set; }
        public string Artist { get; set; }

        public SongInfoResponse(string name, string artist)
        {
            this.Name = name;
            this.Artist = artist; 
        }

    }
}