using AutoMapper;
using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Application.Services;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Enums;
using DocCareWeb.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AppointmentService
    : GenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>, IAppointmentService
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

    public override async Task<AppointmentListDto?> AddAsync(
    Appointment appointment,
    Func<IQueryable<Appointment>, IQueryable<Appointment>>? includes = null)
    {
        var isValid = await ValidateIdsAsync(
            appointment.Items,
            s => s.ServiceId,
            async ids =>
            {
                var result = await _uow.Services.GetAllAsync(s => ids.Contains(s.Id));
                return result.Items.Select(s => s.Id).ToHashSet();
            },
            "ServiceId"
        );

        if (!isValid)
          return null;

        if (!await ValidateAppointmentDependenciesAsync(appointment))
          return null;

        if (!ValidateAppointmentDateChange(appointment, appointment.AppointmentDate))
          return null;

        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
            a.DoctorId == appointment.DoctorId &&
            a.AppointmentDate == appointment.AppointmentDate &&
            a.Status != AppointmentStatus.Canceled);

        if (HasTimeConflict(appointment, existingAppointments.Items))
        {
            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
            return null;
        }

        appointment.CreatedBy = GetCurrentUserId();
        appointment.CreatedAt = DateTime.Now;
        appointment.CalculateTotal();

        await _uow.Appointments.AddAsync(appointment);
        await _uow.CommitAsync();

        Appointment? fullAppointment;

        fullAppointment = await _uow.Appointments.GetByIdAsync(appointment.Id, includes); 
        
        return _mapper.Map<AppointmentListDto>(fullAppointment);
    }
    public override async Task UpdateAsync(AppointmentUpdateDto dto)
    {
        var isValid = await ValidateIdsAsync(
            dto.Items,
            s => s.ServiceId,
            async ids =>
            {
                var result = await _uow.Services.GetAllAsync(s => ids.Contains(s.Id));
                return result.Items.Select(s => s.Id).ToHashSet();
            },
            "ServiceId"
        );

        if (!isValid)
            return;

        var appointment = await _uow.Appointments.GetByIdAsync(dto.Id, query =>
            query.Include(a => a.Items));

        if (appointment is null)
        {
            Notify("Agendamento não encontrado.");
            return;
        }

        _mapper.Map(dto, appointment);

        if (!ValidateAppointmentDateChange(appointment, DateTime.Parse(dto.AppointmentDate)))
            return;

        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
            a.DoctorId == dto.DoctorId &&
            a.AppointmentDate.Date == DateTime.Parse(dto.AppointmentDate) &&
            a.Id != dto.Id &&
            a.Status != AppointmentStatus.Canceled);

        if (HasTimeConflict(appointment, existingAppointments.Items))
        {
            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
            return;
        }

        if (dto.Status != appointment.Status)
        {
            if (!CanChangeStatus(appointment.Status, dto.Status) ||
                !SetAppointmentStatus(appointment, dto.Status))
            {
                Notify("Mudança de status não permitida.");
                return;
            }
        }

        appointment.LastUpdatedBy = GetCurrentUserId();
        appointment.LastUpdatedAt = DateTime.Now;

        appointment.Items.Clear();

        foreach (var serviceDto in dto.Items)
        {
            appointment.Items.Add(new AppointmentItem
            {
                ServiceId = serviceDto.ServiceId,
                Price = serviceDto.Price
            });
        }

        appointment.CalculateTotal();

        _uow.Appointments.Update(appointment);
        await _uow.CommitAsync();
    }

    public async Task<bool> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus)
    {
        if (!CanChangeStatus(appointment.Status, newStatus))
        {
            Notify("Mudança de status não permitida.");
            return false;
        }

        if (!SetAppointmentStatus(appointment, newStatus))
            return false;

        var userId = GetCurrentUserId();
        appointment.LastUpdatedBy = userId;
        appointment.LastUpdatedAt = DateTime.Now;

        _uow.Appointments.Update(appointment);
        await _uow.CommitAsync();

        return true;
    }

    #region Helpers

    private async Task<bool> ValidateAppointmentDependenciesAsync(Appointment appointment)
    {
        var doctor = await _uow.Doctors.GetByIdAsync(appointment.DoctorId);
        if (doctor == null)
        {
            Notify("O médico especificado não existe.");
            return false;
        }

        var patient = await _uow.Patients.GetByIdAsync(appointment.PatientId);
        if (patient == null)
        {
            Notify("O paciente especificado não existe.");
            return false;
        }

        var healthPlan = await _uow.HealthPlans.GetByIdAsync(appointment.HealthPlanId);
        if (healthPlan == null)
        {
            Notify("O plano de saúde especificado não existe.");
            return false;
        }

        return true;
    }

    private bool ValidateAppointmentDateChange(Appointment appointment, DateTime newDate)
    {  
        if (appointment.AppointmentDate.Date != newDate.Date && newDate < DateTime.Today)
        {
            Notify("A data da consulta não pode ser retroativa.");
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

            if (newStart < existingEnd && newEnd > existingStart)
                return true;
        }

        return false;
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

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
    }

    #endregion
}


