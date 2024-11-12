using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Doctor
{
    public class DoctorUpdateDto : DoctorBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyOrder(7)]
        public required int SpecialtyId { get; set; }
    }
}
