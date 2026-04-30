using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;
using PlayStream.Core.QueryFilters;
using Microsoft.Extensions.Logging;

namespace PlayStream.Services.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PerfilService> _logger;

        public PerfilService(IUnitOfWork unitOfWork, ILogger<PerfilService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Perfil>> GetPerfilesByUsuario(int usuarioId)
        {
            var todos = await _unitOfWork.PerfilRepository.GetAllAsync();
            return todos.Where(p => p.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Perfil>> GetPerfiles(PerfilQueryFilter? filters)
        {
            var perfiles = await _unitOfWork.PerfilRepository.GetAllAsync();
            if (filters == null) return perfiles;

            if (filters.UsuarioId.HasValue)
                perfiles = perfiles.Where(p => p.UsuarioId == filters.UsuarioId.Value);

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                perfiles = perfiles.Where(p => p.NombrePerfil != null && p.NombrePerfil.ToLower().Contains(filters.Nombre.ToLower()));

            return perfiles;
        }

        public async Task<Perfil> GetPerfilById(int id) => await _unitOfWork.PerfilRepository.GetByIdAsync(id);

        public async Task InsertPerfil(Perfil perfil)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetByIdAsync(perfil.UsuarioId);
            if (usuario == null)
                throw new Exception("Regla de Negocio: El Usuario asignado no existe.");

            var perfilesActuales = await GetPerfilesByUsuario(perfil.UsuarioId);
            if (perfilesActuales.Count() >= 4)
                throw new Exception("Regla de Negocio: El usuario ha alcanzado el límite máximo de 4 perfiles.");

            _logger.LogInformation("Insertando perfil para UsuarioId={UsuarioId}", perfil.UsuarioId);
            await _unitOfWork.PerfilRepository.AddAsync(perfil);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Perfil insertado con Id={PerfilId}", perfil.Id);
        }
        public async Task UpdatePerfil(Perfil perfil)
        {
            _logger.LogInformation("Actualizando perfil Id={PerfilId}", perfil.Id);
            await _unitOfWork.PerfilRepository.UpdateAsync(perfil);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Perfil actualizado Id={PerfilId}", perfil.Id);
        }

        public async Task DeletePerfil(int id)
        {
            _logger.LogInformation("Eliminando perfil Id={PerfilId}", id);
            await _unitOfWork.PerfilRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Perfil eliminado Id={PerfilId}", id);
        }
    }
}