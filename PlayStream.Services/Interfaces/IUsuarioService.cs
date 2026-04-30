using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;

namespace PlayStream.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetUsuarios(UsuarioQueryFilter? filters = null);
        Task<Usuario> GetUsuarioById(int id);
        Task InsertUsuario(Usuario usuario);
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(int id);
    }
}