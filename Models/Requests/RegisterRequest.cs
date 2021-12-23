using AutoMapper.Configuration.Conventions;
using MusimilarApi.Entities.MongoDB;

namespace MusimilarApi.Models.Requests{
    public class RegisterRequest{

      public string Name { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }

      [MapTo(nameof(User.Role))]
      public string UserRole { get; set; } = Role.User;
    }
}