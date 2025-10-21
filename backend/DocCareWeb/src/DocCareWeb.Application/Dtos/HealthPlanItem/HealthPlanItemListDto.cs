using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlanItem
{
    public class HealthPlanItemListDto : HealthPlanItemBaseDto
    {
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;
    }
}
