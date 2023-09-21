using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entities;

public class Role : IdentityRole
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}

public static class UserRoles
{
    public const string Administrator = "Administrator";
    public const string SuperUser = "SuperUser";
    public const string User = "User";

}