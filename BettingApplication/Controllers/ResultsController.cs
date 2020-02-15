using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class ResultsController : Controller
    {
        private readonly BettingApplicationContext _context;

        public ResultsController(BettingApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //string[] allowedSports = new string[] { "Nogomet", "TENIS", "Hokej", "Košarka", "Rukomet" };
            //string[] allowedFootballLeagues = new string[]
            //    {"ŠPANJOLSKA", "ITALIJA", "FRANCUSKA", "ENGLESKA", "NJEMAČKA"};
            var date = DateTime.Now;
            string url = $"https://sportdataprovider.volcanobet.me/api/public/Results/getResultOverviews?date={date.Year}-{date.Month.ToString("00")}-{date.Day}T00:00:00.000Z&sportId=1&clientType=WebConsumer&v=1.1.435&lang=sr-Latn-ME";
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
            foreach (var result in results.Where(t=>t.Scores.Count>0))
            {
                var r = new ResultModel();
                r.Date = result.Fixture.StartDate.Date.ToString("dd.MM.yyyy");
                r.SportName = CheckSportName(result.Fixture.Sport.Name);
                r.LeagueName = result.Fixture.League.Name;
                r.Time = $"{result.Fixture.StartDate.Hour.ToString("00")}:{result.Fixture.StartDate.Minute.ToString("00")}";
                r.Teams = $"{result.Fixture.Participants[0].Name} - {result.Fixture.Participants[1].Name}";
                r.WinningTypes = CalculateWinningTypes(Int32.Parse(result.Scores[0].Value), Int32.Parse(result.Scores[1].Value));
                r.Result = $"{result.Scores[0].Value}:{result.Scores[1].Value}";
                if ((_context.Results.Where(t => t.Teams == r.Teams).FirstOrDefault()) == null)
                {
                    _context.AddRange(r);
                    _context.SaveChanges();
                    convertedResult.Add(r);
                }
            }
            return View(convertedResult.OrderBy(l=>l.LeagueName));
        }
        private string CalculateWinningTypes(int firstValue, int secondValue)
        {
            if (firstValue > secondValue)
                return "1;1X;12";
            if (secondValue > firstValue)
                return "2;X2;12";
            return "X";
        }

        private string CheckSportName(string name)
        {
            if (name == "Fudbal")
                return "Nogomet";
            return name;
        }
    }
}