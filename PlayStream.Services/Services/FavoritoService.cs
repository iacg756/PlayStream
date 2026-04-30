using PlayStream.Core.Entities;
using PlayStream.Core.Exceptions;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;
using System.Net;
using PlayStream.Core.QueryFilters;
using System.Linq;

namespace PlayStream.Services.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoritoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddFavorito(Favorito favorito)
        {
            var lista = await _unitOfWork.FavoritoRepository.GetAllAsync();
            if (lista.Any(f => f.PerfilId == favorito.PerfilId && f.ContenidoId == favorito.ContenidoId))
                throw new BussinesException("Este contenido ya está en tus favoritos", HttpStatusCode.BadRequest);

            await _unitOfWork.FavoritoRepository.AddAsync(favorito);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Favorito>> GetFavoritosByPerfil(int perfilId)
        {
            var res = await _unitOfWork.FavoritoRepository.GetAllAsync();
            return res.Where(x => x.PerfilId == perfilId);
        }

        public async Task<IEnumerable<Favorito>> GetFavoritos(FavoritoQueryFilter? filters)
        {
            var favoritos = await _unitOfWork.FavoritoRepository.GetAllAsync();
            if (filters == null) return favoritos;

            if (filters.PerfilId.HasValue)
                favoritos = favoritos.Where(f => f.PerfilId == filters.PerfilId.Value);

            if (filters.ContenidoId.HasValue)
                favoritos = favoritos.Where(f => f.ContenidoId == filters.ContenidoId.Value);

            return favoritos;
        }

        public async Task DeleteFavorito(int id)
        {
            await _unitOfWork.FavoritoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}