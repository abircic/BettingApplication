using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Aplikacija_za_kladenje.Models;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;
        public HomeController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //ViewBag.UserId = HttpContext.Session.GetString("UserId");
            //ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (User.Identity.IsAuthenticated)
            {
                var wallet = _context.Wallet.Where(x => x.Userid == userId).FirstOrDefault();
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
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Football")).ToList();
            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Types._1,
                _X = x.Types._X,
                _2 = x.Types._2,
                _1X = x.Types._1X,
                _X2 = x.Types._X2,
                _12 = x.Types._12,
            }).OrderBy((o=>o.League)).ToList();
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
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
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Tennis")).ToList();
            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                _1 = x.Types._1,
                _2 = x.Types._2
            }).ToList();

            foreach (BetSlip item in _context.BetSlip)
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            if (User.Identity.IsAuthenticated)
            {
                
                var wallet = _context.Wallet.Where(x => x.Userid == userId).FirstOrDefault();
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
            List<Matches> topTwoPlayersMatches = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Tennis")).Where(t => t.TopMatch == true).ToList();
            List<TopMatchesViewModel> allMatches = new List<TopMatchesViewModel>();
            List<TopMatchesViewModel> matchVmList = topMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
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
                _1 = x.Types._1 + 0.10m,
                _2 = x.Types._2 + 0.10m
            }).ToList();
            allMatches.AddRange(matchVmList);
            allMatches.AddRange(twoPlayersMatchVmList);
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            if (User.Identity.IsAuthenticated)
            {
                
                var wallet = _context.Wallet.Where(x => x.Userid == userId).FirstOrDefault();
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
        
    }
}
