using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Data;
using BettingApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace BettingApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeagueController : Controller
    {
        private readonly BettingApplicationContext _context;

        public LeagueController(BettingApplicationContext context)
        {
            _context = context;
        }

        // GET: ResultLeagueModel
        public async Task<IActionResult> Index()
        {
            return View(await _context.League.ToListAsync());
        }

        // GET: ResultLeagueModel/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var League = await _context.League
                .FirstOrDefaultAsync(m => m.Id == id);
            if (League == null)
            {
                return NotFound();
            }

            return View(League);
        }


        // GET: ResultLeagueModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var League = await _context.League.FindAsync(id);
            if (League == null)
            {
                return NotFound();
            }
            return View(League);
        }

        // POST: ResultLeagueModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] League league)
        {
            if (id != league.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(league);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeagueExists(league.Id))
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
            return View(league);
        }

        // GET: ResultLeagueModel/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var League = await _context.League
                .FirstOrDefaultAsync(m => m.Id == id);
            if (League == null)
            {
                return NotFound();
            }

            return View(League);
        }

        // POST: ResultLeagueModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var League = await _context.League.FindAsync(id);
            _context.League.Remove(League);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeagueExists(string id)
        {
            return _context.League.Any(e => e.Id == id);
        }
        public IActionResult Add()
        {
            List<Sport> sportList = _context.Sport.Include(l => l.League).ToList();


            return View(sportList);
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string name, string sport)
        {
            var sportName = _context.Sport.Where(s=>s.Name == sport).FirstOrDefault();
            var league = new League();
            league.Name = name;
            league.Sport = sportName;
            _context.League.Add(league);
            _context.SaveChanges();
            return RedirectToAction("Add", "Matches");
        }
    }
}
