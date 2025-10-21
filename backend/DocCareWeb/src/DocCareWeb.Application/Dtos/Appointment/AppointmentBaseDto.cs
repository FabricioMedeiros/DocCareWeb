using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public abstract class AppointmentBaseDto
    {
        [JsonPropertyOrder(4)]
        public required string AppointmentDate { get; set; }
        [JsonPropertyOrder(5)]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido. Use hh:mm.")]
        public required string StartTime { get; set; }
        [JsonPropertyOrder(6)]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido. Use hh:mm.")]
        public required string EndTime { get; set; }
        [JsonPropertyOrder(9)]
        public string? Notes { get; set; }
    }
}
