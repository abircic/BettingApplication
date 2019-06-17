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
        public async Task<IActionResult> Index()
        {
            return View(await _context.BetSlip.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,MatchId,HomeTeam,AwayTeam,Type,Odd,TotalOdd,BetAmount,CashOut")] BetSlip betSlip)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,MatchId,HomeTeam,AwayTeam,Type,Odd,TotalOdd,BetAmount,CashOut")] BetSlip betSlip)
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
