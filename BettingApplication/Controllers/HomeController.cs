﻿using System;
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
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly IAccountService _accountService;
        public HomeController(BettingApplicationContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index(string searchStringLeague, string searchStringSport, string searchStringTeam, string sortMatches)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserById(userId);
            if (!String.IsNullOrEmpty(sortMatches))
            {
                var sortedMatches = _context.Match.Include(c => c.Sport)
                    .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                    .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                    .Include(t => t.Type).ToList();
                if (sortedMatches.Count > 0)
                {
                    var sortedMatchVmList = sortedMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.Competition,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Type._1,
                        _X = x.Type._X,
                        _2 = x.Type._2,
                        _1X = x.Type._1X,
                        _X2 = x.Type._X2,
                        _12 = x.Type._12,
                    }).OrderBy((o => o.Time)).ToList();
                    MatchesPartialView sortedModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == userId).ToList();
                    sortedModel.BetSlip = filterBetSlipList;
                    sortedModel.Matches = sortedMatchVmList;
                    return View(sortedModel);
                }
            }
            ViewData["CurrentFilterSport"] = searchStringSport;
            if (!String.IsNullOrEmpty(searchStringSport))
            {
                var filterMatches = _context.Match.Include(c => c.Sport)
                    .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                    .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                    .Include(t => t.Type).Where(m => m.Sport.Name.ToUpper().Contains(searchStringSport.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.Competition,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Type._1,
                        _X = x.Type._X,
                        _2 = x.Type._2,
                        _1X = x.Type._1X,
                        _X2 = x.Type._X2,
                        _12 = x.Type._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == userId)
                        .Include(m => m.Match).ThenInclude(h => h.HomeTeam)
                        .Include(m => m.Match).ThenInclude(a => a.AwayTeam).ToList();
                    filterModel.BetSlip = filterBetSlipList;
                    filterModel.Matches = filterMatchVmList;
                    return View(filterModel);
                }
               
            }
            ViewData["CurrentFilterLeague"] = searchStringLeague;
            if (!String.IsNullOrEmpty(searchStringLeague))
            {
                var filterMatches = _context.Match.Include(c => c.Sport)
                    .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                    .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                    .Include(t => t.Type).Where(m => m.Competition.ToUpper().Contains(searchStringLeague.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.Competition,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Type._1,
                        _X = x.Type._X,
                        _2 = x.Type._2,
                        _1X = x.Type._1X,
                        _X2 = x.Type._X2,
                        _12 = x.Type._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == userId)
                        .Include(m => m.Match).ThenInclude(h => h.HomeTeam)
                        .Include(m => m.Match).ThenInclude(a => a.AwayTeam).ToList();
                    filterModel.BetSlip = filterBetSlipList;
                    filterModel.Matches = filterMatchVmList;
                    return View(filterModel);
                }
                
            }
            ViewData["CurrentFilterTeam"] = searchStringTeam;
            if (!String.IsNullOrEmpty(searchStringTeam))
            {

                var filterMatches = _context.Match.Include(c => c.Sport).
                    Include(h => h.HomeTeam).ThenInclude(l => l.League).
                    Include(a => a.AwayTeam).ThenInclude(l => l.League).
                    Include(t => t.Type).
                    Where(m => m.HomeTeam.Name.ToUpper().Contains(searchStringTeam.ToUpper()) || m.AwayTeam.Name.ToUpper().Contains(searchStringTeam.ToUpper())).ToList();
                if (filterMatches.Count > 0)
                {
                    var filterMatchVmList = filterMatches.Select(x => new MatchViewModel
                    {
                        Id = x.Id,
                        League = x.Competition,
                        HomeTeamName = x.HomeTeam.Name,
                        AwayTeamName = x.AwayTeam.Name,
                        Time = x.Time,
                        _1 = x.Type._1,
                        _X = x.Type._X,
                        _2 = x.Type._2,
                        _1X = x.Type._1X,
                        _X2 = x.Type._X2,
                        _12 = x.Type._12,
                    }).OrderBy((o => o.League)).ToList();
                    MatchesPartialView filterModel = new MatchesPartialView();
                    List<BetSlip> filterBetSlipList = _context.BetSlip.Where(b => b.User.Id == userId)
                        .Include(m=>m.Match).ThenInclude(h=>h.HomeTeam)
                        .Include(m => m.Match).ThenInclude(a=>a.AwayTeam).ToList();
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
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type).Where(s => s.Sport.Name.Contains("Football") && s.Hide == false).ToList();
            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.Competition,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1,
                _X = x.Type._X,
                _2 = x.Type._2,
                _1X = x.Type._1X,
                _X2 = x.Type._X2,
                _12 = x.Type._12,
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
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .Include(t => t.Type).Where(s => s.Sport.Name.Contains("Tenis") && s.Hide == false).ToList();
            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1,
                _2 = x.Type._2
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
            List<Match> topMatches = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type).Where(s => s.Sport.Name.Contains("Football") && s.Hide == false).Where(t => t.TopMatch == true).ToList();
            List<Match> topTwoPlayersMatches = _context.Match.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Type).Where(s => s.Sport.Name.Contains("Tenis")).Where(t => t.TopMatch == true).ToList();
            List<TopMatchesViewModel> allMatches = new List<TopMatchesViewModel>();
            List<TopMatchesViewModel> matchVmList = topMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1 + 0.10m,
                _X = x.Type._X + 0.10m,
                _2 = x.Type._2 + 0.10m,
                _1X = x.Type._1X + 0.10m,
                _X2 = x.Type._X2 + 0.10m,
                _12 = x.Type._12 + 0.10m
            }).ToList();

            List<TopMatchesViewModel> twoPlayersMatchVmList = topTwoPlayersMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1 + 0.10m,
                _2 = x.Type._2 + 0.10m
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

            var matches = await _context.Match
                .Include(h=>h.HomeTeam)
                .Include(a=>a.AwayTeam)
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
            var matches = await _context.Match.FindAsync(id);
            matches.Hide = true;
            _context.Match.Update(matches);
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

            var matches = await _context.Match.Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            matches.TopMatch = true;
            if (matches == null)
            {
                return NotFound();
            }
            _context.Match.Update(matches);
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

            var matches = await _context.Match.Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            matches.TopMatch = false;
            if (matches == null)
            {
                return NotFound();
            }
            _context.Match.Update(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public void UserBetWin()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            foreach (var item in _context.UserBetMatch.Where(u => u.UserBet.User.Id == userId && u.Win=="Pending")
                .Include(m=>m.Match.HomeTeam)
                .Include(a=>a.Match.AwayTeam)
                .Include(u=>u.UserBet).ToList())
            {
                var match = _context.Result.Where(m=>m.Id==item.Match.Id).FirstOrDefault();
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
            var check = _context.UserBet.Where(x => x.User.Id == user.Id).FirstOrDefault();
            var userBets = _context.UserBet.Where(t => t.Win == "Pending" && t.User==user).Include(x=>x.BetMatches).ToList();
            if (check != null)
            {
                if (userBets.Count > 0)
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
                            UserTransaction transaction = new UserTransaction();
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
