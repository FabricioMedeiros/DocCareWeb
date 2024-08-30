﻿using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Domain.Enums;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;

namespace DocCareWeb.Application.Validators
{
    public class AppointmentValidator : AbstractValidator<AppointmentBaseDto>
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IHealthPlanService _healthPlanService;

        public AppointmentValidator(
            IDoctorService doctorService,
            IPatientService patientService,
            IHealthPlanService healthPlanService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _healthPlanService = healthPlanService;

            RuleFor(a => a.DoctorId)
                .NotEmpty().WithMessage("O ID do médico é obrigatório.")
                .MustAsync(async (id, cancellation) => (await _doctorService.GetByIdAsync(id)) != null)
                .WithMessage("O médico especificado não existe.");

            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("O ID do paciente é obrigatório.")
                .MustAsync(async (id, cancellation) => (await _patientService.GetByIdAsync(id)) != null)
                .WithMessage("O paciente especificado não existe.");

            RuleFor(a => a.HealthPlanId)
                .NotEmpty().WithMessage("O ID do plano de saúde é obrigatório.")
                .MustAsync(async (id, cancellation) => (await _healthPlanService.GetByIdAsync(id)) != null)
                .WithMessage("O plano de saúde especificado não existe.");

            RuleFor(a => a.AppointmentTime)
                .NotEmpty().WithMessage("A hora da consulta é obrigatória.");

            RuleFor(a => a.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("O valor da consulta deve ser maior ou igual a zero.");
        }
    }

    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
    {
        public AppointmentCreateValidator(
            IDoctorService doctorService,
            IPatientService patientService,
            IHealthPlanService healthPlanService)
        {
            Include(new AppointmentValidator(doctorService, patientService, healthPlanService));

            RuleFor(a => a.AppointmentDate)
                .Must(date => DateTime.TryParse(date, out var parsedDate) && parsedDate >= DateTime.Today)
                .WithMessage("A data da consulta deve ser igual ou superior à data atual.");
        }
    }

    public class AppointmentUpdateValidator : AbstractValidator<AppointmentUpdateDto>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IHealthPlanService _healthPlanService;

        public AppointmentUpdateValidator(
            IAppointmentRepository appointmentRepository,
            IDoctorService doctorService,
            IPatientService patientService,
            IHealthPlanService healthPlanService)
        {
            _appointmentRepository = appointmentRepository;
            _doctorService = doctorService;
            _patientService = patientService;
            _healthPlanService = healthPlanService;

            Include(new AppointmentValidator(doctorService, patientService, healthPlanService));

            RuleFor(a => a.Id)
                .NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(a => a.AppointmentDate)
                .MustAsync(async (dto, date, cancellation) => await ValidateUpdateDateAsync(dto.Id, date, cancellation))
                .WithMessage("A data da consulta não atende aos critérios definidos.");

            RuleFor(a => a)
                .MustAsync(async (dto, cancellation) => await CanUpdateAsync(dto.Id, cancellation))
                .WithMessage("Não é permitido alterar o agendamento com o status atual.");
        }

        private async Task<bool> ValidateUpdateDateAsync(int appointmentId, string date, CancellationToken cancellation)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            return appointment?.Status == AppointmentStatus.Scheduled || parsedDate >= DateTime.Today;
        }

        private async Task<bool> CanUpdateAsync(int appointmentId, CancellationToken cancellation)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            return appointment?.Status != AppointmentStatus.Confirmed &&
                   appointment?.Status != AppointmentStatus.Completed &&
                   appointment?.Status != AppointmentStatus.Canceled;
        }
    }
}