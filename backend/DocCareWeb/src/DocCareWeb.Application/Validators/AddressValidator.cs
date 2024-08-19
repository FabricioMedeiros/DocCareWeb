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
                .Length(2, 100).WithMessage("A rua deve ter entre 2 e 50 caracteres.");

            RuleFor(a => a.Number)
                .MaximumLength(20).WithMessage("O número deve ter no máximo 10 caracteres."); 

            RuleFor(a => a.Complement)
                .MaximumLength(20).WithMessage("O complemento deve ter no máximo 20 caracteres.");

            RuleFor(a => a.Neighborhood)
                .NotEmpty().WithMessage("O bairro é obrigatório.")
                .Length(2, 50).WithMessage("O bairro deve ter entre 2 e 50 caracteres."); 

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .Length(2, 50).WithMessage("A cidade deve ter entre 2 e 50 caracteres."); 

            RuleFor(a => a.State)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2, 2).WithMessage("O estado deve ter 2 caracteres.");

            RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{8}$").WithMessage("O CEP deve conter exatamente 8 dígitos numéricos.");
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
