using DocCareWeb.Application.Dtos.Service;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class ServiceValidator : AbstractValidator<ServiceBaseDto>
    {
        public ServiceValidator() 
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("O nome obrigatório.")
                .Length(2, 60).WithMessage("A descrição deve ter entre 2 e 60 caracteres.");

        }

        public class ServiceCreateValidator : AbstractValidator<ServiceCreateDto>
        {
            public ServiceCreateValidator()
            {
                Include(new ServiceValidator());

                RuleFor(s => s.BasePrice)
               .GreaterThanOrEqualTo(0).WithMessage("O valor do serviço deve ser maior ou igual a zero.");
            }
        }

        public class ServiceUpdateValidator : AbstractValidator<ServiceUpdateDto>
        {
            public ServiceUpdateValidator()
            {
                Include(new ServiceValidator());

                RuleFor(s => s.Id)
                    .NotEmpty().WithMessage("O ID é obrigatório.");

                RuleFor(s => s.BasePrice)
                    .GreaterThanOrEqualTo(0).WithMessage("O valor do serviço deve ser maior ou igual a zero.");
            }
        }
    }
}
