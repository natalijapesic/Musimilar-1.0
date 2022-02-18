using System.Collections.Generic;

namespace MusimilarApi.Models.Responses{
    public class PlaylistResponse{
        public string Name { get; set; }
        public List<SongInfoResponse> Songs { get; set; }
        public string Example { get; set; }

    }

}