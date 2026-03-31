using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;

namespace PlayStream.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _usuarioRepository;
        public UsuarioService(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios() => await _usuarioRepository.GetAllAsync();
        public async Task<Usuario> GetUsuarioById(int id) => await _usuarioRepository.GetByIdAsync(id);
        public async Task InsertUsuario(Usuario usuario)
        {
            usuario.FechaRegistro = DateTime.Now;
            usuario.RolId = 1;
            await _usuarioRepository.AddAsync(usuario);
        }
        public async Task UpdateUsuario(Usuario usuario) => await _usuarioRepository.UpdateAsync(usuario);
        public async Task DeleteUsuario(int id) => await _usuarioRepository.DeleteAsync(id);
    }
}