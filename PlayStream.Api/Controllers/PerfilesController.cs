using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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

        public PerfilesController(IPerfilService perfilService, IMapper mapper)
        {
            _perfilService = perfilService;
            _mapper = mapper;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var perfiles = await _perfilService.GetPerfilesByUsuario(usuarioId);
            var perfilesDto = _mapper.Map<IEnumerable<PerfilDto>>(perfiles);
            return Ok(new ApiResponse<IEnumerable<PerfilDto>>(perfilesDto));
        }

        [HttpPost]
        public async Task<IActionResult> Post(PerfilDto perfilDto)
        {
            try
            {
                var perfil = _mapper.Map<Perfil>(perfilDto);
                await _perfilService.InsertPerfil(perfil);
                var resultDto = _mapper.Map<PerfilDto>(perfil);
                return Ok(new ApiResponse<PerfilDto>(resultDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error de validación", error = ex.Message });
            }
        }
    }
}