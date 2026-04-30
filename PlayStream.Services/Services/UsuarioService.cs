using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;
using PlayStream.Core.QueryFilters;
using System.Linq;

namespace PlayStream.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios(UsuarioQueryFilter? filters = null)
        {
            var usuarios = await _unitOfWork.UsuarioRepository.GetAllAsync();
            if (filters == null) return usuarios;

            if (!string.IsNullOrWhiteSpace(filters.Correo))
                usuarios = usuarios.Where(u => u.Correo != null && u.Correo.ToLower().Contains(filters.Correo.ToLower()));

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                usuarios = usuarios.Where(u => u.Nombre != null && u.Nombre.ToLower().Contains(filters.Nombre.ToLower()));

            return usuarios;
        }
        public async Task<Usuario> GetUsuarioById(int id) => await _unitOfWork.UsuarioRepository.GetByIdAsync(id);
        public async Task InsertUsuario(Usuario usuario)
        {
            usuario.FechaRegistro = DateTime.Now;
            usuario.RolId = 1;
            await _unitOfWork.UsuarioRepository.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateUsuario(Usuario usuario)
        {
            await _unitOfWork.UsuarioRepository.UpdateAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteUsuario(int id)
        {
            await _unitOfWork.UsuarioRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}