using DocCareWeb.Application.Dtos.Specialty;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class SpecialtyValidator : AbstractValidator<SpecialtyBaseDto>
    {
        public SpecialtyValidator()
        {
            RuleFor(s => s.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .Length(2, 50).WithMessage("A descrição deve ter entre 2 e 50 caracteres.");
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
