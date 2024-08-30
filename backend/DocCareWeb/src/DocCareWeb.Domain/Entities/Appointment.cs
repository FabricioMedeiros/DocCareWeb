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
        public AppointmentStatus Status { get; private set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }

        public void Schedule()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("A consulta deve estar no status 'Scheduled' para ser agendada.");
            }

            Status = AppointmentStatus.Scheduled;
        }

        public void Confirm()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("A consulta deve estar no status 'Scheduled' para ser confirmada.");
            }

            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Não é possível cancelar uma consulta já completada.");
            }

            Status = AppointmentStatus.Canceled;
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                throw new InvalidOperationException("A consulta deve estar no status 'Confirmed' para ser completada.");
            }

            Status = AppointmentStatus.Completed;
        }
    }
}

