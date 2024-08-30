using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentStatusDto
    {
        public int Id { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
