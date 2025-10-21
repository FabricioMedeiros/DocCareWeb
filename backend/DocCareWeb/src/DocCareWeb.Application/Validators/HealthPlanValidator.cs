using DocCareWeb.Application.Dtos.HealthPlan;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class HealthPlanValidator : AbstractValidator<HealthPlanBaseDto>
    {
        public HealthPlanValidator()
        {
            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 60).WithMessage("O nome deve ter entre 2 e 60 caracteres.");
        }
    }

    public class HealthPlanCreateValidator : AbstractValidator<HealthPlanCreateDto>
    {
        public HealthPlanCreateValidator()
        {
            Include(new HealthPlanValidator());
        }
    }

    public class HealthPlanUpdateValidator : AbstractValidator<HealthPlanUpdateDto>
    {
        public HealthPlanUpdateValidator()
        {
            Include(new HealthPlanValidator());

            RuleFor(h => h.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
