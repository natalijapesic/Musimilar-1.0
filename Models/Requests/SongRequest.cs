namespace MusimilarApi.Models.Requests{
    public class SongRequest{
      public string Name { get; set; }
      public string Artist { get; set; }
      
      public AudioFeature AudioFeatures { get; set; }
    }

    public class AudioFeature
    {
        public double Tempo { get; set; }
        public double Energy { get; set; }
        public double Speechiness { get; set; }
        public double Danceability { get; set; }
        public double DurationMS { get; set; }
        public double Valence { get; set; }
    }
}