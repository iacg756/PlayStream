using System;
using FluentValidation;
using PlayStream.Core.DTOs;

namespace PlayStream.Services.Validators
{
    public class ContenidoDtoValidator : AbstractValidator<ContenidoDto>
    {
        public ContenidoDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");

            RuleFor(x => x.Categoria)
                .NotEmpty().WithMessage("La categoría es obligatoria.")
                .MaximumLength(50).WithMessage("La categoría no puede exceder los 50 caracteres.");

            RuleFor(x => x.AnioLanzamiento)
                .GreaterThan(1900).WithMessage("El año debe ser mayor a 1900.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("El año no puede ser futuro.");
        }
    }
}