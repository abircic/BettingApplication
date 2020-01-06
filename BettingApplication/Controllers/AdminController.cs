using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BettingApplication.Data;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using System.IO;

namespace BettingApplication.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly BettingApplicationContext _context;

        public AdminController(BettingApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Menu()
        {
            @TempData["UsersForActivate"] = _context.Users.Where(u => u.EmailConfirmed == false).Count();
            return View();
        }

        public async Task<IActionResult> TopMatchIndex()
        {
            return View(await _context.AdminTopMatchConfigs.ToListAsync());
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminTopMatchConfig = await _context.AdminTopMatchConfigs.FindAsync(id);
            if (adminTopMatchConfig == null)
            {
                return NotFound();
            }
            return View(adminTopMatchConfig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MinimumNumberOfMatches")] AdminTopMatchConfig adminTopMatchConfig)
        {
            if (id != adminTopMatchConfig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminTopMatchConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminTopMatchConfigExists(adminTopMatchConfig.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TopMatchIndex));
            }
            return View(adminTopMatchConfig);
        }
        private bool AdminTopMatchConfigExists(int id)
        {
            return _context.AdminTopMatchConfigs.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportDatabase()
        {
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Football")).ToList();
            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Types._1,
                _X = x.Types._X,
                _2 = x.Types._2,
                _1X = x.Types._1X,
                _X2 = x.Types._X2,
                _12 = x.Types._12,
            }).OrderBy((o => o.League)).ToList();
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesExport.csv";
            using (var stream = new StreamWriter(path))
            {
                string line;
                stream.WriteLine("League, Home Team, Away Team, 1, X, 2, 1X, X2");
                stream.Flush();
                foreach (var match in matchVmList)
                {
                    string League = match.League;
                    string HomeTeam = match.HomeTeamName;
                    string AwayTeam = match.AwayTeamName;
                    string _1 = match._1.ToString();
                    string _X = match._X.ToString();
                    string _2 = match._2.ToString();
                    string _1X = match._1X.ToString();
                    string _X2 = match._X2.ToString();
                    line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", League, HomeTeam, AwayTeam, _1, _X, _2, _1X, _X2);
                    stream.WriteLine(line);
                    stream.Flush();
                }
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> ImportDatabase()
        {
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesImport.csv";
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var match = new MatchViewModel();
                    var sportFootball = _context.Sports.SingleOrDefault(s => s.Name.Contains("Football"));
                    var hour = values[8].Split(':');
                    var firstTeam = values[0].Split('"');
                    _context.Matches.AddRange(
                        new Matches
                        {
                            HomeTeam = _context.Teams.First(s => s.Name.Contains(firstTeam[1])),
                            AwayTeam = _context.Teams.First(s => s.Name.Contains(values[1])),
                            Time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, Int32.Parse(hour[0]), Int32.Parse(hour[1]), 00),
                            Types = new Types { _1 = Convert.ToDecimal(values[2]), _X = Convert.ToDecimal(values[3]), _2 = Convert.ToDecimal(values[4]), _1X = Convert.ToDecimal(values[5]), _X2 = Convert.ToDecimal(values[6]), _12 = Convert.ToDecimal(values[7]) },
                            Sport = sportFootball
                        });
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ExportTwoPlayerDatabase()
        {
            List<Matches> matchesList = _context.Matches.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Types).Where(s => s.Sport.Name.Contains("Tennis")).ToList();
            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                _1 = x.Types._1,
                _2 = x.Types._2
            }).ToList();
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesExportTwoPlayer.csv";
            using (var stream = new StreamWriter(path))
            {
                string line;
                stream.WriteLine("First Player, Second Player, 1, 2");
                stream.Flush();
                foreach (var match in matchVmList)
                {
                    string firstPlayer = match.FirstPlayer;
                    string secondPlayer = match.SecondPlayer;
                    string _1 = match._1.ToString();
                    string _2 = match._2.ToString();
                    line = string.Format("{0}, {1}, {2}, {3}", firstPlayer, secondPlayer, _1,_2);
                    stream.WriteLine(line);
                    stream.Flush();
                }
            }
            return RedirectToAction("TwoPlayerIndex", "Home");
        }

            public async Task<IActionResult> ImportTwoPlayerDatabase()
        {
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesImportTwoPlayer.csv";
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var match = new MatchViewModel();
                    var sportTennis = _context.Sports.SingleOrDefault(s => s.Name.Contains("Tennis"));
                    var leagueATP = _context.Leagues.SingleOrDefault(l => l.Name.Contains("ATP"));
                    var hour = values[4].Split(':');
                    var firstTeam = values[0].Split('"');
                    _context.Matches.AddRange(
                        new Matches
                        {
                            HomeTeam = new Teams { Name = firstTeam[1], League = leagueATP },
                            AwayTeam = new Teams { Name = values[1], League = leagueATP },
                            Types = new Types { _1 = Convert.ToDecimal(values[2]), _X = Convert.ToDecimal(values[3])},
                            Time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, Int32.Parse(hour[0]), Int32.Parse(hour[1]), 00),
                            Sport = sportTennis,
                        });
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("TwoPlayerIndex", "Home");
        }
    }
}