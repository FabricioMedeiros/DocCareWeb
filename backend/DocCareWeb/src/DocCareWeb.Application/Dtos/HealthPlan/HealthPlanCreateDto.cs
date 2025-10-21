using DocCareWeb.Application.Dtos.HealthPlanItem;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlan
{
    public class HealthPlanCreateDto : HealthPlanBaseDto
    {
        [JsonPropertyOrder(3)]
        public ICollection<HealthPlanItemCreateDto> Items { get; set; } = new List<HealthPlanItemCreateDto>();
    }
}
