using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountController> logger;
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, BettingApplicationContext context, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
            _accountService = accountService;
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
                var isExist = await _accountService.IsUsernameExist(model.UserName);
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
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _accountService.Register(user);
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
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded && model.UserName == "admin")
                {
                    return RedirectToAction("UsersForActivate", "Account");
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                var user = await _accountService.GetUser(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Wrong username");
                    return View();
                }
                if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError(string.Empty, "Sorry account is not verified yet");
                }
                ModelState.AddModelError(string.Empty, "Wrong password");

            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
       

        public async Task<IActionResult> UsersList()
        {
            var response = await _accountService.GetUsersList();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                AppUser applicationUser = await _userManager.FindByIdAsync(id);
                //UsersViewModel test = new UsersViewModel();
                //test.FirstName = applicationUser.FirstName;
                //test.LastName = applicationUser.LastName;
            }
            return View();
        }
        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {

                await _accountService.DeleteUser(id);
                var user = _context.Wallet.Where(u => u.User.Id == id).FirstOrDefault();//TODO
                _context.Wallet.Remove(user);
                _context.SaveChanges();

                AppUser applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UsersList");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UsersForActivate()
        {
            var response = await _accountService.GetUsersForActivate();
            return View(response);
        }

        [HttpPost, ActionName("AktivateUser")]
        public async Task<IActionResult> UsersForActivate(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                AppUser applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    _accountService.ActivateUser(applicationUser);
                }
            }
            return RedirectToAction("UsersForActivate", "Account");
        }

    }
}