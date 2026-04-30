using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;

namespace PlayStream.Services.Interfaces
{
    public interface ICalificacionService
    {
        Task<IEnumerable<Calificacion>> GetCalificacionesByContenido(int contenidoId);
        Task InsertCalificacion(Calificacion calificacion);
        Task<double> GetPromedioDapper(int contenidoId);
        Task<IEnumerable<Calificacion>> GetCalificaciones(CalificacionQueryFilter? filters);
    }
}