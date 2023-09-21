using IdentityService.Entities;

namespace IdentityService.Abstraction.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
