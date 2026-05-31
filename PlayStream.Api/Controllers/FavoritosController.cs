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
    /// Gestión de contenidos favoritos por perfil de usuario
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;
        private readonly IMapper _mapper;
        private readonly IValidator<FavoritoDto> _validator;

        public FavoritosController(IFavoritoService favoritoService, IMapper mapper, IValidator<FavoritoDto> validator)
        {
            _favoritoService = favoritoService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Lista favoritos con filtros opcionales por perfil o contenido.
        /// </summary>
        /// <param name="filters">Filtros opcionales: PerfilId y/o ContenidoId.</param>
        /// <returns>Lista de favoritos.</returns>
        /// <response code="200">Retorna la lista de favoritos.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FavoritoQueryFilter? filters)
        {
            var favoritos = await _favoritoService.GetFavoritos(filters);
            var favoritosDto = _mapper.Map<IEnumerable<FavoritoDto>>(favoritos);
            return Ok(new ApiResponse<IEnumerable<FavoritoDto>>(favoritosDto));
        }

        /// <summary>
        /// Lista todos los favoritos de un perfil específico.
        /// </summary>
        /// <param name="perfilId">ID del perfil a consultar.</param>
        /// <param name="filters">Filtros adicionales opcionales.</param>
        /// <returns>Lista de favoritos del perfil.</returns>
        /// <response code="200">Retorna los favoritos del perfil.</response>
        /// <response code="401">No autenticado.</response>
        [HttpGet("perfil/{perfilId}")]
        public async Task<IActionResult> GetByPerfil(int perfilId, [FromQuery] FavoritoQueryFilter? filters)
        {
            filters ??= new FavoritoQueryFilter();
            filters.PerfilId = perfilId;

            var favoritos = await _favoritoService.GetFavoritos(filters);
            var favoritosDto = _mapper.Map<IEnumerable<FavoritoDto>>(favoritos);
            return Ok(new ApiResponse<IEnumerable<FavoritoDto>>(favoritosDto));
        }

        /// <summary>
        /// Agrega un contenido a la lista de favoritos de un perfil. No se permiten duplicados.
        /// </summary>
        /// <param name="favoritoDto">Datos del favorito: perfilId y contenidoId.</param>
        /// <returns>Favorito registrado.</returns>
        /// <response code="201">Favorito agregado correctamente.</response>
        /// <response code="400">El contenido ya está en favoritos o datos inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> Post(FavoritoDto favoritoDto)
        {
            var validationResult = await _validator.ValidateAsync(favoritoDto);
            if (!validationResult.IsValid)
                return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });

            try
            {
                var favorito = _mapper.Map<Favorito>(favoritoDto);
                await _favoritoService.AddFavorito(favorito);
                var resultDto = _mapper.Map<FavoritoDto>(favorito);
                return CreatedAtAction(nameof(GetByPerfil), new { perfilId = favorito.PerfilId }, new ApiResponse<FavoritoDto>(resultDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un favorito por su ID.
        /// </summary>
        /// <param name="id">ID del favorito a eliminar.</param>
        /// <returns>Mensaje de confirmación.</returns>
        /// <response code="200">Favorito eliminado correctamente.</response>
        /// <response code="400">Error al eliminar.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _favoritoService.DeleteFavorito(id);
                return Ok(new { message = "Favorito eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar el favorito", error = ex.Message });
            }
        }
    }
}