using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;
namespace PlayStream.Services.Interfaces
{
    public interface IFavoritoService
    {
        Task<IEnumerable<Favorito>> GetFavoritosByPerfil(int perfilId);
        Task<IEnumerable<Favorito>> GetFavoritos(FavoritoQueryFilter? filters);
        Task AddFavorito(Favorito favorito);
        Task DeleteFavorito(int id);
    }
}