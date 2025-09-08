using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Application.Interfaces
{
    public interface IAppointmentService : IGenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>
    {
        Task<bool> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus);
    }
}
