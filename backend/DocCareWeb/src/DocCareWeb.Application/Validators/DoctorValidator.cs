using DocCareWeb.Application.Dtos.Doctor;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class DoctorValidator : AbstractValidator<DoctorBaseDto>
    {
        public DoctorValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 60).WithMessage("O nome deve ter entre 2 e 60 caracteres.");

            RuleFor(d => d.SpecialtyId)
                .NotEmpty().WithMessage("A especialidade é obrigatória.");

            RuleFor(d => d.Crm)
                .MaximumLength(20).WithMessage("O CRM deve ter no máximo 20 caracteres.");

            RuleFor(d => d.Email)
                .EmailAddress().WithMessage("O e-mail é inválido.");

            RuleFor(p => p.Phone)
                   .Matches(@"^\d{10}$").WithMessage("O telefone fixo deve conter exatamente 10 dígitos numéricos.");

            RuleFor(p => p.CellPhone)
                .Matches(@"^\d{11}$").WithMessage("O celular deve conter exatamente 11 dígitos numéricos.");
        }
    }

    public class DoctorCreateValidator : AbstractValidator<DoctorCreateDto>
    {
        public DoctorCreateValidator()
        {
            Include(new DoctorValidator());
        }
    }

    public class DoctorUpdateValidator : AbstractValidator<DoctorUpdateDto>
    {
        public DoctorUpdateValidator()
        {
            Include(new DoctorValidator());

            RuleFor(d => d.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
