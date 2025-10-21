using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Service
{
    public class ServiceUpdateDto : ServiceBaseDto
    {
        [JsonPropertyOrder(1)]
        public required int Id { get; set; }

        [JsonPropertyOrder(3)]
        public required decimal BasePrice { get; set; }
    }
}
