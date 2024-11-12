namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientBasicInfoDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public string? Phone { get; set; }
        public string? CellPhone { get; set; }
    }
}
