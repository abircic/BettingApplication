using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;
using System.Globalization;
using Aplikacija_za_kladenje.Data;

namespace Aplikacija_za_kladenje.Controllers
{
    public class WalletsController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        public WalletsController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wallet.Include(t=>t.Transactions).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Payment(string submit, string stake)
        {
            TempData["msg"] = null;
            var walletStake = decimal.Parse(stake, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            if(walletStake < 10)
            {
                TempData["msg"] = "Minimum is 10 kn";
                return RedirectToAction("Index");
            }
            Wallet wallet = _context.Wallet.FirstOrDefault();
            UserTransactions transaction = new UserTransactions();
            List<UserTransactions> listTransactions = new List<UserTransactions>();
            if(submit=="CashIn")
            {
                wallet.Saldo += walletStake;
                transaction.UserId = wallet.Userid;
                transaction.Payment = stake;
                transaction.Transactions = "Uplata u iznosu od " + stake.ToString() + " kn " + " " + DateTime.Now.ToString();
                listTransactions.Add(transaction);
                wallet.Transactions = listTransactions;
                TempData["msg"] = "The transaction is successful";
            }
            else
            {
                if ((wallet.Saldo -= walletStake) >= 0)
                {
                    transaction.UserId = wallet.Userid;
                    transaction.Payment = stake;
                    transaction.Transactions = "Isplata u iznosu od " + stake.ToString() + " kn " + " " + DateTime.Now.ToString();
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
        public async Task<IActionResult> Transactions()
        {
            return View(await _context.UserTransactions.ToListAsync());
        }

        private bool WalletExists(int id)
        {
            return _context.Wallet.Any(e => e.Userid == id.ToString());
        }
    }
}
