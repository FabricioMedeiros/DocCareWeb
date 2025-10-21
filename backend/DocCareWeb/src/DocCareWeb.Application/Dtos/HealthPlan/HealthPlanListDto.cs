using DocCareWeb.Application.Dtos.HealthPlanItem;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlan
{
    public class HealthPlanListDto : HealthPlanBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(3)]
        public ICollection<HealthPlanItemListDto> Items { get; set; } = new List<HealthPlanItemListDto>();
    }
}
