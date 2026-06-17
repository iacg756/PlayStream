using AutoMapper;
using FluentValidation;
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
    /// Gestión de perfiles por cuenta de usuario (máximo 4 por usuario)
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        private readonly IPerfilService _perfilService;
        private readonly IMapper _mapper;
        private readonly IValidator<PerfilDto> _validator;

        public PerfilesController(IPerfilService perfilService, IMapper mapper, IValidator<PerfilDto> validator)
        {
            _perfilService = perfilService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Lista todos los perfiles asociados a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">ID del usuario propietario de los perfiles.</param>
        /// <param name="filters">Filtros opcionales.</param>
        /// <returns>Lista de perfiles del usuario.</returns>
        /// <response code="200">Retorna los perfiles del usuario.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId, [FromQuery] PerfilQueryFilter? filters)
        {
            filters ??= new PerfilQueryFilter();
            filters.UsuarioId = usuarioId;

            var perfiles = await _perfilService.GetPerfiles(filters);
            var perfilesDto = _mapper.Map<IEnumerable<PerfilDto>>(perfiles);
            return Ok(new ApiResponse<IEnumerable<PerfilDto>>(perfilesDto));
        }

        /// <summary>
        /// Crea un nuevo perfil para un usuario. Máximo 4 perfiles por cuenta.
        /// </summary>
        /// <param name="perfilDto">Datos del nuevo perfil (nombre y avatar opcional).</param>
        /// <returns>Perfil creado.</returns>
        /// <response code="200">Perfil creado correctamente.</response>
        /// <response code="400">Datos inválidos o límite de perfiles alcanzado.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PerfilDto perfilDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(perfilDto);
                if (!validationResult.IsValid)
                    return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });

                var perfil = _mapper.Map<Perfil>(perfilDto);
                await _perfilService.InsertPerfil(perfil);
                var resultDto = _mapper.Map<PerfilDto>(perfil);
                return Ok(new ApiResponse<PerfilDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Actualiza el nombre o avatar de un perfil existente.
        /// </summary>
        /// <param name="id">ID del perfil a actualizar.</param>
        /// <param name="perfilDto">Nuevos datos del perfil.</param>
        /// <returns>Perfil actualizado.</returns>
        /// <response code="200">Perfil actualizado correctamente.</response>
        /// <response code="404">Perfil no encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PerfilDto perfilDto)
        {
            var validationResult = await _validator.ValidateAsync(perfilDto);
            if (!validationResult.IsValid)
                return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });

            try
            {
                var perfilExistente = await _perfilService.GetPerfilById(id);
                if (perfilExistente == null)
                    return NotFound(new { message = $"No se encontró el perfil con ID {id} para actualizar." });

                perfilExistente.NombrePerfil = perfilDto.NombrePerfil;
                perfilExistente.AvatarUrl = perfilDto.AvatarUrl;

                await _perfilService.UpdatePerfil(perfilExistente);
                return Ok(new ApiResponse<PerfilDto>(_mapper.Map<PerfilDto>(perfilExistente)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar el perfil", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un perfil por su ID.
        /// </summary>
        /// <param name="id">ID del perfil a eliminar.</param>
        /// <returns>Mensaje de confirmación.</returns>
        /// <response code="200">Perfil eliminado correctamente.</response>
        /// <response code="404">Perfil no encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var perfilExistente = await _perfilService.GetPerfilById(id);
                if (perfilExistente == null)
                    return NotFound(new { message = $"No se encontró el perfil con ID {id} para eliminar." });

                await _perfilService.DeletePerfil(id);
                return Ok(new { message = "Perfil eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar el perfil", error = ex.Message });
            }
        }
    }
}