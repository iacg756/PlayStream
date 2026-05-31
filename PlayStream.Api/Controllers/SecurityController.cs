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