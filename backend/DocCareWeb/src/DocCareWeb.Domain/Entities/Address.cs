﻿namespace DocCareWeb.Domain.Entities
{
    public class Address : BaseEntity
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public string? Neighborhood { get; set; }        
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public string? Country { get; set; }
        public ICollection<Patient>? Patients { get; set; }
    }
}
