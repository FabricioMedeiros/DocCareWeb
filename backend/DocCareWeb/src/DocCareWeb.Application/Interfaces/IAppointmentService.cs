using DocCareWeb.Application.Dtos.Appointment;
using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IAppointmentService : IGenericService<Appointment, AppointmentCreateDto, AppointmentUpdateDto, AppointmentListDto>
    {
    }
}
