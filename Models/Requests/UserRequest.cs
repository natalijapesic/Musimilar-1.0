using MusimilarApi.Entities.MongoDB;

namespace MusimilarApi.Models.Requests{

    public class UserRequest{

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        
    }
}