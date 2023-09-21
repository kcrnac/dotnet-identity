using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Abstraction.Services;
using IdentityService.Entities;
using IdentityService.Infrastructure.Configuration.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _options;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        }

        public string GenerateToken([NotNull] User user, List<string> roles)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(user.Email);

            var claims = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email ),
                new (ClaimTypes.GivenName, user.FullName )
            };

            if (roles.Any())
            {
                var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

                claims.AddRange(roleClaims);
            }
            
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(double.Parse(_options.Value.Expires)),
                SigningCredentials = credentials,
                Issuer = _options.Value.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
