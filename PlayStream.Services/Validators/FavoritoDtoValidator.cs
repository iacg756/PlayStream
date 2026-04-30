using FluentValidation;
using PlayStream.Core.DTOs;

namespace PlayStream.Services.Validators
{
    public class FavoritoDtoValidator : AbstractValidator<FavoritoDto>
    {
        public FavoritoDtoValidator()
        {
            RuleFor(x => x.PerfilId)
                .NotEmpty().WithMessage("El ID de perfil es obligatorio.");

            RuleFor(x => x.ContenidoId)
                .NotEmpty().WithMessage("El ID de contenido es obligatorio.");
        }
    }
}