using DocCareWeb.Application.Dtos.AppointmentItem;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentUpdateDto : AppointmentBaseDto
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }
        [JsonPropertyOrder(1)]
        public required int DoctorId { get; set; }
        [JsonPropertyOrder(2)]
        public required int PatientId { get; set; }
        [JsonPropertyOrder(3)]
        public required int HealthPlanId { get; set; }
        [JsonPropertyOrder(7)]
        public AppointmentStatus Status { get; set; }
        [JsonPropertyOrder(10)]
        public ICollection<AppointmentItemUpdateDto> Items { get; set; } = new List<AppointmentItemUpdateDto>();
    }
}
