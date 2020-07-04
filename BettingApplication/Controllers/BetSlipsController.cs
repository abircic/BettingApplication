using System;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BettingApplication.Services.Interfaces;

namespace BettingApplication.Controllers
{
    [Authorize]
    public class BetSlipsController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly IAccountService _accountService;
        private readonly IWalletService _walletService;
        private readonly IBetSlipService _betSlipService;
        private readonly IMatchService _matchService;
        
        public BetSlipsController(BettingApplicationContext context, IAccountService accountService,
            IWalletService walletService, IBetSlipService betSlipService, IMatchService matchService)
        {
            _context = context;
            _walletService = walletService;
            _accountService = accountService;
            _betSlipService = betSlipService;
            _matchService = matchService;
        }
        // GET: BetSlips
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var wallet = await _walletService.GetWallet(userId);
            int counter = 0;
            decimal totOdd = 1;
            var response = await _betSlipService.GetUserBetSlip(userId);
            foreach (var item in response)
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }

            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            TempData["Tax"] = "kn";
            TempData["Saldo"] = wallet.Saldo+" kn";
            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> Bet()
        {
            return View(await _context.BetSlip.ToListAsync());//TODO
        }
        [HttpPost]
        public async Task<IActionResult> Bet(string matchId, string type, bool top)
        {
            int topMatchValue = await _matchService.GetTopMatchValue();
            var match = await _matchService.GetMatch(matchId);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserById(userId);
            decimal betValue = 0;
            //var football = _context.Match.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Type).SingleOrDefault(q => q.Id == matchId);
             if(match != null)
            {
                switch (type)
                {
                    case "1":
                        betValue = match.Type._1;
                        break;
                    case "X":
                        betValue = match.Type._X;
                        break;
                    case "2":
                        betValue = match.Type._2;
                        break;
                    case "1X":
                        betValue = match.Type._1X;
                        break;
                    case "X2":
                        betValue = match.Type._X2;
                        break;
                    case "12":
                        betValue = match.Type._12;
                        break;
                }
                
            }
            else 
            {
                var other = await _matchService.GetMatch(matchId);
                switch (type)
                {
                    case "1":
                        betValue = other.Type._1;
                        break;
                    case "2":
                        betValue = other.Type._2;
                        break;
                }
            }
            int counter = 0;
            int counterOdd = 0;
            bool existMatch = false;
            BetSlip temp = new BetSlip();
            BetSlip matches = null;
            bool tempTopStatus = false;
            foreach (var item in await _betSlipService.GetUserBetSlip(userId))
            {
               if(item.Match == match)
               {
                    existMatch = true;
                    matches = item;
               }
               if(item.TopMatch && item.Match != match && top)
               {
                    TempData["betmsg"] = "Top match is on ticket already";
                    return RedirectToAction("TopMatchesIndex", "Home");
               }
               if (item.Odd > 1.10m)
               {
                    counterOdd++;
               }
               counter++;
            }
            if(counterOdd < topMatchValue+1 && counter < topMatchValue+1 && top && existMatch)
            {
                TempData["betmsg"] = "You already have that match on ticket";
                return RedirectToAction("TopMatchesIndex", "Home");
            }
            if(counterOdd < topMatchValue && counter < topMatchValue && top)
            {
                TempData["betmsg"] = $"You need to have at least {topMatchValue} pairs on ticket";
                return RedirectToAction("Index", "Home");
            }
            if(counterOdd >= topMatchValue && counter >= topMatchValue && top == true)
            {
                tempTopStatus = true;
            }
            if (matches == null)
            {
                if(top && (tempTopStatus) || !top)
                {
                    temp.Match = match;
                    
                    if (match != null)
                    {
                        temp.Match = match;
                        temp.TopMatch = top;
                        if (top)
                        {
                            temp.Odd = betValue+0.10m;
                        }
                        else
                        {
                            temp.Odd = betValue;
                        }
                    }
                    else
                    {
                        var other = await _matchService.GetMatch(matchId);
                        temp.Match = other;
                        temp.TopMatch = top;
                        temp.Odd = betValue+0.10m;
                    }
                    temp.Type = type;
                    decimal totOdd = 1;
                    temp.User = user;
                    _context.BetSlip.Add(temp);
                    await _context.SaveChangesAsync();
                    foreach (var item in await _betSlipService.GetUserBetSlip(userId))
                    {
                        totOdd = totOdd * item.Odd;
                    }
                    TempData["Odd"] = totOdd;
                    _context.BetSlip.Update(temp);
                    await _context.SaveChangesAsync();
                }
               
            }
            else if(top && counter >= topMatchValue+1)
            {

                matches.Match = match;
                matches.Type = type;
                matches.Odd = betValue+0.10m;
                matches.TopMatch = top;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (var item in await _betSlipService.GetUserBetSlip(userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            else if (!top)
            {
                matches.Match = match;

                matches.Type = type;
                matches.Odd = betValue;
                matches.TopMatch = false;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (var item in await _betSlipService.GetUserBetSlip(userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["betmsg"] = $"You need to have at least {topMatchValue} pairs on ticket";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index","Home");
        }
        
            // GET: BetSlips/Details/5
            public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _betSlipService.Details(id);

            if (betSlip == null)
            {
                return NotFound();
            }

            return View(betSlip);
        }


        [HttpPost]
        public async Task<IActionResult> BetTwoPlayer(string matchId, string type, bool top)
        {
            var match = await _matchService.GetMatch(matchId);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserById(userId);
            decimal betValue = 0;
            BetSlip temp = new BetSlip();
            BetSlip betSlipMatch = null;
            //var other = _context.Match.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t=>t.Type).SingleOrDefault(q => q.Id == matchId);
            switch (type)
            {
                case "1":
                    betValue = match.Type._1;
                    break;
                case "2":
                    betValue = match.Type._2;
                    break;
            }
            foreach (var item in await _betSlipService.GetUserBetSlip(userId))
            {
                if (item.Match == match)
                {
                    betSlipMatch = item;
                }
            }
            if(betSlipMatch == null)
            {
                temp.Match = match;
                temp.TopMatch = top;
                temp.Odd = betValue;
                temp.Type = type;
                temp.User = user;
                _context.BetSlip.Add(temp);
                decimal totOdd = 1;
                await _context.SaveChangesAsync();
                foreach (var item in await _betSlipService.GetUserBetSlip(userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                await _context.SaveChangesAsync();
            }
            else
            {
                betSlipMatch.Match = match;
                betSlipMatch.Type = type;
                betSlipMatch.Odd = betValue;
                betSlipMatch.TopMatch = top;
                _context.BetSlip.Update(betSlipMatch);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (var item in await _betSlipService.GetUserBetSlip(userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(betSlipMatch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("TwoPlayerIndex", "Home");
        }
       
   
        // GET: BetSlips/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _betSlipService.GetMatchFromBetSlip(id);
            if (betSlip == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: BetSlips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _betSlipService.DeleteMatchFromBetSlip(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
