using DemoIdentity.Data;
using DemoIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Net;

namespace DemoIdentity
{
    public class IdentitySeeding
    {
        public async Task IdentitySeedingAsync(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager) {
            try
            {

                // Rollen aanmaken 
                // User
                bool role = await roleManager.RoleExistsAsync("user");
                if (!role) await roleManager.CreateAsync(new IdentityRole("user"));

                // Admin
                role = await roleManager.RoleExistsAsync("admin");
                if (!role) await roleManager.CreateAsync(new IdentityRole("admin"));

                // superadmin
                role = await roleManager.RoleExistsAsync("superadmin");
                if (!role) await roleManager.CreateAsync(new IdentityRole("superadmin"));

                // Gebruiker aanmaken
                // Gebruiker: Admin naam bestaat nog niet?
                // Eerste admin wordt superadmin
                if (userManager.FindByNameAsync("Admin").Result == null)
                {
                    // Gebruiker voorzien
                    var defaultUser = new CustomUser
                    {
                        UserName = "StealthFish",
                        Email = "dries.hoefkens@thomasmore.be"
                    };

                    // Gebruiker aanmaken met rol Superadmin
                    var admin = await userManager.CreateAsync(defaultUser, "t0LTHxzy.v");
                    if (admin.Succeeded && userManager.FindByNameAsync("Admin").Result != null)
                        await userManager.AddToRoleAsync(defaultUser, "superadmin");

                }
            } 
             catch (DbException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}