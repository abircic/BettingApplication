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
    public class SportController : Controller
    {
        private readonly BettingApplicationContext _context;

        public SportController(BettingApplicationContext context)
        {
            _context = context;
        }

        // GET: ResultSportModel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sport.ToListAsync());
        }

        // GET: ResultSportModel/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Sport = await _context.Sport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Sport == null)
            {
                return NotFound();
            }

            return View(Sport);
        }

        // GET: ResultSportModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResultSportModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sport);
                await _context.SaveChangesAsync();
                return RedirectToAction("Add", "Matches");
            }
            return View(sport);
        }

        // GET: ResultSportModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Sport = await _context.Sport.FindAsync(id);
            if (Sport == null)
            {
                return NotFound();
            }
            return View(Sport);
        }

        // POST: ResultSportModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Sport sport)
        {
            if (id != sport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportExists(sport.Id))
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
            return View(sport);
        }

        // GET: ResultSportModel/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Sport = await _context.Sport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Sport == null)
            {
                return NotFound();
            }

            return View(Sport);
        }

        // POST: ResultSportModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Sport = await _context.Sport.FindAsync(id);
            _context.Sport.Remove(Sport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportExists(string id)
        {
            return _context.Sport.Any(e => e.Id == id);
        }
    }
}
