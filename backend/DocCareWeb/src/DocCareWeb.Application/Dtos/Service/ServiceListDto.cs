using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Service
{
    public class ServiceListDto : ServiceBaseDto
    {
        [JsonPropertyOrder(1)]
        public required int Id { get; set; }
        [JsonPropertyOrder(3)]
        public required decimal Price { get; set; }
        [JsonPropertyOrder(4)]
        public bool IsHealthPlanPrice { get; set; } = false;
    }
}
