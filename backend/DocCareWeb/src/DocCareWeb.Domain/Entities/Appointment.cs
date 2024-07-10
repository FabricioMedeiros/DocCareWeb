using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int HealthPlanId { get; set; }
        public HealthPlan? HealthPlan { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public required decimal Cost { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}
