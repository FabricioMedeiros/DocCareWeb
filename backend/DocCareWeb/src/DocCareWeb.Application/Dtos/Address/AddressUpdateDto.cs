namespace DocCareWeb.Application.Dtos.Address
{
    public class AddressUpdateDto : AddressBaseDto
    {
        public AddressUpdateDto(string street, string number, string? complement, string neighborhood, string city, string state, string zipCode) : base(street, number, complement, neighborhood, city, state, zipCode)
        {
        }

        public int Id { get; set; }

    }
}
