namespace DocCareWeb.Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }
    }
}
