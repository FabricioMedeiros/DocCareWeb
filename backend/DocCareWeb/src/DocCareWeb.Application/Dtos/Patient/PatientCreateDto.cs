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

        public PatientCreateDto(string name, string? cpf, string? rg, Gender gender, DateTime birthDate, string? email, string? phone, string? cellPhone, string? notes, int healthPlanId, AddressCreateDto address)
        : base(name, cpf, rg, gender, birthDate, email, phone, cellPhone, notes, healthPlanId)
        {
            Address = address;
        }

        [JsonPropertyOrder(100)]
        public required AddressCreateDto Address { get; set; }
    }
}
