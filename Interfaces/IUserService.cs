using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB.Entities;

namespace MusimilarApi.Interfaces{
    public interface IUserService : IEntityService<User>{
        Task<string> LogIn(string email, string password);
    }
}
