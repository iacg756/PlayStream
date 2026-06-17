using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Api.Responses;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;
using PlayStream.Core.QueryFilters;
using PlayStream.Services.Interfaces;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Gestión de cuentas de usuario de la plataforma
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los usuarios registrados en la plataforma.
        /// </summary>
        /// <param name="filters">Filtros opcionales: nombre o correo.</param>
        /// <returns>Lista de usuarios.</returns>
        /// <response code="200">Retorna la lista de usuarios.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UsuarioQueryFilter? filters)
        {
            var usuarios = await _usuarioService.GetUsuarios(filters);
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(new ApiResponse<IEnumerable<UsuarioDto>>(usuariosDto));
        }

        /// <summary>
        /// Obtiene un usuario específico por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Datos del usuario.</returns>
        /// <response code="200">Retorna el usuario encontrado.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null) return NotFound();

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return Ok(new ApiResponse<UsuarioDto>(usuarioDto));
        }

        /// <summary>
        /// Registra un nuevo usuario en la plataforma.
        /// </summary>
        /// <param name="usuarioDto">Datos del nuevo usuario (nombre y correo).</param>
        /// <returns>Usuario creado.</returns>
        /// <response code="201">Usuario creado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDto usuarioDto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioDto);
                await _usuarioService.InsertUsuario(usuario);
                var resultDto = _mapper.Map<UsuarioDto>(usuario);
                return CreatedAtAction(nameof(Get), new { id = usuario.Id }, new ApiResponse<UsuarioDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }
    }
}