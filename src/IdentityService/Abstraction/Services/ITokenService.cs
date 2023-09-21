using IdentityService.Entities;

namespace IdentityService.Abstraction.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}
