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
    public class BetSlipsController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        
        public BetSlipsController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: BetSlips
        public async Task<IActionResult> Index(string stake)
        {
            int counter = 0;
            decimal totOdd = 1;
            foreach (BetSlip item in _context.BetSlip)
            {
                totOdd = totOdd * item.Odd;
                counter++;

            }
            TempData["Odd"] = totOdd.ToString("0.00");
            TempData["NumberOfMatches"] = counter;
            TempData["CashOut"] = stake+"kn";

            return View(await _context.BetSlip.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> Bet()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Bet(string MatchId, decimal value, Boolean top)
        {
            int counter = 0;
            int counter_odd = 0;
            BetSlip temp = new BetSlip();
            BetSlip matches = null;
            Boolean temp_top_status = false;
            foreach (BetSlip item in _context.BetSlip)
            {
               if(item.MatchId==MatchId)
                {
                    matches = item;
                }
               if(item.TopMatch==true & item.MatchId!=MatchId)
                {
                    temp_top_status = true;
                }
                if (item.Odd > 1.10m)
                {
                    counter_odd++;
                }
                counter++;
            }
            if(counter_odd<4)
            {
                temp_top_status = true;
            }
            if (matches == null)
            {
                if(((top==true)&&(temp_top_status==false)&&(counter>4))||(top==false))
                {
                    temp.MatchId = MatchId;
                    var football = _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam).SingleOrDefault(q => q.Id == MatchId);
                    if (football != null)
                    {
                        temp.HomeTeam = football.HomeTeam.Name;
                        temp.AwayTeam = football.AwayTeam.Name;
                        temp.TopMatch = top;
                    }
                    else
                    {
                        var other = _context.TwoPlayersMatches.Include(f => f.First).Include(s => s.Second).SingleOrDefault(q => q.Id == MatchId);
                        temp.HomeTeam = other.First.Name;
                        temp.AwayTeam = other.Second.Name;
                        temp.TopMatch = top;
                    }
                    temp.Odd = value;
                    decimal totOdd = 1;
                    _context.BetSlip.Add(temp);
                    await _context.SaveChangesAsync();
                    foreach (BetSlip item in _context.BetSlip)
                    {
                        totOdd = totOdd * item.Odd;
                    }
                    TempData["Odd"] = totOdd;
                    _context.BetSlip.Update(temp);
                    await _context.SaveChangesAsync();
                }
            }
            else if(matches!=null && ((top==true)&&(temp_top_status==false)&& (counter > 4)))
            {
                matches.MatchId = MatchId;
                matches.Odd = value;
                matches.TopMatch = top;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (BetSlip item in _context.BetSlip)
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            else if (matches != null && (top == false))
            {
                matches.MatchId = MatchId;
                matches.Odd = value;
                matches.TopMatch = top;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
                decimal totOdd = 1;
                foreach (BetSlip item in _context.BetSlip)
                {
                    totOdd = totOdd * item.Odd;
                }
                TempData["Odd"] = totOdd;
                _context.BetSlip.Update(matches);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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

        // GET: BetSlips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BetSlips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MatchId,HomeTeam,AwayTeam,Type,Odd")] BetSlip betSlip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(betSlip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(betSlip);
        }

        // GET: BetSlips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlip.FindAsync(id);
            if (betSlip == null)
            {
                return NotFound();
            }
            return View(betSlip);
        }

        // POST: BetSlips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MatchId,HomeTeam,AwayTeam,Type,Odd")] BetSlip betSlip)
        {
            if (id != betSlip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(betSlip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BetSlipExists(betSlip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(betSlip);
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
            return RedirectToAction(nameof(Index));
        }

        private bool BetSlipExists(int id)
        {
            return _context.BetSlip.Any(e => e.Id == id);
        }
    }
}
