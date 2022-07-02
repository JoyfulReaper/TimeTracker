using Microsoft.AspNetCore.Identity;

namespace TimeTrackerApi;

public static class AdminUserSeeder
{
    public static void SeedAdminUser(this WebApplication app)
    {
        SeedAdmin(app).GetAwaiter().GetResult();
    }

    // Seed the user store with the initial user data.
    private async static Task SeedAdmin(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            IConfiguration config =
                scope.ServiceProvider.GetRequiredService<IConfiguration>();
            UserManager<IdentityUser> userManager =
                scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string roleName = config["Admin:Role"] ?? "Admin";
            string userName = config["Admin:User"] ?? "Admin";
            string email = config["Admin:Email"] ?? "admin@example.com";
            string password = config["Admin:Password"] ?? "Pass123!";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            
            IdentityUser adminUser =
                await userManager.FindByNameAsync(userName);
            
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(adminUser);
                adminUser = await userManager.FindByNameAsync(userName);
                var res = await userManager.AddPasswordAsync(adminUser, password);
            }
            
            if (!await userManager.IsInRoleAsync(adminUser, roleName))
            {
                await userManager.AddToRoleAsync(adminUser, roleName);
            }
        }
    }
}