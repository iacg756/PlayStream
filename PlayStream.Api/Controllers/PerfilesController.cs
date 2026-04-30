using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using PlayStream.Core.Entities;
using PlayStream.Core.DTOs;
using PlayStream.Services.Interfaces;
using PlayStream.Api.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayStream.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var perfiles = await _perfilService.GetPerfilesByUsuario(usuarioId);
            var perfilesDto = _mapper.Map<IEnumerable<PerfilDto>>(perfiles);
            return Ok(new ApiResponse<IEnumerable<PerfilDto>>(perfilesDto));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PerfilDto perfilDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(perfilDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });
                }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PerfilDto perfilDto)
        {
            var validationResult = await _validator.ValidateAsync(perfilDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });
            }

            try
            {
                var perfilExistente = await _perfilService.GetPerfilById(id);
                if (perfilExistente == null)
                {
                    return NotFound(new { message = $"No se encontró el perfil con ID {id} para actualizar." });
                }

                perfilExistente.NombrePerfil = perfilDto.NombrePerfil;
                perfilExistente.AvatarUrl = perfilDto.AvatarUrl;

                await _perfilService.UpdatePerfil(perfilExistente);
                
                var resultDto = _mapper.Map<PerfilDto>(perfilExistente);
                return Ok(new ApiResponse<PerfilDto>(resultDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar el perfil", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var perfilExistente = await _perfilService.GetPerfilById(id);
                if (perfilExistente == null)
                {
                    return NotFound(new { message = $"No se encontró el perfil con ID {id} para eliminar." });
                }

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