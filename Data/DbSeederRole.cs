using Microsoft.AspNetCore.Identity;
using ASM.Constants;

namespace ASM.Data
{
    public class DbSeederRole {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Staff.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Customer.ToString()));

            //Create admin user
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true

            };
            var staff = new IdentityUser
            {
                UserName = "staff@gmail.com",
                Email = "staff@gmail.com",
                EmailConfirmed = true
            };
            var adminInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (adminInDb is null) {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
            var staffInDb = await userMgr.FindByEmailAsync(staff.Email);
            if (staffInDb is null)
            {
                await userMgr.CreateAsync(staff, "Staff@123");
                await userMgr.AddToRoleAsync(staff, Roles.Staff.ToString());
            }

        }
    }
    
    
}
