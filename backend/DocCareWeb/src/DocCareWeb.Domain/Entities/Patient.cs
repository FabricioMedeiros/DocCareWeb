using DocCareWeb.Domain.Enums;
using System.Net;
using System.Reflection;

namespace DocCareWeb.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public required string Name { get; set; }
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CellPhone { get; set; }
        public string? Notes { get; set; }
        public int HealthPlanId { get; set; }
        public HealthPlan? HealthPlan { get; set; }
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}
