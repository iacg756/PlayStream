using FluentValidation;
using PlayStream.Core.DTOs;

namespace PlayStream.Services.Validators
{
    public class PerfilDtoValidator : AbstractValidator<PerfilDto>
    {
        public PerfilDtoValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("El UsuarioId es obligatorio.");

            RuleFor(x => x.NombrePerfil)
                .NotEmpty().WithMessage("El nombre del perfil es obligatorio.")
                .MaximumLength(50).WithMessage("No puede exceder los 50 caracteres.");
        }
    }
}