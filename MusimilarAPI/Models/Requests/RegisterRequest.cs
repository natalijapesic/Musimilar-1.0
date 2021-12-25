using AutoMapper.Configuration.Conventions;
using MusimilarApi.Entities.MongoDB;
using MusimilarApi.Models.DTOs;

namespace MusimilarApi.Models.Requests{
    public class RegisterRequest{

      public string Name { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }

      [MapTo(nameof(UserDTO.Role))]
      public string UserRole { get; set; } = Role.User;
    }
}