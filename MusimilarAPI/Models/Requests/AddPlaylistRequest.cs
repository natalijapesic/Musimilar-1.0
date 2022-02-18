using System.Collections.Generic;

namespace MusimilarApi.Models.Requests{

    public class AddPlaylistRequest{

        public string UserId { get; set; }
        public string Name { get; set; }
        public ICollection<SongInfoRequest> Songs { get; set; }

        public string Example { get; set; }

    }
}