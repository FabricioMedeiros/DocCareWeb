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
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.");

            RuleFor(d => d.SpecialtyId)
                .NotEmpty().WithMessage("A especialidade é obrigatória.");

            RuleFor(d => d.Crm)
                .MaximumLength(20).WithMessage("O CRM deve ter no máximo 20 caracteres.");

            RuleFor(d => d.Email)
                .EmailAddress().WithMessage("O e-mail é inválido.");

            RuleFor(d => d.CellPhone)
                .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$").WithMessage("O celular é inválido.");

            RuleFor(d => d.Phone)
                .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$").WithMessage("O telefone é inválido.");
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
