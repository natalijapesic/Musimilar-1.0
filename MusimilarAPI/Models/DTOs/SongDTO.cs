namespace MusimilarApi.Models.DTOs{
    public class SongDTO : DTO
    {
      public string Artist { get; set; }
      public string Genre { get; set; }
      public AudioFeaturesDTO AudioFeatures { get; set; }
    }
}