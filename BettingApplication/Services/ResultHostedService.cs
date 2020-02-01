using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BettingApplication.Services
{
    public class ResultHostedService : IHostedService
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly BettingApplicationContext _context = null;

        public ResultHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _timer = new Timer(async (e) => { GetResult(); }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
        }

        public async Task GetResult()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<BettingApplicationContext>();
                string[] allowedSports = new string[] {"nogomet", "tenis", "hokej", "košarka", "rukomet"};
                var date = DateTime.Now;
                string url =
                    $"https://www.supersport.hr/rezultati/sport/{date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}";
                string html;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                var results = JsonConvert.DeserializeObject<SuperSportResultsModelDto>(html);
                var convertedResults = new List<SuperSportResultModel>();
                foreach (var sport in results.Sports.Where(s => allowedSports.Contains(s.SportName.ToLower())))
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
            }
        }
    }
}
