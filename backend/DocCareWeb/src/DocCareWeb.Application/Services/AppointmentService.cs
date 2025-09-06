using AutoMapper;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Enums;
using DocCareWeb.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class AppointmentService : GenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>, IAppointmentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(
        IUnitOfWork uow,
        IMapper mapper,
        INotificator notificator,
        IHttpContextAccessor httpContextAccessor)
        : base(uow, uow.Appointments, mapper, notificator)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AppointmentListDto?> AddAsync(Appointment appointment)
    {
        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
            a.DoctorId == appointment.DoctorId &&
            a.AppointmentDate == appointment.AppointmentDate &&
            a.Status != AppointmentStatus.Canceled);

        if (HasTimeConflict(appointment, existingAppointments.Items))
        {
            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
            return null;
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
        appointment.CreatedBy = userId;
        appointment.CreatedAt = DateTime.Now;

        await _uow.Appointments.AddAsync(appointment);
        await _uow.CommitAsync();

        return _mapper.Map<AppointmentListDto>(appointment);
    }

    public new async Task<bool> UpdateAsync(Appointment appointment)
    {
        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
            a.DoctorId == appointment.DoctorId &&
            a.AppointmentDate == appointment.AppointmentDate &&
            a.Id != appointment.Id &&
            a.Status != AppointmentStatus.Canceled);

        if (HasTimeConflict(appointment, existingAppointments.Items))
        {
            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
            return false;
        }


        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
        appointment.LastUpdatedBy = userId;
        appointment.LastUpdatedAt = DateTime.Now;

        _uow.Appointments.Update(appointment);
        await _uow.CommitAsync();

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
        appointment.LastUpdatedAt = DateTime.Now;

        _uow.Appointments.Update(appointment);
        await _uow.CommitAsync();

        return true;
    }

    private bool CanChangeStatus(AppointmentStatus currentStatus, AppointmentStatus newStatus)
    {
        return currentStatus switch
        {
            AppointmentStatus.Scheduled => newStatus is AppointmentStatus.Confirmed or AppointmentStatus.Canceled or AppointmentStatus.Completed,
            AppointmentStatus.Confirmed => newStatus is AppointmentStatus.Completed or AppointmentStatus.Canceled,
            AppointmentStatus.Completed or AppointmentStatus.Canceled => false,
            _ => false
        };
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

    private bool HasTimeConflict(Appointment appointment, IEnumerable<Appointment?> existingAppointments)
    {
        var newStart = appointment.StartTime;
        var newEnd = appointment.EndTime;

        foreach (var existing in existingAppointments)
        {
            if (existing == null) continue;

            var existingStart = existing.StartTime;
            var existingEnd = existing.EndTime;

            bool overlaps = newStart < existingEnd && newEnd > existingStart;
            if (overlaps)
                return true;
        }

        return false;
    }
}