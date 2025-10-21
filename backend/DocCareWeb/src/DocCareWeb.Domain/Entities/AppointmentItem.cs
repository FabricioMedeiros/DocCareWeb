namespace DocCareWeb.Domain.Entities
{
    public class AppointmentItem : BaseEntity
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public decimal SuggestedPrice { get; set; }
        public decimal Price { get; set; }
    }
}
