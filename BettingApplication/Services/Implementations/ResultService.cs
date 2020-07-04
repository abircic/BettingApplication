using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BettingApplication.Services.Implementations
{
    public class ResultService : IResultService
    {
        private readonly BettingApplicationContext _context;

        public ResultService(BettingApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<ResultModel>> GetResults()
        {
            //string[] allowedSport = new string[] { "Football", "TENIS", "Hokej", "Košarka", "Rukomet" };
            //string[] allowedFootballLeague = new string[]
            //    {"ŠPANJOLSKA", "ITALIJA", "FRANCUSKA", "ENGLESKA", "NJEMAČKA"};
            var date = DateTime.Now;
            string url = $"https://sportdataprovider.volcanobet.me/api/public/Results/getResultOverviews?date={date.Year}-{date.Month.ToString("00")}-{date.Day}T00:00:00.000Z&sportId=1&clientType=WebConsumer&v=1.1.435&lang=sr-Latn-EN";
            //string url = "https://sportdataprovider.volcanobet.me/api/public/Results/getDailyResultOverviews?sportId=1&clientType=WebConsumer&v=1.1.496-rc6&lang=sr-Latn-ME";
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var results = JsonConvert.DeserializeObject<List<ResultsModel>>(html);
            var convertedResult = new List<ResultModel>();
            foreach (var result in results.Where(t => t.Scores.Count > 0))
            {
                var match = await _context.Result.Where(t => t.Id == result.Fixture.EventId).FirstOrDefaultAsync();
                if (match == null)
                {
                    var r = new ResultModel();
                    r.Id = result.Fixture.EventId;
                    r.SportName = result.Fixture.Sport.Name;
                    r.LeagueName = $"{result.Fixture.League.LocationName} - {result.Fixture.League.Name}";
                    r.Time = result.Fixture.StartDate.AddHours(1);
                    r.HomeTeam = result.Fixture.Participants[0].Name;
                    r.AwayTeam = result.Fixture.Participants[1].Name;
                    r.WinningTypes = CalculateWinningTypes(Int32.Parse(result.Scores[0].Value),
                        Int32.Parse(result.Scores[1].Value));
                    r.Result = $"{result.Scores[0].Value}:{result.Scores[1].Value}";
                    _context.AddRange(r);
                    _context.SaveChanges();
                    convertedResult.Add(r);
                }
            }

            return convertedResult;
        }

        public async Task<List<ResultModel>> GetYesterdayResults()
        {
            //string[] allowedSport = new string[] { "Football", "TENIS", "Hokej", "Košarka", "Rukomet" };
            //string[] allowedFootballLeague = new string[]
            //    {"ŠPANJOLSKA", "ITALIJA", "FRANCUSKA", "ENGLESKA", "NJEMAČKA"};
            var date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string url = $"https://sportdataprovider.volcanobet.me/api/public/Results/getResultOverviews?date={date}T00:00:00.000Z&sportId=1&clientType=WebConsumer&v=1.1.507&lang=sr-Latn-EN";
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var results = JsonConvert.DeserializeObject<List<ResultsModel>>(html);
            var convertedResult = new List<ResultModel>();
            foreach (var result in results.Where(t => t.Scores.Count > 0))
            {
                var match = await _context.Result.Where(t => t.Id == result.Fixture.EventId).FirstOrDefaultAsync();
                if (match == null)
                {
                    var r = new ResultModel
                    { 
                        Id = result.Fixture.EventId,
                        SportName = result.Fixture.Sport.Name,
                        LeagueName = $"{result.Fixture.League.LocationName} - {result.Fixture.League.Name}",
                        Time = result.Fixture.StartDate.AddHours(1),
                        HomeTeam = result.Fixture.Participants[0].Name,
                        AwayTeam = result.Fixture.Participants[1].Name,
                        WinningTypes = CalculateWinningTypes(Int32.Parse(result.Scores[0].Value), Int32.Parse(result.Scores[1].Value)),
                        Result = $"{result.Scores[0].Value}:{result.Scores[1].Value}",
                };
                    await _context.AddRangeAsync(r);
                    await _context.SaveChangesAsync();
                    convertedResult.Add(r);
                }
            }

            return convertedResult;
        }

        public async Task<List<Match>> GetMatch()
        {
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type)
                .Where(s => s.Sport.Name.Contains("Football") && s.Result == null).OrderBy(x => x.Competition).ToList();
            return matchesList;
        }

        public async Task<List<Match>> GetYesterdayMatch()
        {
            var dateTime = DateTime.Now.AddDays(-1);
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type)
                .Where(s => s.Sport.Name.Contains("Football") && s.Time.Day == dateTime.Day && s.Result == null)
                .OrderBy(x => x.Competition).ToList();
            return matchesList;
        }
        public async Task<List<Match>> AddResult(string MatchId, string result)
        {
            var match = _context.Match.Where((x => x.Id == MatchId)).Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League).SingleOrDefault();
            match.Result = result;
            _context.Match.Update(match);
            var newResult = new ResultModel();
            var exist = await _context.Result.Where(x => x.Id == MatchId).SingleOrDefaultAsync();
            if (exist == null)
            {
                newResult.Id = match.Id;
                newResult.HomeTeam = match.HomeTeam.Name;
                newResult.AwayTeam = match.AwayTeam.Name;
                newResult.Time = match.Time;
                newResult.Result = result;
                newResult.SportName = match.Sport.Name;
                var splitResult = result.Split(":");
                newResult.WinningTypes = CalculateWinningTypes(Int32.Parse(splitResult[0]),
                    Int32.Parse(splitResult[1]));
                _context.Result.Add(newResult);
            }

            _context.SaveChanges();
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type).Where(s => s.Sport.Name.Contains("Football") && s.Result == null).OrderBy(x => x.Competition).ToList();
            return matchesList;
        }

        public async Task<List<Match>> AddYesterdayResult(string MatchId, string result)
        {
            var dateTime = DateTime.Now.AddDays(-1);
            var match = _context.Match.Where((x => x.Id == MatchId)).Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League).SingleOrDefault();
            match.Result = result;
            _context.Match.Update(match);
            var newResult = new ResultModel();
            var exist = await _context.Result.Where(x => x.Id == MatchId).SingleOrDefaultAsync();
            if (exist == null)
            {
                newResult.Id = match.Id;
                newResult.HomeTeam = match.HomeTeam.Name;
                newResult.AwayTeam = match.AwayTeam.Name;
                newResult.Time = match.Time;
                newResult.Result = result;
                newResult.SportName = match.Sport.Name;
                var splitResult = result.Split(":");
                newResult.WinningTypes = CalculateWinningTypes(Int32.Parse(splitResult[0]),
                    Int32.Parse(splitResult[1]));
                _context.Result.Add(newResult);
            }
            _context.SaveChanges();
            List<Match> matchesList = _context.Match.Include(c => c.Sport)
                .Include(h => h.HomeTeam).ThenInclude(l => l.League)
                .Include(a => a.AwayTeam).ThenInclude(l => l.League)
                .Include(t => t.Type)
                .Where(s => s.Sport.Name.Contains("Football") && s.Time.Day == dateTime.Day && s.Result == null)
                .OrderBy(x => x.Competition).ToList();
            return matchesList;
        }

        #region Private

        private string CalculateWinningTypes(int firstValue, int secondValue)
        {
            if (firstValue > secondValue)
                return "1;1X;12";
            if (secondValue > firstValue)
                return "2;X2;12";
            return "1X;X;X2";
        }

        #endregion
    }
}
