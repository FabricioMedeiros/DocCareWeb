using DocCareWeb.Application.Dtos.Doctor;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Application.Dtos.Patient;
using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentListDto : AppointmentBaseDto
    {
        public int Id { get; set; }
        public AppointmentStatus Status { get; set; }
        public required DoctorBasicInfoDto Doctor { get; set; }
        public required PatientBasicInfoDto Patient { get; set; }
        public required HealthPlanBasicInfoDto HealthPlan { get; set; }
    }
}
