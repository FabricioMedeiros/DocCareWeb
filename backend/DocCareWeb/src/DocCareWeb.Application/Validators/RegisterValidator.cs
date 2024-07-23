using DocCareWeb.Application.Dtos.User;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class RegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail é inválido.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
