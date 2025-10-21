using DocCareWeb.Application.Dtos.AppointmentItem;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentCreateDto : AppointmentBaseDto
    {
        [JsonPropertyOrder(1)]
        public required int DoctorId { get; set; }
        [JsonPropertyOrder(2)]
        public required int PatientId { get; set; }
        [JsonPropertyOrder(3)]
        public required int HealthPlanId { get; set; }
        [JsonPropertyOrder(10)]
        public ICollection<AppointmentItemCreateDto> Items { get; set; } = new List<AppointmentItemCreateDto>();
    }
}
