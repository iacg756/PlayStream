using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayStream.Core.Entities;

namespace PlayStream.Services.Interfaces
{
    public interface IContenidoService
    {
        Task<IEnumerable<Contenido>> GetContenidos();
        Task<Contenido> GetContenidoById(int id);
        Task InsertContenido(Contenido contenido);
        Task UpdateContenido(Contenido contenido);
        Task DeleteContenido(int id);
    }
}