using Microsoft.AspNetCore.Identity;

namespace DocCareWeb.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
    }
}