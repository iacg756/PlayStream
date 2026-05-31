using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayStream.Api.Responses;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;
using PlayStream.Core.Enum;
using PlayStream.Services.Interfaces;

namespace PlayStream.Api.Controllers
{
    /// <summary>
    /// Gestión de usuarios del sistema (registro y administración)
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(
            ISecurityService securityService,
            IMapper mapper,
            IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Registra un nuevo usuario. Solo Administradores pueden crear usuarios.
        /// </summary>
        /// <param name="securityDto">Datos del nuevo usuario.</param>
        /// <returns>Usuario registrado.</returns>
        /// <response code="200">Usuario registrado correctamente.</response>
        /// <response code="403">No tiene permisos para esta acción.</response>
        [Authorize(Roles = nameof(RoleType.Administrador))]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            return Ok(new ApiResponse<SecurityDto>(securityDto));
        }

        /// <summary>
        /// Endpoint de prueba de conexión (solo usuarios autenticados).
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var login = User.Claims.FirstOrDefault(c => c.Type == "Login")?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Token válido.",
                login,
                name,
                role
            });
        }
    }
}