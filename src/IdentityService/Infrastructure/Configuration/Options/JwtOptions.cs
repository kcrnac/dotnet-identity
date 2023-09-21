using IdentityService.Abstraction.Configuration;

namespace IdentityService.Infrastructure.Configuration.Options
{
    public class JwtOptions : IConfigurationOption
    {
        public string Issuer { get; init; }

        public string Audience { get; init; }

        public string Key { get; init; }

        public string Expires { get; init; }

        public string GetSectionName() => "Jwt";
    }
}
