namespace DocCareWeb.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = null!;
        public string? Crm { get; set; }
        public string? Email { get; set; }
        public string? CellPhone { get; set; }
        public string? Phone { get; set; }
    }
}
