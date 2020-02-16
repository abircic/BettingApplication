using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BettingApplication.Controllers
{
    public class OfferController : Controller
    {
        private readonly BettingApplicationContext _context;

        public OfferController(BettingApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var date = DateTime.Now;
            string url = $"https://sportdataprovider.volcanobet.me/api/public/prematch/SportEvents?SportId=1&from=2020-02-{date.AddDays(-1).Day}T23:00:00.000Z&to=2020-02-{date.AddDays(1).Day}T06:00:00.000Z&timezone=-60&clientType=WebConsumer&v=1.1.435&lang=sr-Latn-EN";
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var offer = JsonConvert.DeserializeObject<OfferModel>(html);
            var sport = await _context.Sports.Where(s => s.Name == offer.Sport.Name).FirstOrDefaultAsync();
            if (sport == null)
            {
                sport = new Sports();
                sport.Name = offer.Sport.Name;
                _context.AddRange(sport);
                _context.SaveChanges();
            }
            foreach (var location in offer.Locations)
            {
                
                foreach (var league in location.Leagues)
                {
                    var leagueDatabase = await _context.Leagues.Where(l => l.Name == $"{location.Name} - {league.Name}").SingleOrDefaultAsync();
                    if (leagueDatabase == null)
                    {
                         leagueDatabase = new Leagues();
                         leagueDatabase.Name = $"{location.Name} - {league.Name}";
                         leagueDatabase.Sport = sport;
                        _context.Leagues.AddRange(leagueDatabase);
                        _context.SaveChanges();
                    }
                   
                    foreach (var eventDate in league.EventDateGroups)
                    {
                        foreach (var eventDateEvent in eventDate.Events)
                        {
                            var matchModel = new Matches();
                            matchModel.Id = Guid.NewGuid().ToString();
                            var types = new Types();
                            matchModel.Sport = sport;
                            matchModel.Time = eventDateEvent.Fixture.StartDate.AddHours(1);
                            var firstTeam = await _context.Teams.Where(t => t.Name == eventDateEvent.Fixture.Participants[0].Name).FirstOrDefaultAsync();
                            var secondTeam = await _context.Teams.Where(t => t.Name == eventDateEvent.Fixture.Participants[1].Name).FirstOrDefaultAsync();
                            if (firstTeam == null)
                            {
                                firstTeam = new Teams();
                                firstTeam.League = leagueDatabase;
                                firstTeam.Name = eventDateEvent.Fixture.Participants[0].Name;
                                _context.Teams.AddRange(firstTeam);
                                matchModel.HomeTeam = firstTeam;
                            }
                            if (firstTeam != null)
                                matchModel.HomeTeam = firstTeam;
                            if (secondTeam == null)
                            {
                                secondTeam = new Teams();
                                secondTeam.League = leagueDatabase;
                                secondTeam.Name = eventDateEvent.Fixture.Participants[1].Name;
                                _context.Teams.AddRange(secondTeam);
                                matchModel.AwayTeam = secondTeam;
                            }
                            if (secondTeam != null)
                                matchModel.AwayTeam= secondTeam;
                            foreach (var market in eventDateEvent.Markets)
                            {
                                if (market.Name == "1x2")
                                {
                                    foreach (var pick in market.Picks)
                                    {
                                        if (pick.Name == "1")
                                            types._1 = pick.Odds;
                                        if (pick.Name == "x")
                                            types._X = pick.Odds;
                                        if (pick.Name == "2")
                                            types._2 = pick.Odds;
                                    }
                                }
                                if (market.Name == "Double chance")
                                {
                                    foreach (var pick in market.Picks)
                                    {
                                        if (pick.Name == "1x")
                                            types._1X = pick.Odds;
                                        if (pick.Name == "x2")
                                            types._X2 = pick.Odds;
                                        if (pick.Name == "12")
                                            types._12 = pick.Odds;
                                    }
                                }
                            }
                            var matchExist = await _context.Matches
                                .Include(h => h.HomeTeam)
                                .Include(a => a.AwayTeam)
                                .Where(m => m.HomeTeam == matchModel.HomeTeam && m.AwayTeam == matchModel.AwayTeam &&
                                            m.Time == matchModel.Time).FirstOrDefaultAsync();
                            if (matchExist == null && matchModel.HomeTeam != null && matchModel.AwayTeam != null)
                            {
                                matchModel.Types = types;
                                matchModel.TopMatch = false;
                                _context.Matches.Add(matchModel);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
               
               
            }
            return View();
        }
    }
}
