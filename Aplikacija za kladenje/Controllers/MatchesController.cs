using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;
using System.Web;

namespace Aplikacija_za_kladenje.Controllers
{
    public class MatchesController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        public MatchesController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: Matches
        [HttpGet]
        public IActionResult Index()
        {
            List<Matches> MatchesList = _context.Matches.Include(h=>h.HomeTeam).Include(a=>a.AwayTeam).Include(t=>t.Types).ToList();

            MatchViewModel MatchVM = new MatchViewModel();

            List<MatchViewModel> MatchVMList = MatchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                HomeTeamName=x.HomeTeam.Name,
                AwayTeamName=x.AwayTeam.Name,
                _1=x.Types._1,
                _X=x.Types._X,
                _2 = x.Types._2,
                _1X = x.Types._1X,
                _X2 = x.Types._X2,
                _12 = x.Types._12,
            }).ToList();

            return View(MatchVMList);
        }
        [HttpGet]
        public async Task<IActionResult> BetTest()
        {
            return View(await _context.BetSlip.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> BetTest(int MatchId, decimal value, string type)
        {
            BetSlip temp = new BetSlip();
            var matches = _context.BetSlip.SingleOrDefault(m => m.MatchId == MatchId);
            if (matches == null)
            {
                temp.MatchId = MatchId;
                var match = _context.Matches.Include(h=>h.HomeTeam).Include(a=>a.AwayTeam).SingleOrDefault(q => q.Id == MatchId);
                temp.HomeTeam = match.HomeTeam.Name;
                temp.AwayTeam = match.AwayTeam.Name;
                var testiranje = _context.Matches.Include(o => o.Types).SingleOrDefault(q => q.Id == MatchId);
                temp.Odd = value;
                temp.TotalOdd = 1;
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
            else
            {
                matches.MatchId = MatchId;
                matches.Odd = value;
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
        



















        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HomeTeam,AwayTeam,Result")] Matches matches)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matches);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(matches);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches.FindAsync(id);
            if (matches == null)
            {
                return NotFound();
            }
            return View(matches);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HomeTeam,AwayTeam,Result")] Matches matches)
        {
            if (id != matches.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matches);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchesExists(matches.Id))
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
            return View(matches);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matches = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchesExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
