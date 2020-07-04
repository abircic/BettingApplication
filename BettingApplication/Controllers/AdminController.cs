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
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Type = BettingApplication.Models.Type;

namespace BettingApplication.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly IAccountService _accountService;
        private readonly IAdminService _adminService;

        public AdminController(BettingApplicationContext context, IAccountService accountService, IAdminService adminService)
        {
            _context = context;
            _accountService = accountService;
            _adminService = adminService;
        }

        public async Task<IActionResult> Menu()
        {
            var response = await _accountService.GetUsersForActivate();
            @TempData["UsersForActivate"] = response.Count;
            return View();
        }

        public async Task<IActionResult> TopMatchIndex()
        {
            return View(await _context.AdminTopMatchConfig.ToListAsync());
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminTopMatchConfig = await _context.AdminTopMatchConfig.FindAsync(id);
            if (adminTopMatchConfig == null)
            {
                return NotFound();
            }
            return View(adminTopMatchConfig);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,MinimumNumberOfMatches")] AdminTopMatchConfig adminTopMatchConfig)
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
        private bool AdminTopMatchConfigExists(string id)
        {
            return _context.AdminTopMatchConfig.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportDatabase()
        {
            await _adminService.ExportDatabase();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> ImportDatabase()
        {
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesImport.csv";
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        var match = new MatchViewModel();
                        var sportFootball = _context.Sport.SingleOrDefault(s => s.Name.Contains("Football"));
                        var hour = values[8].Split(':');
                        var firstTeam = values[0].Split('"');
                        var secondTeam = values[1];
                        var time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                            Int32.Parse(hour[0]), Int32.Parse(hour[1]), 00);
                        var HomeTeam = _context.Team.FirstOrDefault(s => s.Name.Contains(firstTeam[1]));
                        var AwayTeam = _context.Team.FirstOrDefault(s => s.Name.Contains(values[1]));
                        if (HomeTeam == null || AwayTeam == null)
                        {
                            ModelState.AddModelError("Error", $"Wrong name: {firstTeam[1]} or {values[1]}");
                            return View();
                        }
                        var Time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                            Int32.Parse(hour[0]), Int32.Parse(hour[1]), 00);
                        var Types = new Type
                        {
                            _1 = Convert.ToDecimal(values[2]),
                            _X = Convert.ToDecimal(values[3]),
                            _2 = Convert.ToDecimal(values[4]),
                            _1X = Convert.ToDecimal(values[5]),
                            _X2 = Convert.ToDecimal(values[6]),
                            _12 = Convert.ToDecimal(values[7])
                        };
                        var Sport = sportFootball;
                        if ((await _context.Match.Where(t => t.HomeTeam.Name == firstTeam[1] && t.AwayTeam.Name == secondTeam && t.Time == time).FirstOrDefaultAsync()) == null)
                        {
                            _context.Match.AddRange(
                                new Match
                                {
                                    HomeTeam = _context.Team.First(s => s.Name.Contains(firstTeam[1])),
                                    AwayTeam = _context.Team.First(s => s.Name.Contains(values[1])),
                                    Time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, Int32.Parse(hour[0]), Int32.Parse(hour[1]), 00),
                                    Type = new Type { _1 = Convert.ToDecimal(values[2]), _X = Convert.ToDecimal(values[3]), _2 = Convert.ToDecimal(values[4]), _1X = Convert.ToDecimal(values[5]), _X2 = Convert.ToDecimal(values[6]), _12 = Convert.ToDecimal(values[7]) },
                                    Sport = sportFootball
                                });
                            _context.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Error", $"{e}");
                        return View();
                    }
                }
                return View();
            }
        }
        public async Task<IActionResult> ExportTwoPlayerDatabase()
        {
            List<Match> matchesList = _context.Match.Include(c => c.Sport).Include(h => h.HomeTeam).Include(a => a.AwayTeam).Include(t => t.Type).Where(s => s.Sport.Name.Contains("Tenis")).ToList();
            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                _1 = x.Type._1,
                _2 = x.Type._2
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
            await _adminService.ImportTwoPlayerDatabase();
            return RedirectToAction("TwoPlayerIndex", "Home");
        }
    }
}