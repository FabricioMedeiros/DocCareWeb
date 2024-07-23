using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Domain.Enums;

namespace DocCareWeb.Application.Dtos.Patient
{
    public abstract class PatientBaseDto
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
        public AddressBaseDto Address { get; set; } = null!;
    }
}
