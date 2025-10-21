using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlanItem
{
    public abstract class HealthPlanItemBaseDto
    {
        [JsonPropertyOrder(1)]
        public int ServiceId { get; set; }

        [JsonPropertyOrder(4)]
        public required decimal Price { get; set; }
    }
}
