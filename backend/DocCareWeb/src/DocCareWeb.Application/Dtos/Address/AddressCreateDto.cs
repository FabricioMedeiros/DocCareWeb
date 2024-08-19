namespace DocCareWeb.Application.Dtos.Address
{
    public class AddressCreateDto : AddressBaseDto
    {
        public AddressCreateDto(string street, string number, string? complement, string neighborhood, string city, string state, string zipCode) : base(street, number, complement, neighborhood, city, state, zipCode)
        {
        }
    }
}
