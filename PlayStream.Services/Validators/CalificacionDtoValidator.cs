using FluentValidation;
using PlayStream.Core.DTOs;

namespace PlayStream.Services.Validators
{
    public class CalificacionDtoValidator : AbstractValidator<CalificacionDto>
    {
        public CalificacionDtoValidator()
        {
            RuleFor(x => x.Puntuacion)
                .InclusiveBetween(1, 5).WithMessage("La puntuación debe estar entre 1 y 5 estrellas.");

            RuleFor(x => x.PerfilId)
                .GreaterThan(0).WithMessage("Debe especificar un Perfil válido.");

            RuleFor(x => x.ContenidoId)
                .GreaterThan(0).WithMessage("Debe especificar el Contenido a calificar.");
        }
    }
}