using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CellPhone { get; set; }
        public string? Notes { get; set; }
        public int HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}
