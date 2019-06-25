using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;

namespace Aplikacija_za_kladenje.Controllers
{
    public class UserBetsController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        public UserBetsController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: UserBets
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserBets.Include(b=>b.Matches).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> UserBet()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UserBet(decimal stake, string TotalOdd, string submit)
        {
           if(submit=="Remove")
            {
                foreach (var item in _context.BetSlip)
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","BetSlips");
            }

            Boolean temp_top_status = false;
            int counter = 0;
            int counter_odd = 0;
            foreach (BetSlip item in _context.BetSlip)
            {
                if (item.TopMatch == true)
                {
                    temp_top_status = true;
                }
                if (item.Odd > 1.10m)
                {
                    counter_odd++;
                }
                counter++;
            }
            if (counter_odd <5)
            {
                temp_top_status = false;
            }
            Wallet wallet = _context.Wallet.FirstOrDefault();
            UserTransactions transaction = new UserTransactions();
            List<UserTransactions> listTransactions = new List<UserTransactions>();
            if (((wallet.Saldo -= stake) > 0) && (counter>5&&temp_top_status==true)||temp_top_status==false)
            {
                wallet.Saldo -= stake;
                transaction.UserID = wallet.Userid;
                transaction.Payment = stake.ToString();
                transaction.Transactions = "Uplata listica u iznosu od " + stake.ToString() + " kn " + " " + DateTime.Now.ToString();
                listTransactions.Add(transaction);
                wallet.Transactions = listTransactions;
                await _context.SaveChangesAsync();
                UserBets UserBet = new UserBets();
                decimal TotOdd = decimal.Parse(TotalOdd);
                UserBet.TimeStamp = DateTime.Now;
                UserBet.BetAmount = stake;
                UserBet.CashOut = (stake / 100 * 95) * TotOdd;
                UserBet.TotalOdd = TotOdd;
                List<UserBetMatches> listBetMatches = new List<UserBetMatches>();

                foreach (var item in _context.BetSlip)
                {
                    UserBetMatches temp = new UserBetMatches();
                    temp.MatchId = item.MatchId;
                    temp.HomeTeam = item.HomeTeam;
                    temp.AwayTeam = item.AwayTeam;
                    temp.Odd = item.Odd;
                    temp.Type = item.Type;
                    temp.TopMatch = item.TopMatch;
                    listBetMatches.Add(temp);
                }
                UserBet.Matches = listBetMatches;
                _context.UserBets.Add(UserBet);
                await _context.UserBets.AddAsync(UserBet);
                _context.SaveChanges();
            }
           
            return RedirectToAction("Index");
        }









        // GET: UserBets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UserBets.Include(b => b.Matches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }


        // GET: UserBets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UserBets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }

        // POST: UserBets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBets = await _context.UserBets.FindAsync(id);
            _context.UserBets.Remove(userBets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBetsExists(int id)
        {
            return _context.UserBets.Any(e => e.Id == id);
        }
    }
}
