using DocCareWeb.Application.Dtos.Appointment;
using FluentValidation;
using System.Globalization;

namespace DocCareWeb.Application.Validators
{
    public class AppointmentValidator : AbstractValidator<AppointmentBaseDto>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.AppointmentDate)
                .NotEmpty().WithMessage("A data da consulta é obrigatória.");

            RuleFor(a => a.StartTime)
                .NotEmpty().WithMessage("A hora inicial da consulta é obrigatória.")
                .Must(IsValidTime).WithMessage("A hora inicial da consulta está em formato inválido. Use hh:mm.");

            RuleFor(a => a.EndTime)
                .NotEmpty().WithMessage("A hora final da consulta é obrigatória.")
                .Must(IsValidTime).WithMessage("A hora final da consulta está em formato inválido. Use hh:mm.");

            RuleFor(a => a)
                .Must(a => IsEndTimeAfterStartTime(a.StartTime, a.EndTime))
                .WithMessage("A hora final da consulta deve ser posterior à hora inicial.");
        }

        private bool IsValidTime(string time)
        {
            return TimeSpan.TryParseExact(time, @"hh\:mm", CultureInfo.InvariantCulture, out _);
        }

        private bool IsEndTimeAfterStartTime(string start, string end)
        {
            if (TimeSpan.TryParseExact(start, @"hh\:mm", CultureInfo.InvariantCulture, out var startTime) &&
                TimeSpan.TryParseExact(end, @"hh\:mm", CultureInfo.InvariantCulture, out var endTime))
            {
                return endTime > startTime;
            }
            return false;
        }
    }

    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
    {    
        public AppointmentCreateValidator()
        {
            Include(new AppointmentValidator());           

            RuleFor(a => a.AppointmentDate)
                .Must(date => DateTime.TryParse(date, out var parsedDate) && parsedDate >= DateTime.Today)
                .WithMessage("A data da consulta deve ser igual ou superior à data atual.");

            RuleFor(a => a.DoctorId)
                .NotEmpty().WithMessage("O ID do médico é obrigatório.");

            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("O ID do paciente é obrigatório.");

            RuleFor(a => a.HealthPlanId)
                .NotEmpty().WithMessage("O ID do plano de saúde é obrigatório.");

            RuleFor(a => a.Items)
              .NotEmpty().WithMessage("A consulta deve ter ao menos um serviço vinculado.");

            RuleForEach(a => a.Items)
               .SetValidator(new AppointmentItemCreateValidator());
        }
    }

    public class AppointmentUpdateValidator : AbstractValidator<AppointmentUpdateDto>
    {      
        public AppointmentUpdateValidator()
        {          
            Include(new AppointmentValidator());

            RuleFor(a => a.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(a => a.Items)
                .NotEmpty().WithMessage("A consulta deve ter ao menos um serviço vinculado.");

            RuleForEach(a => a.Items)
               .SetValidator(new AppointmentItemUpdateValidator());
        }
    }
}