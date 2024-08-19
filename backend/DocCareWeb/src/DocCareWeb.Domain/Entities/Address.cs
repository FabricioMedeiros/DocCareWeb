namespace DocCareWeb.Domain.Entities
{
    public class Address : BaseEntity
    {
        public required string Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public required string City { get; set; }
        public string? Neighborhood { get; set; }        
        public required string State { get; set; }
        public required string ZipCode { get; set; }
    }
}
