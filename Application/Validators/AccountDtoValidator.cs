using Application.Dtos.Account;
using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// Validator for AccountDto.
    /// </summary>
    public class AccountDtoValidator : AbstractValidator<AccountDto>
    {
        public AccountDtoValidator()
        {
            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .Length(10, 20).WithMessage("Account number must be between 10 and 20 characters.");
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Account type is invalid.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Account status is invalid.");
            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Balance cannot be negative.");
        }
    }
}

