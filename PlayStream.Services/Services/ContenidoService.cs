using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;

namespace PlayStream.Services.Services
{
    public class ContenidoService : IContenidoService
    {
        private readonly IRepository<Contenido> _contenidoRepository;
        public ContenidoService(IRepository<Contenido> contenidoRepository)
        {
            _contenidoRepository = contenidoRepository;
        }

        public async Task<IEnumerable<Contenido>> GetContenidos() => await _contenidoRepository.GetAllAsync();
        public async Task<Contenido> GetContenidoById(int id) => await _contenidoRepository.GetByIdAsync(id);
        public async Task InsertContenido(Contenido contenido) => await _contenidoRepository.AddAsync(contenido);
        public async Task UpdateContenido(Contenido contenido) => await _contenidoRepository.UpdateAsync(contenido);
        public async Task DeleteContenido(int id) => await _contenidoRepository.DeleteAsync(id);
    }
}