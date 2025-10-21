using DocCareWeb.Application.Dtos.HealthPlanItem;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.HealthPlan
{
    public class HealthPlanUpdateDto : HealthPlanBaseDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(3)]
        public ICollection<HealthPlanItemUpdateDto> Items { get; set; } = new List<HealthPlanItemUpdateDto>();
    }
}
