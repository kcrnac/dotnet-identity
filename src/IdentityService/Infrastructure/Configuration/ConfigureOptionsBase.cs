using IdentityService.Abstraction.Configuration;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Configuration
{
    internal class ConfigureOptionsBase<T> : IConfigureOptions<T> where T : class, IConfigurationOption
    {
        private readonly IConfiguration _configuration;
        private readonly string _sectionName;


        public ConfigureOptionsBase(IConfiguration configuration, string sectionName)
        {
            _configuration = configuration;
            _sectionName = sectionName;
        }

        public void Configure(T options)
        {
            _configuration.GetSection(_sectionName).Bind(options);
        }
    }
}
