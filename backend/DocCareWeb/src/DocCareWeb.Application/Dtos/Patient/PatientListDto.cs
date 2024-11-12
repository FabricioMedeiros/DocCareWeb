using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientListDto : PatientBaseDto
    {
        public PatientListDto(string name, string? cpf, string? rg, Gender gender, DateTime birthDate, string? email, string? phone, string? cellPhone, string? notes, AddressListDto address)
        : base(name, cpf, rg, gender, birthDate, email, phone, cellPhone, notes)
        {
            Address = address;
        }

        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(100)]
        public required HealthPlanListDto HealthPlan { get; set; }

        [JsonPropertyOrder(101)]
        public required AddressListDto Address { get; set; }
    }
}