//using AutoMapper;
//using DocCareWeb.Application.Dtos.Appointment;
//using DocCareWeb.Application.Interfaces;
//using DocCareWeb.Application.Notifications;
//using DocCareWeb.Application.Services;
//using DocCareWeb.Domain.Entities;
//using DocCareWeb.Domain.Enums;
//using DocCareWeb.Domain.Interfaces;
//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;

//public class AppointmentService : GenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>, IAppointmentService
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public AppointmentService(
//        IUnitOfWork uow,
//        IMapper mapper,
//        INotificator notificator,
//        IHttpContextAccessor httpContextAccessor)
//        : base(uow, uow.Appointments, mapper, notificator)
//    {
//        _httpContextAccessor = httpContextAccessor;
//    }

//    public override async Task<AppointmentListDto?> AddAsync(Appointment appointment)
//    {
//        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
//            a.DoctorId == appointment.DoctorId &&
//            a.AppointmentDate == appointment.AppointmentDate &&
//            a.Status != AppointmentStatus.Canceled);

//        if (HasTimeConflict(appointment, existingAppointments.Items))
//        {
//            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
//            return null;
//        }

//        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
//        appointment.CreatedBy = userId;
//        appointment.CreatedAt = DateTime.Now;

//        await _uow.Appointments.AddAsync(appointment);
//        await _uow.CommitAsync();

//        return _mapper.Map<AppointmentListDto>(appointment);
//    }

//    public override async Task UpdateAsync(AppointmentUpdateDto dto)
//    {
//        var appointment = await _uow.Appointments.GetByIdAsync(dto.Id);

//        if (appointment is null)
//        {
//            Notify("Agendamento não encontrado.");
//            return;
//        }

//        _mapper.Map(dto, appointment);

//        if (!await ValidateAppointmentDependenciesAsync(appointment))
//            return;

//        if (!ValidateAppointmentDateChange(appointment, dto.AppointmentDate))
//            return;

//        var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
//            a.DoctorId == dto.DoctorId &&
//            a.AppointmentDate.Date == DateOnly.Parse(dto.AppointmentDate).ToDateTime(TimeOnly.MinValue) &&
//            a.Id != dto.Id &&
//            a.Status != AppointmentStatus.Canceled);        

//        if (HasTimeConflict(appointment, existingAppointments.Items))
//        {
//            Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
//            return;
//        }

//        if (dto.Status != appointment.Status)
//        {
//            if (!CanChangeStatus(appointment.Status, dto.Status) ||
//                !SetAppointmentStatus(appointment, dto.Status))
//            {
//                Notify("Mudança de status não permitida.");
//                return;
//            }
//        }

//        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
//        appointment.LastUpdatedBy = userId;
//        appointment.LastUpdatedAt = DateTime.Now;

//        _uow.Appointments.Update(appointment);
//        await _uow.CommitAsync();
//    }

//    private async Task<bool> ValidateAppointmentDependenciesAsync(Appointment appointment)
//    {
//        var doctor = await _uow.Doctors.GetByIdAsync(appointment.DoctorId);
//        if (doctor == null)
//        {
//            Notify("O médico especificado não existe.");
//            return false;
//        }

//        var patient = await _uow.Patients.GetByIdAsync(appointment.PatientId);
//        if (patient == null)
//        {
//            Notify("O paciente especificado não existe.");
//            return false;
//        }

//        var healthPlan = await _uow.HealthPlans.GetByIdAsync(appointment.HealthPlanId);
//        if (healthPlan == null)
//        {
//            Notify("O plano de saúde especificado não existe.");
//            return false;
//        }

//        return true;
//    }

//    private bool ValidateAppointmentDateChange(Appointment appointment, string? newDate)
//    {
//        if (string.IsNullOrWhiteSpace(newDate))
//            return true; // nada para validar se não houve alteração

//        if (!DateTime.TryParse(newDate, out var parsedDate))
//        {
//            Notify("A data informada é inválida.");
//            return false;
//        }

//        // só precisa validar se está tentando mudar a data
//        if (appointment.AppointmentDate.Date != parsedDate.Date && parsedDate < DateTime.Today)
//        {
//            Notify("A data da consulta não pode ser retroativa.");
//            return false;
//        }

//        return true;
//    }



//    //public new async Task<bool> UpdateAsync(Appointment appointment)
//    //{
//    //    var existingAppointments = await _uow.Appointments.GetAllAsync(a =>
//    //        a.DoctorId == appointment.DoctorId &&
//    //        a.AppointmentDate == appointment.AppointmentDate &&
//    //        a.Id != appointment.Id &&
//    //        a.Status != AppointmentStatus.Canceled);

//    //    if (HasTimeConflict(appointment, existingAppointments.Items))
//    //    {
//    //        Notify("Já existe uma consulta agendada para esse médico que conflita com o horário informado.");
//    //        return false;
//    //    }


//    //    var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
//    //    appointment.LastUpdatedBy = userId;
//    //    appointment.LastUpdatedAt = DateTime.Now;

//    //    _uow.Appointments.Update(appointment);
//    //    await _uow.CommitAsync();

//    //    return true;
//    //}

//    public async Task<bool> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus)
//    {
//        if (!CanChangeStatus(appointment.Status, newStatus))
//        {
//            Notify("Mudança de status não permitida.");
//            return false;
//        }

//        if (!SetAppointmentStatus(appointment, newStatus))
//        {
//            return false;
//        }

//        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "Anonymous";
//        appointment.LastUpdatedBy = userId;
//        appointment.LastUpdatedAt = DateTime.Now;

//        _uow.Appointments.Update(appointment);
//        await _uow.CommitAsync();

//        return true;
//    }

//    private bool CanChangeStatus(AppointmentStatus currentStatus, AppointmentStatus newStatus)
//    {
//        return currentStatus switch
//        {
//            AppointmentStatus.Scheduled => newStatus is AppointmentStatus.Confirmed or AppointmentStatus.Canceled or AppointmentStatus.Completed,
//            AppointmentStatus.Confirmed => newStatus is AppointmentStatus.Completed or AppointmentStatus.Canceled,
//            AppointmentStatus.Completed or AppointmentStatus.Canceled => false,
//            _ => false
//        };
//    }

//    public bool SetAppointmentStatus(Appointment appointment, AppointmentStatus newStatus)
//    {
//        switch (newStatus)
//        {
//            case AppointmentStatus.Scheduled:
//                appointment.Schedule();
//                break;
//            case AppointmentStatus.Confirmed:
//                appointment.Confirm();
//                break;
//            case AppointmentStatus.Canceled:
//                appointment.Cancel();
//                break;
//            case AppointmentStatus.Completed:
//                appointment.Complete();
//                break;
//            default:
//                Notify("Status inválido.");
//                return false;
//        }

//        return true;
//    }

//    private bool HasTimeConflict(Appointment appointment, IEnumerable<Appointment?> existingAppointments)
//    {
//        var newStart = appointment.StartTime;
//        var newEnd = appointment.EndTime;

//        foreach (var existing in existingAppointments)
//        {
//            if (existing == null) continue;

//            var existingStart = existing.StartTime;
//            var existingEnd = existing.EndTime;

//            bool overlaps = newStart < existingEnd && newEnd > existingStart;
//            if (overlaps)
//                return true;
//        }

//        return false;
//    }
//}