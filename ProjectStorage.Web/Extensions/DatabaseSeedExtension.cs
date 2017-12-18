namespace ProjectStorage.Web.Extensions
{
    using Constants;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class DatabaseSeedExtension
    {
        public static IApplicationBuilder SeedAndMigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ProjectStorageDbContext>();
                db.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                List<string> roles = typeof(GlobalConstants).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.Name.EndsWith("Role")).Select(f => f.GetValue(typeof(GlobalConstants))).Cast<string>().ToList();
                Task.Run(async () =>
                {
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                        bool existsSeededUser = !(await userManager.GetUsersInRoleAsync(role)).Any();
                        if (existsSeededUser)
                        {
                            User user = new User()
                            {
                                Email = $"{role}@users.com",
                                UserName = $"{role}@users.com"
                            };

                            await userManager.CreateAsync(user, role.ToLower());
                            await userManager.AddToRoleAsync(user, role);
                        }
                    }
                }).GetAwaiter().GetResult();
            }

            return app;
        }
    }
}