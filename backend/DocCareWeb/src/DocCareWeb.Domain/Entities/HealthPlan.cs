namespace DocCareWeb.Domain.Entities
{
    public class HealthPlan : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Cost { get; set; }
        public ICollection<Patient>? Patients { get; set; }
    }
}
