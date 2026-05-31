using PlayStream.Core.CustomEntities;
using PlayStream.Core.Entities;
using PlayStream.Core.Enum;
using PlayStream.Core.Interfaces;
using PlayStream.Core.QueryFilters;
using PlayStream.Services.Interfaces;
using System.Net;

namespace PlayStream.Services.Services
{
    public class ContenidoService : IContenidoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContenidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetContenidosPaginados(ContenidoQueryFilter filters)
        {
            var contenidos = await _unitOfWork.ContenidoRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filters.Categoria))
                contenidos = contenidos.Where(c => c.Categoria.ToLower().Contains(filters.Categoria.ToLower()));

            if (filters.AnioLanzamiento.HasValue)
                contenidos = contenidos.Where(c => c.AnioLanzamiento == filters.AnioLanzamiento.Value);

            if (!string.IsNullOrWhiteSpace(filters.Titulo))
                contenidos = contenidos.Where(c => c.Titulo.ToLower().Contains(filters.Titulo.ToLower()));

            var pagedList = PagedList<object>.Create(
                contenidos.Cast<object>(),
                filters.PageNumber,
                filters.PageSize);

            if (pagedList.Any())
            {
                return new ResponseData
                {
                    Messages = new[] { new Message { Type = TypeMessage.information.ToString(), Description = "Contenidos recuperados correctamente." } },
                    Pagination = pagedList,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData
                {
                    Messages = new[] { new Message { Type = TypeMessage.warning.ToString(), Description = "No se encontraron contenidos con los filtros indicados." } },
                    Pagination = pagedList,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<Contenido?> GetContenidoById(int id) =>
            await _unitOfWork.ContenidoRepository.GetByIdAsync(id);

        public async Task InsertContenido(Contenido contenido)
        {
            await _unitOfWork.ContenidoRepository.AddAsync(contenido);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateContenido(Contenido contenido)
        {
            await _unitOfWork.ContenidoRepository.UpdateAsync(contenido);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteContenido(int id)
        {
            await _unitOfWork.ContenidoRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}