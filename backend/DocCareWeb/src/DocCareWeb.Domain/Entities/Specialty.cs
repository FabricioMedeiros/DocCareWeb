namespace DocCareWeb.Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public string? Description { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }
    }
}
