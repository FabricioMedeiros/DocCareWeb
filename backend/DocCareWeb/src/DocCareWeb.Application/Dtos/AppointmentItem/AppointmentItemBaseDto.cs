using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.AppointmentItem
{
    public abstract class AppointmentItemBaseDto
    {
        [JsonPropertyOrder(1)]
        public int ServiceId { get; set; }

        [JsonPropertyOrder(3)]
        public required decimal SuggestedPrice { get; set; }

        [JsonPropertyOrder(4)]
        public required decimal Price { get; set; }
    }
}
