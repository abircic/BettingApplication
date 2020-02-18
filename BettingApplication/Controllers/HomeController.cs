using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BettingApplication.Models;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using BettingApplication.ViewModels;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly BettingApplicationContext _context;
        public string CurrentFilter { get; set; }
        public HomeController(BettingApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchStringLeague, string searchStringSport, string searchStringTeam)
        {
            ViewData["CurrentFilterSport"] = searchStringSport;
            if (!String.IsNullOrEmpty(searchStringSport))
            {

                var filterUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var filterMatches = _context.Matches.Include(c => c.Sport)
                    .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                    .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                    .Include(t => t.Types).Where(m => m.Sport.Name.ToUpper().Contains(searchStringSport.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.HomeTeam.League.Name,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Types._1,
                        _X = x.Types._X,
                        _2 = x.Types._2,
                        _1X = x.Types._1X,
                        _X2 = x.Types._X2,
                        _12 = x.Types._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == filterUserId).ToList();
                    filterModel.BetSlip = filterBetSlipList;
                    filterModel.Matches = filterMatchVmList;
                    return View(filterModel);
                }
               
            }
            ViewData["CurrentFilterLeague"] = searchStringLeague;
            if (!String.IsNullOrEmpty(searchStringLeague))
            {

                var filterUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var filterMatches = _context.Matches.Include(c => c.Sport)
                    .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                    .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                    .Include(t => t.Types).Where(m => m.HomeTeam.League.Name.ToUpper().Contains(searchStringLeague.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.HomeTeam.League.Name,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Types._1,
                        _X = x.Types._X,
                        _2 = x.Types._2,
                        _1X = x.Types._1X,
                        _X2 = x.Types._X2,
                        _12 = x.Types._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == filterUserId).ToList();
                    filterModel.BetSlip = filterBetSlipList;
                    filterModel.Matches = filterMatchVmList;
                    return View(filterModel);
                }
                
            }
            ViewData["CurrentFilterTeam"] = searchStringTeam;
            if (!String.IsNullOrEmpty(searchStringTeam))
            {

                var filterUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var filterMatches = _context.Matches.Include(c => c.Sport).
                    Include(h => h.HomeTeam).ThenInclude(l => l.League).
                    Include(a => a.AwayTeam).ThenInclude(l => l.League).
                    Include(t => t.Types).
                    Where(m => m.HomeTeam.Name.ToUpper().Contains(searchStringTeam.ToUpper()) || m.AwayTeam.Name.ToUpper().Contains(searchStringTeam.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.HomeTeam.League.Name,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Types._1,
                        _X = x.Types._X,
                        _2 = x.Types._2,
                        _1X = x.Types._1X,
                        _X2 = x.Types._X2,
                        _12 = x.Types._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == filterUserId).ToList();
                    filterModel.BetSlip = filterBetSlipList;
                    filterModel.Matches = filterMatchVmList;
                    return View(filterModel);
                }
                
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if(!User.IsInRole("Admin"))
                UserBetWin();
            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            //ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            @TempData["UsersForActivate"] = _context.Users.Where(u => u.EmailConfirmed == false).Count();
            if (User.Identity.IsAuthenticated)
            {
                var wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
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
            else
            {
                RedirectToAction("Index", "Matches");
            }
            int counter = 0;
            decimal totOdd = 1;
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Types).Where(s => s.Sport.Name.Contains("Football")).ToList();
            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Types._1,
                _X = x.Types._X,
                _2 = x.Types._2,
                _1X = x.Types._1X,
                _X2 = x.Types._X2,
                _12 = x.Types._12,
            }).OrderBy((o=>o.League)).ToList();
            foreach (BetSlip item in _context.BetSlip.Include(m=>m.Match).ThenInclude(h=>h.HomeTeam).Include(m => m.Match).ThenInclude(a=>a.AwayTeam).Where(b => b.User.Id == userId))
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
           
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            
            
            List<BetSlip> betSlipList = _context.BetSlip.Where(b => b.User.Id == userId).ToList();
            MatchesPartialView model = new MatchesPartialView();
            model.BetSlip = betSlipList;
            model.Matches = matchVmList;
            return View(model);
        }

        public IActionResult TwoPlayerIndex()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            int counter = 0;
            decimal totOdd = 1;
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Tenis")).ToList();
            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Types._1,
                _2 = x.Types._2
            }).ToList();

            foreach (BetSlip item in _context.BetSlip.Include(m=>m.Match).ThenInclude(h=>h.HomeTeam).Include(m=>m.Match).ThenInclude(a=>a.AwayTeam))
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            if (User.Identity.IsAuthenticated)
            {
                
                var wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
                if (wallet == null)
                {
                    TempData["Saldo"] = null;
                }
                else
                {
                    TempData["Saldo"] = wallet.Saldo + " kn";
                    TempData["Username"] = user.UserName;
                }
                    

            }
            List<BetSlip> betSlipList = _context.BetSlip.Where(b => b.User.Id == userId).ToList();
            TwoPlayersPartialView model = new TwoPlayersPartialView();
            model.BetSlip = betSlipList;
            model.TwoPlayerMatches = matchVmList;
            return View(model);

        }

        public IActionResult TopMatchesIndex()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            int counter = 0;
            decimal totOdd = 1;
            List<Matches> topMatches = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Football")).Where(t => t.TopMatch == true).ToList();
            List<Matches> topTwoPlayersMatches = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Tenis")).Where(t => t.TopMatch == true).ToList();
            List<TopMatchesViewModel> allMatches = new List<TopMatchesViewModel>();
            List<TopMatchesViewModel> matchVmList = topMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Types._1 + 0.10m,
                _X = x.Types._X + 0.10m,
                _2 = x.Types._2 + 0.10m,
                _1X = x.Types._1X + 0.10m,
                _X2 = x.Types._X2 + 0.10m,
                _12 = x.Types._12 + 0.10m
            }).ToList();

            List<TopMatchesViewModel> twoPlayersMatchVmList = topTwoPlayersMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Types._1 + 0.10m,
                _2 = x.Types._2 + 0.10m
            }).ToList();
            allMatches.AddRange(matchVmList);
            allMatches.AddRange(twoPlayersMatchVmList);
            foreach (BetSlip item in _context.BetSlip.Include(m => m.Match).ThenInclude(h => h.HomeTeam).Include(m => m.Match).ThenInclude(a => a.AwayTeam))
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            if (User.Identity.IsAuthenticated)
            {
                
                var wallet = _context.Wallet.Where(x => x.User == user).FirstOrDefault();
                if (wallet == null)
                {
                    TempData["Saldo"] = null;
                }
                else
                {
                    TempData["Saldo"] = wallet.Saldo + " kn";
                    TempData["Username"] = user.UserName;
                }
                    

            }
            List<BetSlip> betSlipList = _context.BetSlip.Where(b => b.User.Id == userId).ToList();
            TopMatchesPartialView model = new TopMatchesPartialView();
            model.BetSlip = betSlipList;
            model.TopMatches = allMatches;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: test/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches.Include(h=>h.HomeTeam).Include(a=>a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }
        [Authorize(Roles = "Admin")]
        // POST: test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var matches = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TopMatch(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            matches.TopMatch = true;
            if (matches == null)
            {
                return NotFound();
            }
            _context.Matches.Update(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTopMatch(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            matches.TopMatch = false;
            if (matches == null)
            {
                return NotFound();
            }
            _context.Matches.Update(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public void UserBetWin()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            foreach (var item in _context.UserBetMatches.Where(u => u.UserBets.User.Id == userId && u.Win=="Pending")
                .Include(m=>m.Match.HomeTeam)
                .Include(a=>a.Match.AwayTeam)
                .Include(u=>u.UserBets).ToList())
            {
                var match = _context.Results.Where(t => t.HomeTeam.Contains(item.Match.HomeTeam.Name) 
                && t.AwayTeam.Contains(item.Match.AwayTeam.Name) 
                && t.Time==item.Match.Time).FirstOrDefault();
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
                    item.Result = match.Result;
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
            var userBets = _context.UserBets.Where(t => t.Win == "Pending" && t.User==user).Include(x=>x.BetMatches).ToList();
            if (check != null)
            {
                if (userBets.Count > 0)
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
                                flag = false;
                                break;
                            }
                            if (match.Win == "Win")
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
                            transaction.Transactions = "Isplata dobitka u iznosu od " + item.CashOut + " kn " + " " + DateTime.Now;
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
