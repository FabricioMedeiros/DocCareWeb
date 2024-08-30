using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Application.Validators;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class PatientValidator : AbstractValidator<PatientBaseDto>
    {
        public PatientValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 60).WithMessage("O nome deve ter entre 2 e 60 caracteres.");

            RuleFor(p => p.Cpf)
                .Must(CPFValidator.IsValid).WithMessage("O CPF é inválido.");

            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("O e-mail é inválido.");

            RuleFor(p => p.Phone)
                  .Matches(@"^\d{10}$").WithMessage("O telefone fixo deve conter exatamente 10 dígitos numéricos.");

            RuleFor(p => p.CellPhone)
                .Matches(@"^\d{11}$").WithMessage("O celular deve conter exatamente 11 dígitos numéricos.");

            RuleFor(p => p.Gender)
                .IsInEnum().WithMessage("O gênero é inválido.");

            RuleFor(p => p.BirthDate)
                .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser no passado.");

            RuleFor(p => p.HealthPlanId)
                .NotEmpty().WithMessage("O plano de saúde é obrigatório.");
        }
    }

    public class PatientCreateValidator : AbstractValidator<PatientCreateDto>
    {
        public PatientCreateValidator()
        {
            Include(new PatientValidator());

             RuleFor(p => p.Address)
                .SetValidator(new AddressCreateValidator())
                .WithMessage("O endereço é inválido.");
        }
    }

    public class PatientUpdateValidator : AbstractValidator<PatientUpdateDto>
    {
        public PatientUpdateValidator()
        {
            Include(new PatientValidator());

            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(p => p.Address)
                .SetValidator(new AddressUpdateValidator())
                .WithMessage("O endereço é inválido.");
        }
    }
}
