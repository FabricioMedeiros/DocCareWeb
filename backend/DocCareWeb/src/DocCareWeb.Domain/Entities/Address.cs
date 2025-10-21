namespace DocCareWeb.Domain.Entities
{
    public class Address : BaseEntity
    {
        public int PatientId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public required string City { get; set; }
        public string? Neighborhood { get; set; }        
        public  string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
