using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BettingApplication.Data;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace BettingApplication.Models
{
    public static class SeedData
    {
        public static void methodSeedData(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                AppUser user = new AppUser();
                user.FirstName = "Admin";
                user.LastName = "Admin";
                user.Age = 18;
                user.UserName = "Admin";
                user.EmailConfirmed = true;
                IdentityResult result = userManager.CreateAsync(user, "Adminpass1").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                    var wallet = new Wallet();
                    wallet.User = user;
                    wallet.Saldo = 9999999;
                }
            }
        }
        public static void InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new BettingApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BettingApplicationContext>>()))
            {
               
                if (context.AdminTopMatchConfig.Any())
                {
                    return;   // DB has been seeded
                }
                context.AdminTopMatchConfig.AddRange(
                   new AdminTopMatchConfig
                   {
                       MinimumNumberOfMatches = 5
                   });
               
                context.SaveChanges();
            }
        }
    }
}