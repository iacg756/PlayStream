using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using PlayStream.Services.Interfaces;
using PlayStream.Core.QueryFilters;
using PlayStream.Core.DTOs;
using PlayStream.Api.Responses;
using PlayStream.Core.Entities;

namespace PlayStream.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FavoritoQueryFilter? filters)
        {
            var favoritos = await _favoritoService.GetFavoritos(filters);
            var favoritosDto = _mapper.Map<IEnumerable<FavoritoDto>>(favoritos);
            return Ok(new ApiResponse<IEnumerable<FavoritoDto>>(favoritosDto));
        }

        [HttpGet("perfil/{perfilId}")]
        public async Task<IActionResult> GetByPerfil(int perfilId)
        {
            var favoritos = await _favoritoService.GetFavoritosByPerfil(perfilId);
            var favoritosDto = _mapper.Map<IEnumerable<FavoritoDto>>(favoritos);
            return Ok(new ApiResponse<IEnumerable<FavoritoDto>>(favoritosDto));
        }

        [HttpPost]
        public async Task<IActionResult> Post(FavoritoDto favoritoDto)
        {
            var validationResult = await _validator.ValidateAsync(favoritoDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { message = "Errores de validación", errors = validationResult.Errors });
            }

            try
            {
                var favorito = _mapper.Map<Favorito>(favoritoDto);
                await _favoritoService.AddFavorito(favorito);
                var resultDto = _mapper.Map<FavoritoDto>(favorito);
                return CreatedAtAction(nameof(GetByPerfil), new { perfilId = favorito.PerfilId }, new ApiResponse<FavoritoDto>(resultDto));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _favoritoService.DeleteFavorito(id);
                return Ok(new { message = "Favorito eliminado correctamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar el favorito", error = ex.Message });
            }
        }
    }
}
