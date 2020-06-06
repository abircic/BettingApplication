using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BettingApplication.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly BettingApplicationContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccountService(BettingApplicationContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        public async Task Register(AppUser user)
        {
            _userManager.AddToRoleAsync(user, "User").Wait();
            //await signInManager.SignInAsync(user, isPersistent:false);
            Wallet wallet = new Wallet();
            wallet.User = user;
            wallet.Saldo = 0.00m;
            await _context.Wallet.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            var user = await _context.Users.Where(a => a.UserName == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _context.Users.Where(x => x.UserName == username).SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.Where(x => x.Id == id).SingleOrDefaultAsync();
        }
        public async Task<List<UsersViewModel>> GetUsersList()
        {
            var usersList = (from user in _context.Users.Where(u => u.UserName != "Admin")
                select new UsersViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Email = user.Email
                }).ToList();
            return usersList;
        }

        public async Task<List<UsersViewModel>> GetUsersForActivate()
        {
            var usersList = (from user in _context.Users.Where(u => u.UserName != "Admin" && u.EmailConfirmed == false)
                select new UsersViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Email = user.Email
                }).ToList();
            return usersList;
        }
        public async Task DeleteUser(string id)
        {
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == id))
            {
                _context.Remove(item);
            }
            foreach (UserTransaction item in _context.UserTransaction.Where(b => b.UserId == id))
            {
                _context.Remove(item);
            }
            foreach (UserBetMatch item in _context.UserBetMatch.Where(b => b.UserBet.User.Id == id))
            {
                _context.Remove(item);
            }
            foreach (UserBet item in _context.UserBet.Where(b => b.User.Id == id))
            {
                _context.Remove(item);
            }
        }

        public async Task ActivateUser(AppUser applicationUser)
        {
            applicationUser.EmailConfirmed = true;
            await _context.SaveChangesAsync();
        }
        //private async Task CreateRolesandUsers()
        //{
        //    bool x = await roleManager.RoleExistsAsync("Admin");
        //    if (!x)
        //    {
        //        // first we create Admin rool    
        //        var role = new IdentityRole();
        //        role.Name = "Admin";
        //        await roleManager.CreateAsync(role);

        //        //Here we create a Admin super user who will maintain the website                   

        //        var user = new AppUser();
        //        user.UserName = "admin";
        //        string userPWD = "admin";
        //        IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = await _userManager.AddToRoleAsync(user, "Admin");
        //        }
        //    }

        //    // creating Creating Manager role     
        //    x = await roleManager.RoleExistsAsync("User");
        //    if (!x)
        //    {
        //        var role = new IdentityRole();
        //        role.Name = "User";
        //        await roleManager.CreateAsync(role);
        //    }
    }
}
