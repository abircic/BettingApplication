using System;
using System.Linq;
using System.Threading.Tasks;
using Aplikacija_za_kladenje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Aplikacija_za_kladenje.Controllers
{
    [Authorize]
    public class BetSlipsController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        
        public BetSlipsController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: BetSlips
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var wallet = _context.Wallet.Where(x => x.Userid == userId).FirstOrDefault();
            int counter = 0;
            decimal totOdd = 1;
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id==userId))
            {
                totOdd = totOdd * item.Odd;
                counter++;
            }

            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = "kn";
            TempData["Tax"] = "kn";
            TempData["Saldo"] = wallet.Saldo+" kn";
            return View(await _context.BetSlip.Where(x => x.User.Id == userId).ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> Bet()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Bet(string matchId, string type, bool top)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            decimal betValue = 0;
            var football = _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).SingleOrDefault(q => q.Id == matchId);
             if(football != null)
            {
                switch (type)
                {
                    case "1":
                        betValue = football.Types._1;
                        break;
                    case "X":
                        betValue = football.Types._X;
                        break;
                    case "2":
                        betValue = football.Types._2;
                        break;
                    case "1X":
                        betValue = football.Types._1X;
                        break;
                    case "X2":
                        betValue = football.Types._X2;
                        break;
                    case "12":
                        betValue = football.Types._12;
                        break;
                }
                
            }
             else
            {
                var other = _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).SingleOrDefault(q => q.Id == matchId);
                switch (type)
                {
                    case "1":
                        betValue = other.Types._1;
                        break;
                    case "2":
                        betValue = other.Types._2;
                        break;
                }
            }
            int counter = 0;
            int counterOdd = 0;
            bool existMatch = false;
            BetSlip temp = new BetSlip();
            BetSlip matches = null;
            bool tempTopStatus = false;
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
            {
               if(item.MatchId == matchId)
                {
                    existMatch = true;
                    matches = item;
                }
               if(item.TopMatch && item.MatchId != matchId && top)
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
            if(counterOdd < 6 && counter < 6 && top && existMatch)
            {
                TempData["betmsg"] = "You already have that match on ticket";
                return RedirectToAction("TopMatchesIndex", "Home");
            }
            else if(counterOdd < 5 && counter < 5&& top)
            {
                TempData["betmsg"] = "You need to have at least 5 pairs on ticket";
                return RedirectToAction("Index", "Home");
            }
            else if(counterOdd >= 5 && counter >= 5 && top == true)
            {
                tempTopStatus = true;
            }
            else
            {
                tempTopStatus = false;
            }
            if (matches == null)
            {
                if(((top) && (tempTopStatus)) || (!top))
                {
                    temp.MatchId = matchId;
                    
                    if (football != null)
                    {
                        temp.HomeTeam = football.HomeTeam.Name;
                        temp.AwayTeam = football.AwayTeam.Name;
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
                        var other = _context.Matches.Include(f => f.HomeTeam).Include(s => s.AwayTeam).Include(t => t.Types).SingleOrDefault(q => q.Id == matchId);
                        temp.HomeTeam = other.HomeTeam.Name;
                        temp.AwayTeam = other.AwayTeam.Name;
                        temp.TopMatch = top;
                        temp.Odd = betValue+0.10m;
                    }
                    temp.Type = type;
                    decimal totOdd = 1;
                    temp.User = user;
                    _context.BetSlip.Add(temp);
                    await _context.SaveChangesAsync();
                    foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                    {
                        totOdd = totOdd * item.Odd;
                    }
                    TempData["Odd"] = totOdd;
                    _context.BetSlip.Update(temp);
                    await _context.SaveChangesAsync();
                }
               
            }
            else if(top && counter >= 6)
            {

                matches.MatchId = matchId;
                matches.Type = type;
                matches.Odd = betValue+0.10m;
                matches.TopMatch = top;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            else if (!top)
            {
                matches.MatchId = matchId;

                matches.Type = type;
                matches.Odd = betValue;
                matches.TopMatch = false;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["betmsg"] = "You need to have at least 5 pairs on ticket";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index","Home");
        }
        
            // GET: BetSlips/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betSlip == null)
            {
                return NotFound();
            }

            return View(betSlip);
        }


        [HttpPost]
        public async Task<IActionResult> BetTwoPlayer(string matchId, string type, Boolean top)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            decimal betValue = 0;
            BetSlip temp = new BetSlip();
            BetSlip match = null;
            var other = _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t=>t.Types).SingleOrDefault(q => q.Id == matchId);
            switch (type)
            {
                case "1":
                    betValue = other.Types._1;
                    break;
                case "2":
                    betValue = other.Types._2;
                    break;
            }
            foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
            {
                if (item.MatchId == matchId)
                {
                    match= item;
                }
            }
            if(match == null)
            {
                temp.MatchId = matchId;
                temp.HomeTeam = other.HomeTeam.Name;
                temp.AwayTeam = other.AwayTeam.Name;
                temp.TopMatch = top;
                temp.Odd = betValue;
                temp.Type = type;
                temp.User = user;
                _context.BetSlip.Add(temp);
                decimal totOdd = 1;
                await _context.SaveChangesAsync();
                foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                await _context.SaveChangesAsync();
            }
            else
            {
                match.MatchId = matchId;
                match.Type = type;
                match.Odd = betValue;
                match.TopMatch = top;
                _context.BetSlip.Update(match);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (BetSlip item in _context.BetSlip.Where(b => b.User.Id == userId))
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(match);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("TwoPlayerIndex", "Home");
        }
       
   
        // GET: BetSlips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlip
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betSlip == null)
            {
                return NotFound();
            }

            return View(betSlip);
        }

        // POST: BetSlips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var betSlip = await _context.BetSlip.FindAsync(id);
            _context.BetSlip.Remove(betSlip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool BetSlipExists(int id)
        {
            return _context.BetSlip.Any(e => e.Id == id);
        }
    }
}
