using System.ComponentModel.DataAnnotations;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public abstract class AppointmentBaseDto
    {
        public required string AppointmentDate { get; set; }

        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido. Use hh:mm.")]
        public required string StartTime { get; set; }
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido. Use hh:mm.")]
        public required string EndTime { get; set; }
        public required decimal Cost { get; set; }
        public string? Notes { get; set; }
    }
}
