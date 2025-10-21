using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public abstract class PatientBaseDto
    {
        [JsonPropertyOrder(1)]
        public required string Name { get; set; }
        [JsonPropertyOrder(2)]
        public string? Cpf { get; set; }
        [JsonPropertyOrder(3)]
        public string? Rg { get; set; }
        [JsonPropertyOrder(4)]
        public Gender Gender { get; set; }
        [JsonPropertyOrder(5)]
        public DateTime BirthDate { get; set; }
        [JsonPropertyOrder(6)]
        public string? Email { get; set; }
        [JsonPropertyOrder(7)]
        public string? Phone { get; set; }
        [JsonPropertyOrder(8)]
        public string? CellPhone { get; set; }
        [JsonPropertyOrder(9)]
        public string? Notes { get; set; }
    }
}
