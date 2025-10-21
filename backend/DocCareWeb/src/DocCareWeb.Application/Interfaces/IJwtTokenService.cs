using DocCareWeb.Domain.Entities;

namespace DocCareWeb.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
