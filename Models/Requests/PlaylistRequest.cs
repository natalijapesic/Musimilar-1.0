using System.Collections.Generic;

namespace MusimilarApi.Models.Requests{
    public class PlaylistRequest{
      public int NumberOfSongs { get; set; }
      public string SongExample { get; set; }
      public string PlaylistName { get; set; }
    }
}