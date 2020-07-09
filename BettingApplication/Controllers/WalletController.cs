using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using System.Globalization;
using BettingApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BettingApplication.Services.Interfaces;
using BettingApplicationContext = BettingApplication.Data.BettingApplicationContext;

namespace BettingApplication.Controllers
{   
    [Authorize]
    public class WalletController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly IAccountService _accountService;
        private readonly IWalletService _walletService;

        public WalletController(BettingApplicationContext context, IAccountService accountService, IWalletService walletService)
        {
            _context = context;
            _accountService = accountService;
            _walletService = walletService;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserById(userId);
            var wallet = await _walletService.GetWallet(userId);
            TempData["Username"] = user.UserName;
            TempData["Saldo"] = wallet.Saldo + " kn";
            return View(wallet);
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            return View(await _context.BetSlip.ToListAsync());//TODO
        }
        [HttpPost]
        public async Task<IActionResult> Payment(string submit, string stake)
        {
            TempData["msg"] = null;
            var walletStake = decimal.Parse(stake, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            if (walletStake < 10)
            {
                TempData["msg"] = "Minimum is 10 kn";
                return RedirectToAction("Index");
            }
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserById(userId);
            var wallet = await _walletService.GetWallet(userId);

            UserTransaction transaction = new UserTransaction();
            List<UserTransaction> listTransactions = new List<UserTransaction>();
            if(submit=="CashIn")
            {
                await _walletService.CashIn(walletStake, stake, userId);
                TempData["msg"] = "The transaction is successful";
            }
            else
            {
                if ((wallet.Saldo -= walletStake) >= 0)
                {
                    await _walletService.CashOut(userId, stake);
                    TempData["msg"] = "The transaction is successful";
                }
                else
                {
                    TempData["msg"] = "You dont have enough funds for this transaction.";
                    return RedirectToAction("Index");
                }
            } 
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Transactions()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = _walletService.GetUserTransactions(userId);
            return View(response);
        }
    }
}
