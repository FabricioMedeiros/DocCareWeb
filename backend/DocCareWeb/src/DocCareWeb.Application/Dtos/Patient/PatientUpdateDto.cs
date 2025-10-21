using DocCareWeb.Application.Dtos.Address;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientUpdateDto : PatientBaseDto
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }

        [JsonPropertyOrder(100)]
        public required int HealthPlanId { get; set; }

        [JsonPropertyOrder(101)]
        public required AddressUpdateDto Address { get; set; }
    }
}
