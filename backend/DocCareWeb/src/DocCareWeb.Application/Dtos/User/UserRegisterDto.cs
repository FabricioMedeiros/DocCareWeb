namespace DocCareWeb.Application.Dtos.User
{
    public class UserRegisterDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
