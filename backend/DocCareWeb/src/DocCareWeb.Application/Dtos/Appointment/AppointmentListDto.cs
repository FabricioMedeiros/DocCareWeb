using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentListDto : AppointmentBaseDto
    {
        public int Id { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
