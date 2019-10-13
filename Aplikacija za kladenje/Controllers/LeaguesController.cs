using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Data;
using Aplikacija_za_kladenje.Models;
using Microsoft.AspNetCore.Authorization;

namespace Aplikacija_za_kladenje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaguesController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;

        public LeaguesController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        // GET: Leagues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leagues.ToListAsync());
        }

        // GET: Leagues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leagues == null)
            {
                return NotFound();
            }

            return View(leagues);
        }


        // GET: Leagues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues.FindAsync(id);
            if (leagues == null)
            {
                return NotFound();
            }
            return View(leagues);
        }

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Leagues leagues)
        {
            if (id != leagues.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leagues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaguesExists(leagues.Id))
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
            return View(leagues);
        }

        // GET: Leagues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leagues == null)
            {
                return NotFound();
            }

            return View(leagues);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leagues = await _context.Leagues.FindAsync(id);
            _context.Leagues.Remove(leagues);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaguesExists(int id)
        {
            return _context.Leagues.Any(e => e.Id == id);
        }
        public IActionResult Add()
        {
            List<Sports> sportList = _context.Sports.Include(l => l.Leagues).ToList();


            return View(sportList);
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string name, string sport)
        {
            var sportName = _context.Sports.Where(s=>s.Name == sport).FirstOrDefault();
            var league = new Leagues();
            league.Name = name;
            league.Sport = sportName;
            _context.Leagues.Add(league);
            _context.SaveChanges();
            return RedirectToAction("Add", "Matches");
        }
    }
}
