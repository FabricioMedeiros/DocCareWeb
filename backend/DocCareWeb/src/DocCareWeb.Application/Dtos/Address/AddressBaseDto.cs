using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Address
{
    public abstract class AddressBaseDto
    {
        protected AddressBaseDto()
        {
        }
        
        [JsonConstructor]
        public AddressBaseDto(string street, string number, string? complement, string neighborhood, string city, string state, string zipCode)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
        }     

        public required string Street { get; set; }
        public required string Number { get; set; }
        public string? Complement { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
    }
}
