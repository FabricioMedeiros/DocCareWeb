using DocCareWeb.Application.Dtos.AppointmentItem;
using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentListDto : AppointmentBaseDto
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }
        [JsonPropertyOrder(1)]
        public required DoctorBasicInfoDto Doctor { get; set; }
        [JsonPropertyOrder(2)]
        public required PatientBasicInfoDto Patient { get; set; }
        [JsonPropertyOrder(3)]
        public required HealthPlanBasicInfoDto HealthPlan { get; set; }
        [JsonPropertyOrder(7)]
        public AppointmentStatus Status { get; set; }
        [JsonPropertyOrder(8)]
        public decimal TotalAmount => Items.Sum(item => item.Price);
        [JsonPropertyOrder(10)]
        public ICollection<AppointmentItemListDto> Items { get; set; } = new List<AppointmentItemListDto>();
    }
}
