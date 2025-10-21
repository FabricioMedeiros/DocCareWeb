using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.AppointmentItem
{
    public class AppointmentItemListDto : AppointmentItemBaseDto
    {

        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;
    }
}
