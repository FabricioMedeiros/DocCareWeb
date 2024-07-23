using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public abstract class AppointmentBaseDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int HealthPlanId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public required decimal Cost { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
