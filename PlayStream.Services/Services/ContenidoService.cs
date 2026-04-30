using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;
using PlayStream.Core.QueryFilters;
using System.Linq;

namespace PlayStream.Services.Services
{
    public class ContenidoService : IContenidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContenidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Contenido>> GetContenidos(ContenidoQueryFilter? filters = null)
        {
            var contenidos = await _unitOfWork.ContenidoRepository.GetAllAsync();
            if (filters == null) return contenidos;

            if (!string.IsNullOrWhiteSpace(filters.Categoria))
                contenidos = contenidos.Where(c => c.Categoria != null && c.Categoria.ToLower().Contains(filters.Categoria.ToLower()));

            if (filters.AnioLanzamiento.HasValue)
                contenidos = contenidos.Where(c => c.AnioLanzamiento == filters.AnioLanzamiento.Value);

            if (!string.IsNullOrWhiteSpace(filters.Titulo))
                contenidos = contenidos.Where(c => c.Titulo != null && c.Titulo.ToLower().Contains(filters.Titulo.ToLower()));

            if (filters.DuracionMinima.HasValue)
            {
                contenidos = contenidos.Where(c => true);
            }

            return contenidos;
        }
        public async Task<Contenido> GetContenidoById(int id) => await _unitOfWork.ContenidoRepository.GetByIdAsync(id);
        public async Task InsertContenido(Contenido contenido)
        {
            await _unitOfWork.ContenidoRepository.AddAsync(contenido);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateContenido(Contenido contenido)
        {
            await _unitOfWork.ContenidoRepository.UpdateAsync(contenido);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteContenido(int id)
        {
            await _unitOfWork.ContenidoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}