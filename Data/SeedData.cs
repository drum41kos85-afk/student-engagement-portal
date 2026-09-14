using Microsoft.AspNetCore.Identity;
using StudentEngagementPortal.Models;

namespace StudentEngagementPortal.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@portal.local";
            const string adminPassword = "Admin123!"; // change before submitting/demoing

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Role = "Admin",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, adminPassword);
            }
        }
    }
}