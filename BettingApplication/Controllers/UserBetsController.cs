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
        // GET: UserBet
        public async Task<IActionResult> Index()
        {
            await UserBetWin();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userBet = _context.UserBet.Where(x => x.User.Id == userId)
                .Include(t => t.BetMatches).ToList();

            if (User.Identity.IsAuthenticated)
            {
                var wallet = await _context.Wallet.Where(x => x.User == user).FirstOrDefaultAsync();
                
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

            return View(userBet.OrderByDescending(u => u.TimeStamp));
        }

        [HttpGet]
        public async Task<IActionResult> UserBet()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UserBet(string stake, string TotalOdd, string submit)
        {
            var topMatchValue = await GetTopMatchValue();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var wallet = await _context.Wallet.Where(x => x.User == user).FirstOrDefaultAsync();
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
            UserTransaction transaction = new UserTransaction();
            List<UserTransaction> listTransactions = new List<UserTransaction>();
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
                UserBet userBet = new UserBet();
                decimal totOdd = decimal.Parse(TotalOdd);
                userBet.TimeStamp = DateTime.Now;
                userBet.BetAmount = betStake;
                userBet.CashOut = (betStake / 100 * 95) * totOdd;
                userBet.TotalOdd = totOdd;
                userBet.Win = "Pending";
                userBet.User = user;
                List<UserBetMatch> listBetMatches = new List<UserBetMatch>();

                foreach (BetSlip item in _context.BetSlip.Include(m=>m.Match).ThenInclude(h=>h.HomeTeam).Include(m=>m.Match).ThenInclude(a=>a.AwayTeam).Where(b => b.User.Id == userId))
                {
                    UserBetMatch temp = new UserBetMatch();
                    //var football = _context.Match.Include(h=>h.HomeTeam).Include(a=>a.AwayTeam).SingleOrDefault(q => q.Id == item.MatchId);
                    temp.Match = item.Match;
                    temp.Odd = item.Odd;
                    temp.Type = item.Type;
                    temp.Win = "Pending";
                    listBetMatches.Add(temp);
                }
                userBet.BetMatches = listBetMatches;
                await _context.UserBet.AddAsync(userBet);
                await _context.UserBet.AddAsync(userBet);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["betmsg"] = "The transaction is not successful";
                return RedirectToAction("Index","Home");
            }
           
            return RedirectToAction("Index");
        }


        // GET: UserBet/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //.FirstOrDefaultAsync(m => m.Id == id);
            var userBets = await _context.UserBet.Include(b => b.BetMatches).ThenInclude(m=>m.Match).ThenInclude(h=>h.HomeTeam)
                .Include(m=>m.Match).Include(b => b.BetMatches).ThenInclude(m => m.Match).ThenInclude(a=>a.AwayTeam).FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }


        // GET: UserBet/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBets = await _context.UserBet.FirstOrDefaultAsync(m => m.Id == id);
            if (userBets == null)
            {
                return NotFound();
            }

            return View(userBets);
        }

        // POST: UserBet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBets = await _context.UserBet.FindAsync(id);
            _context.UserBet.Remove(userBets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<int> GetTopMatchValue()
        {
            var topMatch = await _context.AdminTopMatchConfig.FirstOrDefaultAsync();

            return topMatch.MinimumNumberOfMatches;
        }
        public async Task UserBetWin()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            foreach (var item in _context.UserBetMatch.Where(u => u.UserBet.User.Id == userId).Include(m => m.Match.HomeTeam).Include(a=>a.Match.AwayTeam).Include(u => u.UserBet).ToList())
            {
                var match = await _context.Result.Where(m=>m.Id==item.Match.Id).FirstOrDefaultAsync();
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
                    await _context.SaveChangesAsync();
                }
            }
            await CheckTicket(user);
        }

        public async Task CheckTicket(AppUser user)
        {
            bool flag = false;
            string pendingFlag = "";
            var check = await _context.UserBet.Where(x => x.User.Id == user.Id).FirstOrDefaultAsync();
            var userBets = _context.UserBet.Where(t => t.Win == "Pending" && t.User == user).Include(x => x.BetMatches).ToList();
            if (check != null)
                if (userBets.Count > 0)
                {
                    {
                        foreach (var item in userBets)
                        {
                            pendingFlag = null;
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
                                var wallet = await _context.Wallet.Where(u => u.User.Id == user.Id).FirstOrDefaultAsync();
                                wallet.Saldo += item.CashOut;
                                UserTransaction transaction = new UserTransaction();
                                transaction.UserId = wallet.User.Id;
                                transaction.Payment = item.CashOut.ToString();
                                transaction.Transactions = "Isplata dobitka u iznosu od " + item.CashOut + " kn " + " " + DateTime.Now.ToString();
                                _context.Update(wallet);
                                _context.Update(transaction);
                                await _context.SaveChangesAsync();

                            }
                        }
                    }
                }
            
                
        }
    }
}
