using AspNetSecurityApplication.Models;

namespace AspNetSecurityApplication.Services;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}
