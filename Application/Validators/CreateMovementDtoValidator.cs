using Application.Dtos.Movement;
using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// Validator for CreateMovementDto.
    /// Ensures AccountId is present, Type is valid, and Value is non-zero.
    /// </summary>
    public class CreateMovementDtoValidator : AbstractValidator<CreateMovementDto>
    {
        public CreateMovementDtoValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account ID is required.");
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Movement type is invalid.");
            RuleFor(x => x.Value)
                .NotEqual(0).WithMessage("Movement value cannot be zero.");
        }
    }
}
