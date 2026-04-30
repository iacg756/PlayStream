using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PlayStream.Services.Interfaces;
using PlayStream.Core.QueryFilters;
using PlayStream.Core.DTOs;
using PlayStream.Api.Responses;

namespace PlayStream.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalificacionesController : ControllerBase
    {
        private readonly ICalificacionService _calificacionService;
        private readonly IMapper _mapper;

        public CalificacionesController(ICalificacionService calificacionService, IMapper mapper)
        {
            _calificacionService = calificacionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CalificacionQueryFilter? filters)
        {
            var calificaciones = await _calificacionService.GetCalificaciones(filters);
            var calificacionesDto = _mapper.Map<IEnumerable<CalificacionDto>>(calificaciones);
            return Ok(new ApiResponse<IEnumerable<CalificacionDto>>(calificacionesDto));
        }

        [HttpGet("contenido/{contenidoId}")]
        public async Task<IActionResult> GetByContenido(int contenidoId)
        {
            var calificaciones = await _calificacionService.GetCalificacionesByContenido(contenidoId);
            var calificacionesDto = _mapper.Map<IEnumerable<CalificacionDto>>(calificaciones);
            return Ok(new ApiResponse<IEnumerable<CalificacionDto>>(calificacionesDto));
        }

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
