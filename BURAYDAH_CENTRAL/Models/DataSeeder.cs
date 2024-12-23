


using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BURAYDAH_CENTRAL.Models
{
   

    public static class DataSeeder
    {
        public static async Task SeedSuperAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "abdo@admin.com";
            string password = "SuperAdmin123!";
            string roleName = "SuperAdmin";

            // Ensure the role exists
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Check if the user exists
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Create the user
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // Assign the role to the user
                    await userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                // Ensure the user is in the role
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }

}
