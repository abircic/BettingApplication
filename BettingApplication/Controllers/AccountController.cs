using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace BettingApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountController> logger;
        public AccountController(BettingApplicationContext context, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsUsernameExist(model.UserName);
                if (isExist)
                {
                    ModelState.AddModelError("UsernameExist", "Username already exist");
                    return View(model);
                }
                var user = new AppUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Email= model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                    //await signInManager.SignInAsync(user, isPersistent:false);
                    Wallet wallet = new Wallet();
                    wallet.User = user;
                    wallet.Saldo = 0.00m;
                    _context.Wallet.Add(wallet);
                    _context.SaveChanges();
                    ViewBag.ErrorTitle = "Registration succesful";
                    ViewBag.ErrorMessage = "Before you can login, Admin need to activate your account :)";
                    return View("View");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded && model.UserName == "admin")
                {
                    return RedirectToAction("UsersForActivate", "Account");
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                var user = _context.Users.Where(x => x.UserName == model.UserName).SingleOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Wrong username");
                    return View();
                }
                else if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError(string.Empty, "Sorry account is not verified yet");
                }
                ModelState.AddModelError(string.Empty, "Wrong password");

            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public bool IsUsernameExist(string username)
        {
            var v = _context.Users.Where(a => a.UserName == username).FirstOrDefault();
            if (v == null)
            {
                return false;
            }
            else
                return true;
        }

        public IActionResult UsersList()
        {
            var usersList = (from user in _context.Users.Where(u=>u.UserName!="Admin")
                             select new UsersViewModel
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Age = user.Age,
                                 Email = user.Email
                             }).ToList();
            return View(usersList);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                AppUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    name = applicationUser.FirstName;
                }
                UsersViewModel test = new UsersViewModel();
                test.FirstName = applicationUser.FirstName;
                test.LastName = applicationUser.LastName;
            }
            return View();
        }
        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            if (!String.IsNullOrEmpty(id))
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

                var user = _context.Wallet.Where(u => u.User.Id == id).FirstOrDefault();
                _context.Wallet.Remove(user);
                _context.SaveChanges();

                AppUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UsersList");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult UsersForActivate()
        {
            var usersList = (from user in _context.Users.Where(u => u.UserName != "Admin" && u.EmailConfirmed==false)
                             select new UsersViewModel
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Age = user.Age,
                                 Email = user.Email
                             }).ToList();
            return View(usersList);
        }
        [HttpPost, ActionName("AktivateUser")]
        public async Task<IActionResult> UsersForActivate(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                AppUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                   applicationUser.EmailConfirmed = true;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("UsersForActivate", "Account");
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
        //        IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

        //        //Add default User to Role Admin    
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = await userManager.AddToRoleAsync(user, "Admin");
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