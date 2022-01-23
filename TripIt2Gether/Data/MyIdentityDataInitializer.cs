using System;
using Microsoft.AspNetCore.Identity;
using TripIt2Gether.Models;

namespace TripIt2Gether.Data
{
    public class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        // name - poprawny adres email
        // password - min 8 znaków, mała i duża litera, cyfra i znak specjalny
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("TourOperator").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "TourOperator",
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Participant").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Participant",
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedOneUser(UserManager<ApplicationUser> userManager, string name, string password, string role = null)
        {
            if (userManager.FindByNameAsync(name).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = name,
                    Email = name
                };

                IdentityResult result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded && role != null)
                {
                    userManager.AddToRoleAsync(user, role).Wait();
                }
            }
        }
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            SeedOneUser(userManager, "TripOperator@localhost", "tripOper1!", "TourOperator");
            SeedOneUser(userManager, "participant1@localhost", "pUpass1!", "Participant");
            SeedOneUser(userManager, "participant2@localhost", "pUpass1!", "Participant");
        }
    }
}
