using System.Data.Common;
using Microsoft.AspNetCore.Identity;

namespace StartSpelerAPI.Services
{
    public class IdentitySeeding
    {
        public async Task RoleSeedingAsync(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                //User
                bool role = await roleManager.RoleExistsAsync("user");
                if (!role) await roleManager.CreateAsync(new IdentityRole("user"));

                //Commmunity manager
                role = await roleManager.RoleExistsAsync("communitymanager");
                if (!role) await roleManager.CreateAsync(new IdentityRole("communitymanager"));

                //Admin
                role = await roleManager.RoleExistsAsync("admin");
                if (!role) await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
