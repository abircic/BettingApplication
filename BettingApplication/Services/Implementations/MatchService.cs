using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Type = BettingApplication.Models.Type;

namespace BettingApplication.Services.Implementations
{
    public class MatchService : IMatchService
    {
        private readonly BettingApplicationContext _context;

        public MatchService(BettingApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<MatchViewModel>>Index()
        {
            List<Match> matchesList = _context.Match.Include(a => a.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type).ToList();

            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1,
                _X = x.Type._X,
                _2 = x.Type._2,
                _1X = x.Type._1X,
                _X2 = x.Type._X2,
                _12 = x.Type._12,
            }).ToList();
            return matchVmList;
        }

        public async Task<List<TwoPlayersViewModel>> IndexTwoPlayers()
        {
            List<Match> matchesList = _context.Match.Include(a => a.Sport)
                .Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .Where(s => s.Sport.Name.Contains("Tenis")).ToList();

            TwoPlayersViewModel matchVm = new TwoPlayersViewModel();

            List<TwoPlayersViewModel> matchVmList = matchesList.Select(x => new TwoPlayersViewModel
            {
                Id = x.Id,
                FirstPlayer = x.HomeTeam.Name,
                SecondPlayer = x.AwayTeam.Name,
                Time = x.Time,
                _1 = x.Type._1,
                _2 = x.Type._2
            }).ToList();
            return matchVmList;
        }

        public async Task<List<TopMatchesViewModel>> TopMatches()
        {
            List<Match> topMatches = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type)
                .Where(s => s.Sport.Name.Contains("Football")).Where(t => t.TopMatch == true).ToList();
            List<Match> topTwoPlayersMatches = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).Include(a => a.AwayTeam)
                .Include(t => t.Type).Where(s => s.Sport.Name.Contains("Tenis"))
                .Where(t => t.TopMatch == true).ToList();

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
            return allMatches;
        }

        public async Task Create(Match match)
        {
            await _context.AddAsync(match);
            await _context.SaveChangesAsync();
        }

        public async Task<Match> GetEdit(int? id)
        {
            var match = await _context.Match.FindAsync(id);
            return match;
        }

        public async Task PostEdit(Match match)
        {
            _context.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task<Match> GetDelete(string id)
        {
            var match = await _context.Match.Include(h => h.HomeTeam)
                .Include(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            return match;
        }

        public async Task Delete(Match match)
        {
            _context.Update(match);
            await _context.SaveChangesAsync();
        }
        public async Task<Match> DeleteConfirmed(string id)
        {
            var match = await _context.Match.FindAsync(id);
            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public bool IsMatchExist(string id)
        {
            var response = _context.Match.Any(e => e.Id == id);
            return response;
        }

        public async Task<List<Team>> GetAdd()
        {
            List<Team> teamList = _context.Team.Include(l => l.League).OrderBy(l => l.League).ToList();
            return teamList;
        }

        public async Task PostAdd(string First, string Second, decimal _1, decimal _X, decimal _2, decimal _1X,
            decimal _X2, decimal _12, string league, bool topMatch, string time)
        {
            var dateNow = DateTime.Now;
            var hour = time.Split(":");
            var type = new Type();
            type._1 = _1;
            type._X = _X;
            type._2 = _2;
            type._1X = _1X;
            type._X2 = _X2;
            type._12 = _12;
            await _context.Type.AddAsync(type);
            await _context.SaveChangesAsync();
            var firstTeam = await _context.Team.Include(l => l.League).Where(f => f.Name == First).FirstOrDefaultAsync();
            var secondTeam = await _context.Team.Include(l => l.League).Where(f => f.Name == Second).FirstOrDefaultAsync();
            var leagueTeam = await _context.League.Include(s => s.Sport).Where(l => l.Name == league).FirstOrDefaultAsync();
            var match = new Match();
            match.HomeTeam = firstTeam;
            match.AwayTeam = secondTeam;
            match.Type = type;
            match.TopMatch = topMatch;
            match.Sport = leagueTeam.Sport;
            match.Time = new DateTime(dateNow.Year,dateNow.Month,dateNow.Day,Int32.Parse(hour[0]), Int32.Parse(hour[1]),00);
            await _context.Match.AddAsync(match);
            await _context.SaveChangesAsync();
        }
    }
}
