namespace DocCareWeb.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public required string Name { get; set; }
        public required int SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }
        public string? Crm { get; set; }
        public string? Email { get; set; }
        public string? CellPhone { get; set; }
        public string? Phone { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
