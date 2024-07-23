using DocCareWeb.Application.Dtos.Address;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class AddressValidator : AbstractValidator<AddressBaseDto>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Street)
                .NotEmpty().WithMessage("A rua é obrigatória.")
                .Length(2, 100).WithMessage("A rua deve ter entre 2 e 100 caracteres.");

            RuleFor(a => a.Number)
                .NotEmpty().WithMessage("O número é obrigatório.");

            RuleFor(a => a.Neighborhood)
                .NotEmpty().WithMessage("O bairro é obrigatório.")
                .Length(2, 100).WithMessage("O bairro deve ter entre 2 e 100 caracteres.");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .Length(2, 100).WithMessage("A cidade deve ter entre 2 e 100 caracteres.");

            RuleFor(a => a.State)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2, 2).WithMessage("O estado deve ter 2 caracteres.");

            RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 12345-678.");
        }
    }

    public class AddressCreateValidator : AbstractValidator<AddressCreateDto>
    {
        public AddressCreateValidator()
        {
            Include(new AddressValidator());
        }
    }

    public class AddressUpdateValidator : AbstractValidator<AddressUpdateDto>
    {
        public AddressUpdateValidator()
        {
            Include(new AddressValidator());

            RuleFor(a => a.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
