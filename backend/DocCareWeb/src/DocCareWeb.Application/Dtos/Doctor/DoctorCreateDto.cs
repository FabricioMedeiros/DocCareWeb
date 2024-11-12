using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Doctor
{
    public class DoctorCreateDto : DoctorBaseDto
    {
        [JsonPropertyOrder(7)]
        public required int SpecialtyId { get; set; }
    }
}
