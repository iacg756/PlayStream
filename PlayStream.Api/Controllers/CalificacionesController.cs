using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Api.Responses;
using PlayStream.Core.DTOs;
using PlayStream.Core.QueryFilters;
using PlayStream.Services.Interfaces;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Registro y consulta de calificaciones de contenido por perfil
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _calificacionService;
        private readonly IMapper _mapper;

        public CalificacionesController(ICalificacionService calificacionService, IMapper mapper)
        {
            _calificacionService = calificacionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista calificaciones con filtros opcionales por perfil, contenido, puntuación o comentario.
        /// </summary>
        /// <param name="filters">Filtros opcionales de búsqueda.</param>
        /// <returns>Lista de calificaciones.</returns>
        /// <response code="200">Retorna las calificaciones encontradas.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CalificacionQueryFilter? filters)
        {
            var calificaciones = await _calificacionService.GetCalificaciones(filters);
            var calificacionesDto = _mapper.Map<IEnumerable<CalificacionDto>>(calificaciones);
            return Ok(new ApiResponse<IEnumerable<CalificacionDto>>(calificacionesDto));
        }

        /// <summary>
        /// Lista todas las calificaciones de un contenido específico.
        /// </summary>
        /// <param name="contenidoId">ID del contenido a consultar.</param>
        /// <param name="filters">Filtros adicionales opcionales.</param>
        /// <returns>Lista de calificaciones del contenido.</returns>
        /// <response code="200">Retorna las calificaciones del contenido.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet("contenido/{contenidoId}")]
        public async Task<IActionResult> GetByContenido(int contenidoId, [FromQuery] CalificacionQueryFilter? filters)
        {
            filters ??= new CalificacionQueryFilter();
            filters.ContenidoId = contenidoId;

            var calificaciones = await _calificacionService.GetCalificaciones(filters);
            var calificacionesDto = _mapper.Map<IEnumerable<CalificacionDto>>(calificaciones);
            return Ok(new ApiResponse<IEnumerable<CalificacionDto>>(calificacionesDto));
        }

        /// <summary>
        /// Registra una calificación (1-5 estrellas) para un contenido. No se permiten duplicados por perfil.
        /// </summary>
        /// <param name="calificacionDto">Datos de la calificación: perfilId, contenidoId, puntuación y comentario opcional.</param>
        /// <returns>Calificación registrada.</returns>
        /// <response code="201">Calificación registrada correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">El perfil ya calificó este contenido.</response>
        [HttpPost]
        public async Task<IActionResult> Post(CalificacionDto calificacionDto)
        {
            try
            {
                var calificacion = _mapper.Map<PlayStream.Core.Entities.Calificacion>(calificacionDto);
                await _calificacionService.InsertCalificacion(calificacion);
                var resultDto = _mapper.Map<CalificacionDto>(calificacion);
                return CreatedAtAction(nameof(GetByContenido), new { contenidoId = calificacion.ContenidoId }, new ApiResponse<CalificacionDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
    }
}