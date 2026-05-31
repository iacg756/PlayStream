using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Core.Enum;
using PlayStream.Services.Interfaces;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Panel Administrativo — solo accesible por Administradores
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
        /// Resumen general del sistema para el panel de administración.
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            var contenidos = await _contenidoService.GetContenidoById(0); // solo para contar usamos GetAll

            return Ok(new
            {
                message = "Panel administrativo",
                totalUsuarios = usuarios.Count(),
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Lista todos los usuarios del sistema.
        /// </summary>
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(new { data = usuarios });
        }
    }
}