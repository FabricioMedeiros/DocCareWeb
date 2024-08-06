using DocCareWeb.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocCareWeb.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
