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
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(new ApiResponse<IEnumerable<UsuarioDto>>(usuariosDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null) return NotFound();

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return Ok(new ApiResponse<UsuarioDto>(usuarioDto));
        }

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