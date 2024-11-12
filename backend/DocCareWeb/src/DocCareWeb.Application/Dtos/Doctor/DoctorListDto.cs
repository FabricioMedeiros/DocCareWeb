using DocCareWeb.Application.Dtos.Specialty;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Doctor
{
    public class DoctorListDto : DoctorBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyOrder(7)]
        public required SpecialtyListDto Specialty { get; set; }
    }
}
