using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlan
{
    public abstract class HealthPlanBaseDto
    {
        [JsonPropertyOrder(2)]
        public required string Name { get; set; }
    }
}
