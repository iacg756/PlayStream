using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;

namespace PlayStream.Services.Interfaces
{
    public interface IPerfilService
    {
        Task<IEnumerable<Perfil>> GetPerfilesByUsuario(int usuarioId);
        Task<Perfil> GetPerfilById(int id);
        Task InsertPerfil(Perfil perfil);
        Task UpdatePerfil(Perfil perfil);
        Task DeletePerfil(int id);
    }
}