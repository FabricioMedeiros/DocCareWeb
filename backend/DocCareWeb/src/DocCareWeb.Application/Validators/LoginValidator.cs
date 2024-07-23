using DocCareWeb.Application.Dtos.User;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail é inválido.");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}
