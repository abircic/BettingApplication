using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using BettingApplication.Data;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Type = BettingApplication.Models.Type;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class MatchesController : Controller
    {
        private readonly BettingApplicationContext _context;

        public MatchesController(BettingApplicationContext context)
        {
            _context = context;
        }

        // GET: Match
        [HttpGet]
        public IActionResult Index()
        {
            List<Match> matchesList = _context.Match.Include(a=>a.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Type).ToList();


            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League=x.HomeTeam.League.Name,
                HomeTeamName=x.HomeTeam.Name,
                AwayTeamName=x.AwayTeam.Name,
                Time = x.Time,
                _1 =x.Type._1,
                _X=x.Type._X,
                _2 = x.Type._2,
                _1X = x.Type._1X,
                _X2 = x.Type._X2,
                _12 = x.Type._12,
            }).ToList();

            return View(matchVmList.OrderBy(o => o.League));
        }


        [HttpGet]
        public IActionResult IndexTwoPlayers()
        {
            List<Match> matchesList = _context.Match.Include(a=>a.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Where(s => s.Sport.Name.Contains("Tenis")).ToList();

            TwoPlayersViewModel matchVm = new TwoPlayersViewModel();

            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer=x.HomeTeam.Name,
                SecondPlayer=x.AwayTeam.Name,
                Time = x.Time,
                _1 =x.Type._1,
                _2=x.Type._2
            }).ToList();

            return View(matchVmList);
        }

        [HttpGet]
        public IActionResult TopMatches()
        {
            List<Match> topMatches = _context.Match.Include(c => c.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Type).Where(s => s.Sport.Name.Contains("Football")).Where(t => t.TopMatch == true).ToList();
            List<Match> topTwoPlayersMatches = _context.Match.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Type).Where(s => s.Sport.Name.Contains("Tenis")).Where(t => t.TopMatch == true).ToList();
            List<TopMatchesViewModel> allMatches = new List<TopMatchesViewModel>();
            List<TopMatchesViewModel> matchVmList = topMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1 + 0.10m,
                _X = x.Type._X + 0.10m,
                _2 = x.Type._2 + 0.10m,
                _1X = x.Type._1X + 0.10m,
                _X2 = x.Type._X2 + 0.10m,
                _12 = x.Type._12 + 0.10m
            }).ToList();

            List<TopMatchesViewModel> twoPlayersMatchVmList = topTwoPlayersMatches.Select(x => new TopMatchesViewModel
            {
                Id = x.Id,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Type._1 + 0.10m,
                _2 = x.Type._2 + 0.10m
            }).ToList();
            allMatches.AddRange(matchVmList);
            allMatches.AddRange(twoPlayersMatchVmList);
            return View(allMatches);
        }


        // GET: Match/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HomeTeam,AwayTeam,ResultModel")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Match/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Match.FindAsync(id);
            if (matches == null)
            {
                return NotFound();
            }
            return View(matches);
        }

        // POST: Match/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,HomeTeam,AwayTeam,ResultModel")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchesExists(match.Id))
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
            return View(match);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _context.Match.Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        // POST: test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var matches = await _context.Match.FindAsync(id);
            _context.Match.Remove(matches);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchesExists(string id)
        {
            return _context.Match.Any(e => e.Id == id);
        }
        public IActionResult Add()
        {
            List<Team> teamList = _context.Team.Include(l => l.League).OrderBy(l => l.League).ToList();


            return View(teamList);
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(string First, string Second, decimal _1, decimal _X, decimal _2, decimal _1X, decimal _X2, decimal _12, string league, bool topMatch)
        {
            var type = new Type();
            type._1 = _1;
            type._X = _X;
            type._2 = _2;
            type._1X = _1X;
            type._X2 = _X2;
            type._12 = _12;
            _context.Type.Add(type);
            _context.SaveChanges();
            var firstTeam = _context.Team.Include(l => l.League).Where(f => f.Name == First).FirstOrDefault();
            var secondTeam = _context.Team.Include(l => l.League).Where(f => f.Name == Second).FirstOrDefault();
            var leagueTeam = _context.League.Include(s => s.Sport).Where(l => l.Name == league).FirstOrDefault();
            var match = new Match();
            match.HomeTeam = firstTeam;
            match.AwayTeam = secondTeam;
            match.Type = type;
            match.TopMatch = topMatch;
            match.Sport = leagueTeam.Sport;
            _context.Match.Add(match);
            _context.SaveChanges();
            return RedirectToAction("Index", "Matches");
        }
    }
}
