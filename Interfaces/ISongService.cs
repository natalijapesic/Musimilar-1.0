using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusimilarApi.Interfaces;
using MusimilarApi.MongoDB.Models.Entities;

namespace MusimilarApi.Interfaces{
    public interface ISongService : IEntityService<Song>{

    }
}
