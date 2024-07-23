using DocCareWeb.Application.Dtos.Specialty;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class SpecialtyValidator : AbstractValidator<SpecialtyBaseDto>
    {
        public SpecialtyValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.");
        }
    }

    public class SpecialtyCreateValidator : AbstractValidator<SpecialtyCreateDto>
    {
        public SpecialtyCreateValidator()
        {
            Include(new SpecialtyValidator());
        }
    }

    public class SpecialtyUpdateValidator : AbstractValidator<SpecialtyUpdateDto>
    {
        public SpecialtyUpdateValidator()
        {
            Include(new SpecialtyValidator());

            RuleFor(s => s.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
