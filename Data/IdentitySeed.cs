using System;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Data;

public static class IdentitySeed
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Admin","User",];
        foreach(var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminName = "admin";
        var adminEmail = "admin@gmail.com";
        var adminPassword = "Jun10";
        var adminCountry = Country.USA;
        var adminAge = 34;
        var adminSalary = "sogood";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if(adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminName,
                Email = adminEmail,
                Country = adminCountry,
                Age = adminAge,
                Salary = adminSalary,
            };

            var result = await userManager.CreateAsync(adminUser,adminPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Adminユーザー作成失敗");
            }
        }

        if(!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser,"Admin");
        }



    }

}
