using System.Collections.Generic;
using System.Threading.Tasks;
using MusimilarApi.Entities.Neo4j;

namespace MusimilarApi.Interfaces
{
    public interface IArtistService {

        Task<ArtistNode> InserNodeAsync(ArtistNode a);
        Task<List<string>> ConnectArtistWithGenresAsync(string name, List<string> genres);
        Task<List<ArtistNode>> GetArtistNodesByGenreAsync(string genre);
        Task<List<ArtistNode>> GetSimilarArtistsAsync(string artistName);
        Task DeleteAsync(string name);

        Task<ArtistNode> GetArtistAsync(string artistName);

    }
}
