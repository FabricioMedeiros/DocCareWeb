using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Service
{
    public abstract class ServiceBaseDto
    {
        [JsonPropertyOrder(2)]
        public required string Name { get; set; }
    }
}
