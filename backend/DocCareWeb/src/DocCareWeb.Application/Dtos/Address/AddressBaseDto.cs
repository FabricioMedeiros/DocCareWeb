using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Address
{
    public abstract class AddressBaseDto
    {        
        public required string Street { get; set; }
        public required string Number { get; set; }
        public string? Complement { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
    }
}
