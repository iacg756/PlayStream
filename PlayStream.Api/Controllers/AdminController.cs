using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Core.Enum;
using PlayStream.Services.Interfaces;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Panel administrativo — estadísticas y gestión general del sistema
    /// </summary>
    [Authorize(Roles = nameof(RoleType.Administrador))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IContenidoService _contenidoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPerfilService _perfilService;

        public AdminController(
            IContenidoService contenidoService,
            IUsuarioService usuarioService,
            IPerfilService perfilService)
        {
            _contenidoService = contenidoService;
            _usuarioService = usuarioService;
            _perfilService = perfilService;
        }

        /// <summary>
        /// Retorna un resumen general del sistema: total de usuarios y timestamp actual.
        /// </summary>
        /// <returns>Resumen del panel administrativo.</returns>
        /// <response code="200">Retorna las estadísticas del sistema.</response>
        /// <response code="403">No tiene permisos de Administrador.</response>
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(new
            {
                message = "Panel administrativo",
                totalUsuarios = usuarios.Count(),
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Lista todos los usuarios registrados en la plataforma. Solo Administradores.
        /// </summary>
        /// <returns>Lista completa de usuarios.</returns>
        /// <response code="200">Retorna todos los usuarios.</response>
        /// <response code="403">No tiene permisos de Administrador.</response>
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(new { data = usuarios });
        }
    }
}