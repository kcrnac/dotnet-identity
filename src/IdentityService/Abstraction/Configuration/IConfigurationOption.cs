namespace IdentityService.Abstraction.Configuration;

internal interface IConfigurationOption
{
    string GetSectionName();
}