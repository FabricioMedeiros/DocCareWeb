namespace DocCareWeb.Application.Dtos.Doctor
{
    public abstract class DoctorBaseDto
    {
        public required string Name { get; set; }
        public required int SpecialtyId { get; set; }
        public string? Crm { get; set; }
        public string? Email { get; set; }
        public string? CellPhone { get; set; }
        public string? Phone { get; set; }
    }
}
