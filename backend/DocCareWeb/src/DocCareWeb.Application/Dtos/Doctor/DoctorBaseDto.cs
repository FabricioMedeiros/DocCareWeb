using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Doctor
{
    public abstract class DoctorBaseDto
    {
        [JsonPropertyOrder(2)]
        public required string Name { get; set; }
        [JsonPropertyOrder(3)]        
        public string? Crm { get; set; }
        [JsonPropertyOrder(4)]
        public string? Email { get; set; }
        [JsonPropertyOrder(5)]
        public string? CellPhone { get; set; }
        [JsonPropertyOrder(6)]            
        public string? Phone { get; set; }
    }
}
