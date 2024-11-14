using AutoMapper;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Enums;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class AppointmentService : GenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>, IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IValidator<AppointmentCreateDto> createValidator,
        IValidator<AppointmentUpdateDto> updateValidator,
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor) : base(appointmentRepository, createValidator, updateValidator, mapper, notificator)
    {
        _appointmentRepository = appointmentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AppointmentListDto?> AddAsync(Appointment appointment)
    {
        var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
            a.DoctorId == appointment.DoctorId && 
            a.AppointmentDate == appointment.AppointmentDate &&
            a.AppointmentTime == appointment.AppointmentTime && 
            a.Status != AppointmentStatus.Canceled);

        if (existingAppointments.Any())
        {
            Notify("Já existe uma consulta agendada para este médico no mesmo dia e horário.");
            return null;
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
        appointment.CreatedBy = userId;
        appointment.CreatedAt = DateTime.Now;

        var createdAppointment = await _appointmentRepository.AddAsync(appointment);
        return _mapper.Map<AppointmentListDto>(createdAppointment);
    }

    public new async Task<bool> UpdateAsync(Appointment appointment)
    {
        var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
            a.DoctorId == appointment.DoctorId && 
            a.AppointmentDate == appointment.AppointmentDate && 
            a.AppointmentTime == appointment.AppointmentTime && 
            a.Id != appointment.Id 
            && a.Status != AppointmentStatus.Canceled);

        if (existingAppointments.Any())
        {
            Notify("Já existe uma consulta agendada para este médico no mesmo dia e horário.");
            return false;
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";

        appointment.LastUpdatedBy = userId;
        appointment.LastUpdatedAt = DateTime.Now;       

        await _appointmentRepository.UpdateAsync(appointment);
        return true;
    }
    public async Task<bool> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus)
    {
        if (!CanChangeStatus(appointment.Status, newStatus))
        {
            Notify("Mudança de status não permitida.");
            return false;
        }

        if (!SetAppointmentStatus(appointment, newStatus))
        {
            return false;
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";

        appointment.LastUpdatedBy = userId;

        await _appointmentRepository.UpdateAsync(appointment);

        return true;
    }

    private bool CanChangeStatus(AppointmentStatus currentStatus, AppointmentStatus newStatus)
    {
        if (currentStatus == AppointmentStatus.Scheduled)
        {
            return newStatus == AppointmentStatus.Confirmed ||
                   newStatus == AppointmentStatus.Canceled ||
                   newStatus == AppointmentStatus.Completed;
        }

        if (currentStatus == AppointmentStatus.Confirmed)
        {
            return newStatus == AppointmentStatus.Completed || newStatus == AppointmentStatus.Canceled;
        }

        if (currentStatus == AppointmentStatus.Completed || currentStatus == AppointmentStatus.Canceled)
        {
            return false;
        }

        return false;
    }

    public bool SetAppointmentStatus(Appointment appointment, AppointmentStatus newStatus)
    {
        switch (newStatus)
        {
            case AppointmentStatus.Scheduled:
                appointment.Schedule();
                break;
            case AppointmentStatus.Confirmed:
                appointment.Confirm();
                break;
            case AppointmentStatus.Canceled:
                appointment.Cancel();
                break;
            case AppointmentStatus.Completed:
                appointment.Complete();
                break;
            default:
                Notify("Status inválido.");
                return false;
        }

        return true;
    }
}