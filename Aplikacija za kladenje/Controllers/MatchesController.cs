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
        public IActionResult IndexTwoPlayers()
        {
            List<TwoPlayersMatches> MatchesList = _context.TwoPlayersMatches.Include(f => f.First).Include(s => s.Second).Include(s=>s.Sport).ToList();

            TwoPlayersViewModel MatchVM = new TwoPlayersViewModel();

            List<TwoPlayersViewModel> MatchVMList = MatchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer=x.First.Name,
                SecondPlayer=x.Second.Name,
                _1=x._1,
                _2=x._2
            }).ToList();

            return View(MatchVMList);
        }

        [HttpGet]
        public IActionResult TopMatches()
        {
            List<Matches> TopMatches = _context.Matches.Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(t => t.TopMatch==true).ToList();
            List<TwoPlayersMatches> TopTwoPlayersMatches = _context.TwoPlayersMatches.Include(f => f.First).Include(s => s.Second).Include(s => s.Sport).Where(t => t.TopMatch == true).ToList();
            List<TopMatchesViewModel> AllMatches=new List<TopMatchesViewModel>();
            TopMatchesViewModel MatchVm = new TopMatchesViewModel();
            List<TopMatchesViewModel> MatchVMList = TopMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Types._1+0.10m,
                _X = x.Types._X + 0.10m,
                _2 = x.Types._2 + 0.10m,
                _1X = x.Types._1X + 0.10m,
                _X2 = x.Types._X2 + 0.10m,
                _12 = x.Types._12 + 0.10m
            }).ToList();

            List<TopMatchesViewModel> TwoPlayersMatchVMList = TopTwoPlayersMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName=x.First.Name,
                AwayTeamName=x.Second.Name,
                _1=x._1 + 0.10m,
                _2=x._2 + 0.10m
            }).ToList();
            AllMatches.AddRange(MatchVMList);
            AllMatches.AddRange(TwoPlayersMatchVMList);
            return View(AllMatches);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,HomeTeam,AwayTeam,Result")] Matches matches)
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
        public async Task<IActionResult> Delete(string id)
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

        private bool MatchesExists(string id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
