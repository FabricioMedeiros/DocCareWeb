using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public abstract class PatientBaseDto
    {
        protected PatientBaseDto()
        {
        }

        [JsonConstructor]
        public PatientBaseDto(string name, string? cpf, string? rg, Gender gender, DateTime birthDate, string? email, string? phone, string? cellPhone, string? notes, int healthPlanId)
        {
            Name = name;
            Cpf = cpf;
            Rg = rg;
            Gender = gender;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            CellPhone = cellPhone;
            Notes = notes;
            HealthPlanId = healthPlanId;
        }

        [JsonPropertyOrder(2)]
        public required string Name { get; set; }
        [JsonPropertyOrder(3)]
        public string? Cpf { get; set; }
        [JsonPropertyOrder(4)]
        public string? Rg { get; set; }
        [JsonPropertyOrder(5)]
        public Gender Gender { get; set; }
        [JsonPropertyOrder(6)]
        public DateTime BirthDate { get; set; }
        [JsonPropertyOrder(7)]
        public string? Email { get; set; }
        [JsonPropertyOrder(8)]
        public string? Phone { get; set; }
        [JsonPropertyOrder(9)]
        public string? CellPhone { get; set; }
        [JsonPropertyOrder(10)]
        public string? Notes { get; set; }
        [JsonPropertyOrder(11)]
        public int HealthPlanId { get; set; }
    }
}
