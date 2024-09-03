using DocCareWeb.Application.Dtos.HealthPlan;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class HealthPlanValidator : AbstractValidator<HealthPlanBaseDto>
    {
        public HealthPlanValidator()
        {
            RuleFor(h => h.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .Length(2, 60).WithMessage("A descrição deve ter entre 2 e 60 caracteres.");

            RuleFor(a => a.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("O valor da consulta deve ser maior ou igual a zero.");
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
