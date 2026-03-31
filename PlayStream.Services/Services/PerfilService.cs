using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;

namespace PlayStream.Services.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IRepository<Perfil> _perfilRepository;
        private readonly IRepository<Usuario> _usuarioRepository;

        public PerfilService(IRepository<Perfil> perfilRepository, IRepository<Usuario> usuarioRepository)
        {
            _perfilRepository = perfilRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Perfil>> GetPerfilesByUsuario(int usuarioId)
        {
            var todos = await _perfilRepository.GetAllAsync();
            return todos.Where(p => p.UsuarioId == usuarioId);
        }

        public async Task<Perfil> GetPerfilById(int id) => await _perfilRepository.GetByIdAsync(id);

        public async Task InsertPerfil(Perfil perfil)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(perfil.UsuarioId);
            if (usuario == null)
                throw new Exception("Regla de Negocio: El Usuario asignado no existe.");

            var perfilesActuales = await GetPerfilesByUsuario(perfil.UsuarioId);
            if (perfilesActuales.Count() >= 4)
                throw new Exception("Regla de Negocio: El usuario ha alcanzado el límite máximo de 4 perfiles.");

            await _perfilRepository.AddAsync(perfil);
        }

        public async Task UpdatePerfil(Perfil perfil) => await _perfilRepository.UpdateAsync(perfil);
        public async Task DeletePerfil(int id) => await _perfilRepository.DeleteAsync(id);
    }
}