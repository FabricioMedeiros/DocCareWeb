using DocCareWeb.Application.Dtos.AppointmentItem;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class AppointmentItemValidator : AbstractValidator<AppointmentItemBaseDto>
    {
        public AppointmentItemValidator()
        {
            RuleFor(i => i.ServiceId)
              .NotEmpty().WithMessage("O ID do serviço é obrigatório.");

            RuleFor(i => i.Price)
               .GreaterThanOrEqualTo(0).WithMessage("O preço do serviço deve ser maior ou igual a zero.");

            RuleFor(i => i.SuggestedPrice)
                .GreaterThanOrEqualTo(0).WithMessage("O preço sugerido deve ser maior ou igual a zero.");
        }
    }

    public class AppointmentItemCreateValidator : AbstractValidator<AppointmentItemCreateDto>
    {
        public AppointmentItemCreateValidator()
        {
            Include(new AppointmentItemValidator());
        }
    }

    public class AppointmentItemUpdateValidator : AbstractValidator<AppointmentItemUpdateDto>
    {
        public AppointmentItemUpdateValidator()
        {
            Include(new AppointmentItemValidator());
        }
    }
}