using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PlayStream.Core.Entities;
using PlayStream.Core.DTOs;
using PlayStream.Services.Interfaces;
using PlayStream.Api.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PlayStream.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenidosController : ControllerBase
    {
        private readonly IContenidoService _contenidoService;
        private readonly IMapper _mapper;

        public ContenidosController(IContenidoService contenidoService, IMapper mapper)
        {
            _contenidoService = contenidoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? categoria, [FromQuery] int? anioLanzamiento)
        {
            var contenidos = await _contenidoService.GetContenidos();

            if (!string.IsNullOrEmpty(categoria))
                contenidos = contenidos.Where(c => c.Categoria.ToLower().Contains(categoria.ToLower()));

            if (anioLanzamiento.HasValue)
                contenidos = contenidos.Where(c => c.AnioLanzamiento == anioLanzamiento.Value);

            var contenidosDto = _mapper.Map<IEnumerable<ContenidoDto>>(contenidos);
            return Ok(new ApiResponse<IEnumerable<ContenidoDto>>(contenidosDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contenido = await _contenidoService.GetContenidoById(id);
            if (contenido == null) return NotFound();

            var contenidoDto = _mapper.Map<ContenidoDto>(contenido);
            return Ok(new ApiResponse<ContenidoDto>(contenidoDto));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContenidoDto contenidoDto)
        {
            try
            {
                var contenido = _mapper.Map<Contenido>(contenidoDto);
                await _contenidoService.InsertContenido(contenido);
                var resultDto = _mapper.Map<ContenidoDto>(contenido);
                return CreatedAtAction(nameof(Get), new { id = contenido.Id }, new ApiResponse<ContenidoDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
    }
}