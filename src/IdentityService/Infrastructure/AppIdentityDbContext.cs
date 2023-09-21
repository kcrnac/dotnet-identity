using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<User, Role, string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private static readonly string Password = "Pa$$w0rd";

        public static async Task SeedDatabaseAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new List<Role>();

            if (!roleManager.Roles.Any())
            {
                roles.Add(new Role(Entities.UserRoles.Administrator));
                roles.Add(new Role(Entities.UserRoles.SuperUser));
                roles.Add(new Role(Entities.UserRoles.User));

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                var userBob = new User
                {
                    FirstName = "Bob",
                    LastName = "Doe",
                    UserName = "bob.doe",
                    Email = "bob@test.com"
                };

                var resultBob = await userManager.CreateAsync(userBob, Password);
                if (resultBob.Succeeded && roles.Any())
                {
                    await userManager.AddToRoleAsync(userBob, roles[0].Name!);
                }

                var userAlice = new User
                {
                    FirstName = "Alice",
                    LastName = "Doe",
                    UserName = "alice.doe",
                    Email = "alice@test.com"
                };

                var resultAlice = await userManager.CreateAsync(userAlice, Password);
                if (resultAlice.Succeeded && roles.Any())
                {
                    await userManager.AddToRoleAsync(userAlice, roles[1].Name!);
                }
            }
        }
    }
}
