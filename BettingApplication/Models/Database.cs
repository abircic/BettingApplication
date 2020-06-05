using BettingApplication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.ViewModels;

namespace BettingApplication.Models
{
    public class Database
    {
        private readonly BettingApplicationContext _context;

        public Database(BettingApplicationContext context)
        {
            _context = context;
        }
        public void ExportDatabase()
        {
            List<Match> matchesList = _context.Match.Include(c => c.Sport).Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Type).Where(s => s.Sport.Name.Contains("Football")).ToList();
            List<MatchViewModel> matchVmList = matchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Type._1,
                _X = x.Type._X,
                _2 = x.Type._2,
                _1X = x.Type._1X,
                _X2 = x.Type._X2,
                _12 = x.Type._12,
            }).OrderBy((o => o.League)).ToList();
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesExport.csv";
            using (var stream = new StreamWriter(path))
            {
                string line;
                stream.WriteLine("ResultLeagueModel, Home Team, Away Team, 1, X, 2, 1X, X2");
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
        }
        public void ImportDatabase()
        {
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesImport.txt";
            using (var reader = new StreamReader(path))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var match = new MatchViewModel();
                    var sportFootball = _context.Sport.SingleOrDefault(s => s.Name.Contains("Football"));
                    _context.Match.AddRange(
                        new Match
                        {
                            HomeTeam = _context.Team.SingleOrDefault(s => s.Name.Contains(values[0])),
                            AwayTeam = _context.Team.SingleOrDefault(s => s.Name.Contains(values[1])),
                            Type = new Type { _1 = Convert.ToDecimal(values[2]), _X = Convert.ToDecimal(values[3]), _2 = Convert.ToDecimal(values[4]), _1X = Convert.ToDecimal(values[5]), _X2 = Convert.ToDecimal(values[6]), _12 = Convert.ToDecimal(values[7]) },
                            Sport = sportFootball
                        });
                    _context.SaveChanges();
                }
            }
        }
        public void ImportTwoPlayerDatabase()
        {
            string path = @"C:\Users\antee\Documents\Visual Studio 2019\Projects\BettingApplication\MatchesImportTwoPlayer.txt";
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var match = new MatchViewModel();
                    var sportTennis = _context.Sport.SingleOrDefault(s => s.Name.Contains("Tenis"));
                    var leagueATP = _context.League.SingleOrDefault(l => l.Name.Contains("ATP"));
                    _context.Match.AddRange(
                        new Match
                        {
                            HomeTeam = new Team { Name =values[0], League = leagueATP },
                            AwayTeam = new Team { Name = values[1], League = leagueATP },
                            Type = new Type { _1 = 1.65m, _2 = 2.10m, },
                            Sport = sportTennis,
                        });
                    _context.SaveChanges();
                }
            }
        }
    }
}