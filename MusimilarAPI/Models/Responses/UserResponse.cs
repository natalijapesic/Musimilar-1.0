using System.Collections.Generic;

namespace MusimilarApi.Models.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Subscriptions { get; set; }
        public IEnumerable<PlaylistResponse> Playlists { get; set; }
        
    }

}