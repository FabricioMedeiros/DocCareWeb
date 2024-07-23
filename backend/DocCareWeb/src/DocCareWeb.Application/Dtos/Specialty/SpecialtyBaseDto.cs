namespace DocCareWeb.Application.Dtos.Specialty
{
    public abstract class SpecialtyBaseDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
