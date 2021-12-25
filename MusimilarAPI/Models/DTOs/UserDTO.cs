using System.Collections.Generic;

namespace MusimilarApi.Models.DTOs
{
    public class UserDTO : DTO
    {
        public string Email { get; set; }

        public string Role { get; set; }
        public string Password { get; set; }
        
        public string Token { get; set; }

        public ICollection<string> Subscriptions { get; set; }
        public ICollection<PlaylistDTO> Playlists { get; set; }
        
    }

}