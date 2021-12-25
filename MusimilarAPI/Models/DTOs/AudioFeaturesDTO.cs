namespace MusimilarApi.Models.DTOs{

    public class AudioFeaturesDTO
    {
        public double Tempo { get; set; }
        public double Energy { get; set; }
        public double Speechiness { get; set; }
        public double Danceability { get; set; }
        public double DurationMS { get; set; }
        public double Valence { get; set; }
    }
}