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
            string[] allowedSports = new string[] { "Nogomet", "Tenis", "Hokej", "Košarka", "Rukomet" };
            var date = DateTime.Now;
            string url = $"https://www.supersport.hr/rezultati/sport/{date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}";
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var results = JsonConvert.DeserializeObject<SuperSportResultsModelDto>(html);
            var convertedResults = new List<SuperSportResultModel>();
            foreach (var sport in results.Sports.Where(s => allowedSports.Contains(s.SportName)))
            {
                foreach (var league in sport.Leagues)
                {
                    foreach (var result in league.Results)
                    {
                        var r = new SuperSportResultModel();
                        r.Date = results.Date;
                        r.SportName = sport.SportName;
                        r.LeagueName = league.LeagueName;
                        r.Time = result.Time;
                        r.Teams = result.Teams;
                        r.WinningTypes = result.WinningTypes;
                        r.Result = result.Result;
                        if ((_context.Results.Where(t => t.Teams == r.Teams).FirstOrDefault()) == null)
                        {
                            _context.AddRange(r);
                            _context.SaveChanges();
                            convertedResults.Add(r);
                        }
                    }
                }
            }
            return View(convertedResults);
        }
    }
}