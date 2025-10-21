using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Service
{
    public class ServiceCreateDto : ServiceBaseDto    
    {
        [JsonPropertyOrder(3)]
        public required decimal BasePrice { get; set; }
    }
}
