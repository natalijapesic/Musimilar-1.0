
using System.Collections.Generic;

namespace MusimilarApi.Models.Requests{

    public class SubscribeGenreRequest{

        public string UserId { get; set; }
        public List<string> Genres { get; set; }

    }
}