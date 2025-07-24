using Application.Dtos.Client;
using FluentValidation;

namespace Application.Validators;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del cliente es obligatorio.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("El teléfono del cliente es obligatorio.");
        
        RuleFor(x => x.Identification)
            .NotEmpty().WithMessage("El numero de identificación del cliente es obligatorio.");
    }
}
