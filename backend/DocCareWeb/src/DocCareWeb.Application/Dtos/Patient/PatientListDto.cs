using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Application.Dtos.HealthPlan;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientListDto : PatientBaseDto
    {
        [JsonPropertyOrder(0)]
        public int Id { get; set; }

        [JsonPropertyOrder(100)]
        public required HealthPlanBasicInfoDto HealthPlan { get; set; }

        [JsonPropertyOrder(101)]
        public required AddressListDto Address { get; set; }
    }
}
