using DocCareWeb.Application.Dtos.Appointment;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class AppointmentValidator : AbstractValidator<AppointmentBaseDto>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.DoctorId)
                .NotEmpty().WithMessage("O ID do médico é obrigatório.");

            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("O ID do paciente é obrigatório.");

            RuleFor(a => a.HealthPlanId)
                .NotEmpty().WithMessage("O ID do plano de saúde é obrigatório.");

            RuleFor(a => a.AppointmentDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("A data da consulta deve ser no futuro.");

            RuleFor(a => a.AppointmentTime)
                .NotEmpty().WithMessage("A hora da consulta é obrigatória.");

            RuleFor(a => a.Cost)
                .GreaterThan(0).WithMessage("O custo deve ser maior que zero.");

            RuleFor(a => a.Status)
                .IsInEnum().WithMessage("O status é inválido.");
        }
    }

    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
    {
        public AppointmentCreateValidator()
        {
            Include(new AppointmentValidator());
        }
    }

    public class AppointmentUpdateValidator : AbstractValidator<AppointmentUpdateDto>
    {
        public AppointmentUpdateValidator()
        {
            Include(new AppointmentValidator());

            RuleFor(a => a.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");
        }
    }
}
