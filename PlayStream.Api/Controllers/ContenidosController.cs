using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Api.Responses;
using PlayStream.Core.CustomEntities;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;
using PlayStream.Services.Interfaces;
using System.Net;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Gestión del catálogo de contenido (películas/series)
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContenidosController : ControllerBase
    {
        private readonly IContenidoService _contenidoService;
        private readonly IMapper _mapper;

        public ContenidosController(IContenidoService contenidoService, IMapper mapper)
        {
            _contenidoService = contenidoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene el catálogo de contenido con filtros y paginación.
        /// </summary>
        /// <param name="filters">Filtros: título, categoría, año y paginación.</param>
        /// <returns>Lista paginada de contenidos.</returns>
        /// <response code="200">Retorna la lista paginada de contenidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ContenidoDto>>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ContenidoQueryFilter filters)
        {
            try
            {
                var result = await _contenidoService.GetContenidosPaginados(filters);
                var contenidosDto = _mapper.Map<IEnumerable<ContenidoDto>>(result.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = result.Pagination.TotalCount,
                    PageSize = result.Pagination.PageSize,
                    CurrentPage = result.Pagination.CurrentPage,
                    TotalPages = result.Pagination.TotalPages,
                    HasNextPage = result.Pagination.HasNextPage,
                    HasPreviousPage = result.Pagination.HasPreviousPage
                };

                var response = new ApiResponse<IEnumerable<ContenidoDto>>(contenidosDto)
                {
                    Pagination = pagination,
                    Messages = result.Messages
                };

                return StatusCode((int)result.StatusCode, response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseData
                {
                    Messages = new[] { new Message { Type = "error", Description = ex.Message } }
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Obtiene un contenido por su ID.
        /// </summary>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ContenidoDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contenido = await _contenidoService.GetContenidoById(id);
            if (contenido == null)
                return NotFound(new { message = $"No se encontró contenido con ID {id}." });

            var contenidoDto = _mapper.Map<ContenidoDto>(contenido);
            return Ok(new ApiResponse<ContenidoDto>(contenidoDto));
        }

        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponse<ContenidoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContenidoDto contenidoDto)
        {
            try
            {
                var contenido = _mapper.Map<Contenido>(contenidoDto);
                await _contenidoService.InsertContenido(contenido);
                var resultDto = _mapper.Map<ContenidoDto>(contenido);
                return CreatedAtAction(nameof(Get), new { id = contenido.Id },
                    new ApiResponse<ContenidoDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear contenido.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ContenidoDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContenidoDto contenidoDto)
        {
            var existente = await _contenidoService.GetContenidoById(id);
            if (existente == null)
                return NotFound(new { message = $"No se encontró contenido con ID {id}." });

            try
            {
                existente.Titulo = contenidoDto.Titulo;
                existente.Descripcion = contenidoDto.Descripcion;
                existente.Categoria = contenidoDto.Categoria;
                existente.AnioLanzamiento = contenidoDto.AnioLanzamiento;

                await _contenidoService.UpdateContenido(existente);
                return Ok(new ApiResponse<ContenidoDto>(_mapper.Map<ContenidoDto>(existente)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _contenidoService.GetContenidoById(id);
            if (existente == null)
                return NotFound(new { message = $"No se encontró contenido con ID {id}." });

            await _contenidoService.DeleteContenido(id);
            return NoContent();
        }
    }
}