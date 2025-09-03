namespace DocCareWeb.Application.Dtos.Appointment
{
    public class AppointmentUpdateDto : AppointmentBaseDto
    {
        public int Id { get; set; }
        public required int DoctorId { get; set; }
        public required int PatientId { get; set; }
        public required int HealthPlanId { get; set; }
    }
}
