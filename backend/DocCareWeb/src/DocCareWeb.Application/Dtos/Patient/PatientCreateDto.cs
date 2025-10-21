using DocCareWeb.Application.Dtos.Address;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientCreateDto : PatientBaseDto
    {
        [JsonPropertyOrder(100)]
        public required int HealthPlanId { get; set; }

        [JsonPropertyOrder(101)]
        public required AddressCreateDto Address { get; set; }
    }
}
