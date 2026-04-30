using PlayStream.Core.Entities;
using PlayStream.Core.Exceptions;
using PlayStream.Core.Interfaces;
using PlayStream.Services.Interfaces;
using System.Net;
using PlayStream.Core.QueryFilters;

namespace PlayStream.Services.Services
{
    public class CalificacionService : ICalificacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperContext _dapper;

        public CalificacionService(IUnitOfWork unitOfWork, IDapperContext dapper)
        {
            _unitOfWork = unitOfWork;
            _dapper = dapper;
        }

        public async Task<IEnumerable<Calificacion>> GetCalificacionesByContenido(int contenidoId)
        {
            var todas = await _unitOfWork.CalificacionRepository.GetAllAsync();
            return todas.Where(x => x.ContenidoId == contenidoId);
        }

        public async Task InsertCalificacion(Calificacion calificacion)
        {
            var perfil = await _unitOfWork.PerfilRepository.GetByIdAsync(calificacion.PerfilId);
            if (perfil == null)
                throw new BussinesException("El perfil no existe", HttpStatusCode.NotFound);

            var existentes = await _unitOfWork.CalificacionRepository.GetAllAsync();
            if (existentes.Any(c => c.PerfilId == calificacion.PerfilId && c.ContenidoId == calificacion.ContenidoId))
                throw new BussinesException("Ya has calificado este contenido anteriormente", HttpStatusCode.Conflict);

            await _unitOfWork.CalificacionRepository.AddAsync(calificacion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<double> GetPromedioDapper(int contenidoId)
        {
            string sql = "SELECT COALESCE(AVG(Puntuacion), 0) FROM calificacion WHERE ContenidoId = @Id";
            return await _dapper.ExecuteScalarAsync<double>(sql, new { Id = contenidoId });
        }

        public async Task<IEnumerable<Calificacion>> GetCalificaciones(CalificacionQueryFilter? filters)
        {
            var calificaciones = await _unitOfWork.CalificacionRepository.GetAllAsync();

            if (filters == null) return calificaciones;

            if (filters.PerfilId.HasValue)
                calificaciones = calificaciones.Where(c => c.PerfilId == filters.PerfilId.Value);

            if (filters.ContenidoId.HasValue)
                calificaciones = calificaciones.Where(c => c.ContenidoId == filters.ContenidoId.Value);

            if (filters.Puntuacion.HasValue)
                calificaciones = calificaciones.Where(c => c.Puntuacion == filters.Puntuacion.Value);

            if (!string.IsNullOrWhiteSpace(filters.Comentario))
                calificaciones = calificaciones.Where(c => c.Comentario != null && c.Comentario.ToLower().Contains(filters.Comentario.ToLower()));

            return calificaciones;
        }
    }
}