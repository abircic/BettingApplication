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
using System.Security.Claims;

namespace BettingApplication.Controllers
{
    [Authorize]
    public class UserBetsController : Controller
    {
        private readonly BettingApplicationContext _context;

        public UserBetsController(BettingApplicationContext context)
        {
            _context = context;
        }
        // GET: UserBets
        public async Task<IActionResult> Index()
        {
            UserBetWin();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userBet = _context.UserBets.Where(x => x.User.Id == userId).Include(t => t.BetMatches).ToList();
            var wallet = new Wallet();
            if (User.Identity.IsAuthenticated)
            {
                lock (wallet)
                {
                    wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
                }
                
                if (wallet == null)
                {
                    TempData["Username"] = null;
                    TempData["Saldo"] = null;
                }
                else
                {
                    TempData["Username"] = user.UserName;
                    TempData["Saldo"] = wallet.Saldo + " kn";
                }
            }
            return View(userBet);
        }

        [HttpGet]
        public async Task<IActionResult> UserBet()
        {
            
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UserBet(string stake, string TotalOdd, string submit)
        {
            var topMatchValue = GetTopMatchValue();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
            TempData["betmsg"] = null;
            if (stake == null && submit!="Remove")
            {
                TempData["betmsg"] = "Enter the amount";
                return RedirectToAction("Index", "Home");
            }
            if (submit == "Remove")
            {
                foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            var betStake = decimal.Parse(stake, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            bool tempTopStatus = false;
            int counter = 0;
            int counterOdd = 0;
            foreach (BetSlip item in _context.BetSlip.Include(m=>m.Match)
                .Include(h=>h.Match.HomeTeam)
                .Include(a=>a.Match.AwayTeam)
                .Where(b => b.User.Id == userId))
            {
                if (item.Match.Time < DateTime.Now)
                {
                    TempData["betmsg"] = $"{item.Match.HomeTeam.Name} - {item.Match.AwayTeam.Name} is started already";
                    return RedirectToAction("Index", "Home");
                }
                if (item.TopMatch == true)
                {
                    tempTopStatus = true;
                }
                if (item.Odd > 1.10m)
                {
                    counterOdd++;
                }
                counter++;
            }
            if(counter == 0)
            {
                TempData["betmsg"] = "0 pair on ticket";
                return RedirectToAction("Index", "Home");
            }
            if ((counterOdd <= topMatchValue || counter <= topMatchValue) && tempTopStatus == true)
            {
                TempData["betmsg"] = "The transaction is not successful";
                return RedirectToAction("Index", "Home");
            }
            UserTransactions transaction = new UserTransactions();
            List<UserTransactions> listTransactions = new List<UserTransactions>();
            if (((wallet.Saldo - betStake) >= 0 && betStake >= 1))
            {
                wallet.Saldo -= betStake;
                transaction.UserId = wallet.User.Id;
                transaction.Payment = stake;
                transaction.Transactions = "Uplata listica u iznosu od " + stake + " kn " + " " + DateTime.Now.ToString();
                TempData["betmsg"]= "The transaction is successful";
                listTransactions.Add(transaction);
                wallet.Transactions = listTransactions;
                await _context.SaveChangesAsync();
                UserBets userBet = new UserBets();
                decimal totOdd = decimal.Parse(TotalOdd);
                userBet.TimeStamp = DateTime.Now;
                userBet.BetAmount = betStake;
                userBet.CashOut = (betStake / 100 * 95) * totOdd;
                userBet.TotalOdd = totOdd;
                userBet.Win = "Pending";
                userBet.User = user;
                List<UserBetMatches> listBetMatches = new List<UserBetMatches>();

                foreach (BetSlip item in _context.BetSlip.Include(m=>m.Match).ThenInclude(h=>h.HomeTeam).Include(m=>m.Match).ThenInclude(a=>a.AwayTeam).Where(b => b.User.Id == userId))
                {
                    UserBetMatches temp = new UserBetMatches();
                    //var football = _context.Matches.Include(h=>h.HomeTeam).Include(a=>a.AwayTeam).SingleOrDefault(q => q.Id == item.MatchId);
                    temp.Match = item.Match;
                    temp.Odd = item.Odd;
                    temp.Type = item.Type;
                    temp.Win = "Pending";
                    listBetMatches.Add(temp);
                }
                userBet.BetMatches = listBetMatches;
                _context.UserBets.Add(userBet);
                await _context.UserBets.AddAsync(userBet);
                _context.SaveChanges();
            }
            else
            {
                TempData["betmsg"] = "The transaction is not successful";
                return RedirectToAction("Index","Home");
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
            //.FirstOrDefaultAsync(m => m.Id == id);
            var userBets = await _context.UserBets.Include(b => b.BetMatches).ThenInclude(m=>m.Match).ThenInclude(h=>h.HomeTeam)
                .Include(m=>m.Match).Include(b => b.BetMatches).ThenInclude(m => m.Match).ThenInclude(a=>a.AwayTeam).FirstOrDefaultAsync(m => m.Id == id);
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
        public int GetTopMatchValue()
        {
            var topMatch = _context.AdminTopMatchConfigs.FirstOrDefault();

            return topMatch.MinimumNumberOfMatches;
        }
        public void UserBetWin()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            foreach (var item in _context.UserBetMatches.Where(u => u.UserBets.User.Id == userId).Include(m => m.Match.HomeTeam).Include(a=>a.Match.AwayTeam).Include(u => u.UserBets).ToList())
            {
                var match = _context.Results.Where(t => t.HomeTeam.Contains(item.Match.HomeTeam.Name) 
                && t.AwayTeam.Contains(item.Match.AwayTeam.Name)
                && t.Time == item.Match.Time).FirstOrDefault();
                if (match != null)
                {
                    var winningTypes = match.WinningTypes.Split(';');
                    foreach (var type in winningTypes)
                    {
                        if (item.Type == type)
                            item.Win = "Win";
                    }
                    if (item.Win == "Pending")
                        item.Win = "Lose";
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }
            CheckTicket(user);
        }

        public void CheckTicket(AppUser user)
        {

            bool flag = false;
            string pendingFlag = "";
            var check = _context.UserBets.Where(x => x.User.Id == user.Id).FirstOrDefault();
            var userBets = _context.UserBets.Where(t => t.Win == "Pending" && t.User == user).Include(x => x.BetMatches).ToList();
            if (check != null)
                if (userBets.Count > 0)
                {
                    {
                        foreach (var item in userBets)
                        {
                            foreach (var match in item.BetMatches)
                            {
                                if (match.Win == "Lose")
                                {
                                    item.Win = "Lose";
                                    _context.Update(item);
                                    _context.SaveChanges();
                                    break;
                                }
                                else if (match.Win == "Win")
                                    flag = true;
                                else if (match.Win == "Pending")
                                    pendingFlag = "Pending";
                            }

                            if (flag == true && String.IsNullOrEmpty(pendingFlag))
                            {
                                item.Win = "Win";
                                _context.Update(item);
                                var wallet = _context.Wallet.Where(u => u.User.Id == user.Id).FirstOrDefault();
                                wallet.Saldo += item.CashOut;
                                UserTransactions transaction = new UserTransactions();
                                transaction.UserId = wallet.User.Id;
                                transaction.Payment = item.CashOut.ToString();
                                transaction.Transactions = "Isplata dobitka u iznosu od " + item.CashOut + " kn " + " " + DateTime.Now.ToString();
                                _context.Update(wallet);
                                _context.Update(transaction);
                                _context.SaveChanges();

                            }
                        }
                    }
                }
            
                
        }
    }
}
