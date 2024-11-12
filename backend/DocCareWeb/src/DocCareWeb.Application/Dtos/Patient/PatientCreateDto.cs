using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientCreateDto : PatientBaseDto
    {
        protected PatientCreateDto()
        {
        }

        public PatientCreateDto(string name, string? cpf, string? rg, Gender gender, DateTime birthDate, string? email, string? phone, string? cellPhone, string? notes, AddressCreateDto address)
        : base(name, cpf, rg, gender, birthDate, email, phone, cellPhone, notes)
        {
            Address = address;
        }

        [JsonPropertyOrder(100)]
        public required int HealthPlanId { get; set; }

        [JsonPropertyOrder(101)]
        public required AddressCreateDto Address { get; set; }
    }
}
