using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;

namespace PlayStream.Services.Interfaces
{
    public interface IContenidoService
    {
        Task<IEnumerable<Contenido>> GetContenidos(ContenidoQueryFilter? filters = null);
        Task<Contenido> GetContenidoById(int id);
        Task InsertContenido(Contenido contenido);
        Task UpdateContenido(Contenido contenido);
        Task DeleteContenido(int id);
    }
}