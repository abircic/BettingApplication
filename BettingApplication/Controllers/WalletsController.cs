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
using BettingApplicationContext = BettingApplication.Data.BettingApplicationContext;

namespace BettingApplication.Controllers
{   
    [Authorize]
    public class WalletsController : Controller
    {
        private readonly BettingApplicationContext _context;

        public WalletsController(BettingApplicationContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var wallet = _context.Wallet.Where(x => x.User == user).Include(t => t.Transactions).ToListAsync();
            TempData["Username"] = user.UserName;
            return View(await wallet);
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Payment(string submit, string stake)
        {
            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            //string userId = ViewBag.UserId;
            //var wallet = _context.Wallet.Where(x => x.Userid.ToString() == userId).FirstOrDefault();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
            TempData["msg"] = null;
            var walletStake = decimal.Parse(stake, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            if(walletStake < 10)
            {
                TempData["msg"] = "Minimum is 10 kn";
                return RedirectToAction("Index");
            }
            UserTransactions transaction = new UserTransactions();
            List<UserTransactions> listTransactions = new List<UserTransactions>();
            if(submit=="CashIn")
            {
                wallet.Saldo += walletStake;
                transaction.UserId = wallet.User.Id;
                transaction.Payment = stake;
                transaction.Transactions = "Uplata u iznosu od " + stake + " kn " + " " + DateTime.Now.ToString();
                listTransactions.Add(transaction);
                wallet.Transactions = listTransactions;
                TempData["msg"] = "The transaction is successful";
            }
            else
            {
                if ((wallet.Saldo -= walletStake) >= 0)
                {
                    transaction.UserId = wallet.User.Id;
                    transaction.Payment = stake;
                    transaction.Transactions = "Isplata u iznosu od " + stake + " kn " + " " + DateTime.Now.ToString();
                    listTransactions.Add(transaction);
                    wallet.Transactions = listTransactions;
                    TempData["msg"] = "The transaction is successful";
                }
                else
                {
                    TempData["msg"] = "You dont have enough funds for this transaction.";
                    return RedirectToAction("Index");
                }
            } 
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Transactions()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userTransaction = _context.UserTransactions.Where(x => x.UserId == userId).ToList();
            return View(userTransaction);
        }

        private bool WalletExists(string id)
        {
            return _context.Wallet.Any(e => e.User.Id == id);
        }
    }
}
