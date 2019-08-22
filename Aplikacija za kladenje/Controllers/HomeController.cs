using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Aplikacija_za_kladenje.Models;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Data;


namespace Aplikacija_za_kladenje.Controllers
{
    public class HomeController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;
        public HomeController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
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
            foreach (BetSlip item in _context.BetSlip)
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            var wallet = _context.Wallet.FirstOrDefault();
            TempData["Saldo"] = wallet.Saldo + " kn";
            List<BetSlip> betSlipList = _context.BetSlip.ToList();
            MatchesPartialView model = new MatchesPartialView();
            model.BetSlip = betSlipList;
            model.Matches = matchVmList;
            return View(model);
        }

        public IActionResult TwoPlayerIndex()
        {
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
            var wallet = _context.Wallet.FirstOrDefault();
            TempData["Saldo"] = wallet.Saldo + " kn";
            List<BetSlip> betSlipList = _context.BetSlip.ToList();
            TwoPlayersPartialView model = new TwoPlayersPartialView();
            model.BetSlip = betSlipList;
            model.TwoPlayerMatches = matchVmList;
            return View(model);

        }

        public IActionResult TopMatchesIndex()
        {
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
            foreach (BetSlip item in _context.BetSlip)
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            var wallet = _context.Wallet.FirstOrDefault();
            TempData["Saldo"] = wallet.Saldo + " kn";
            List<BetSlip> betSlipList = _context.BetSlip.ToList();
            TopMatchesPartialView model = new TopMatchesPartialView();
            model.BetSlip = betSlipList;
            model.TopMatches = allMatches;
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
