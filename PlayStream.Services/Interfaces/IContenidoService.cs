using PlayStream.Core.CustomEntities;
using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;

namespace PlayStream.Services.Interfaces
{
    public interface IContenidoService
    {
        Task<ResponseData> GetContenidosPaginados(ContenidoQueryFilter filters);
        Task<Contenido?> GetContenidoById(int id);
        Task InsertContenido(Contenido contenido);
        Task UpdateContenido(Contenido contenido);
        Task DeleteContenido(int id);
    }
}