namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentCreateDto : AppointmentBaseDto
    {
        public required int DoctorId { get; set; }
        public required int PatientId { get; set; }
        public required int HealthPlanId { get; set; }
    }
}
