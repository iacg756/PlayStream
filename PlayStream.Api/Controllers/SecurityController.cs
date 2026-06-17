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
    /// Administración de usuarios del sistema (registro y consulta de sesión)
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
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
        /// Registra un nuevo usuario del sistema. Solo accesible por Administradores.
        /// </summary>
        /// <param name="securityDto">Datos del usuario: login, password, nombre y rol (Administrador/Consumer).</param>
        /// <returns>Usuario registrado.</returns>
        /// <response code="200">Usuario registrado correctamente.</response>
        /// <response code="403">No tiene permisos de Administrador.</response>
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
        /// Retorna los datos del usuario autenticado extraídos del token JWT.
        /// </summary>
        /// <returns>Login, nombre y rol del usuario actual.</returns>
        /// <response code="200">Retorna los datos del usuario autenticado.</response>
        /// <response code="401">Token inválido o expirado.</response>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var login = User.Claims.FirstOrDefault(c => c.Type == "Login")?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { message = "Token válido.", login, name, role });
        }

        /// <summary>
        /// Endpoint de configuración inicial — crea el primer Administrador del sistema sin requerir autenticación.
        /// </summary>
        /// <param name="securityDto">Datos del administrador inicial.</param>
        /// <returns>Administrador creado.</returns>
        /// <response code="200">Administrador creado correctamente.</response>
        [HttpPost("setup")]
        public async Task<IActionResult> Setup([FromBody] SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            security.Role = RoleType.Administrador;
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);
            securityDto = _mapper.Map<SecurityDto>(security);
            return Ok(new ApiResponse<SecurityDto>(securityDto));
        }
    }
}