using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Interfaces;
using MusimilarApi.Models.MongoDB.Entities;

namespace MusimilarApi.Interfaces{
    public interface IArtistService : IEntityService<Artist>{

        Task<ArtistNode> InserNodeAsync(Artist a);
        Task<List<string>> ConnectArtistWithGenresAsync(string id, List<string> genres);

    }
}
