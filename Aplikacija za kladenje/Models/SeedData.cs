using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Aplikacija_za_kladenje.Models
{
    public static class SeedData
    {
        public static async System.Threading.Tasks.Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new Aplikacija_za_kladenjeContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Aplikacija_za_kladenjeContext>>()))
            {
                if (context.Wallet.Any())
                {
                    return;   // DB has been seeded
                }

                context.Wallet.AddRange(
                    new Wallet
                    {
                        Saldo = 0.00m
                    }
                );

                context.Sports.AddRange(
                    new Sports
                    {
                        Name="Football"
                    }
                );
                context.SaveChanges();
                var Sport_Football = context.Sports.SingleOrDefault(s => s.Name.Contains("Football"));
                context.Leagues.AddRange(
                    new Leagues
                    {
                        Name="Spain",
                        Sport=Sport_Football
                    }
                );
                context.SaveChanges();
                var League_Spain = context.Leagues.SingleOrDefault(l => l.Name.Contains("Spain"));
                context.Matches.AddRange(
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Real Madrid", League = League_Spain},
                        AwayTeam = new Teams { Name = "Barcelona", League = League_Spain },
                        Types = new Types { _1 = 2.10m, _X = 3.50m, _2 = 4.20m, _1X=1.50M, _X2=2.70m, _12=1.50m}
                    },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Valencia", League = League_Spain },
                         AwayTeam = new Teams { Name = "Sevilla", League = League_Spain },
                         Types = new Types { _1 = 2.50m, _X = 3.50m, _2 = 2.50m, _1X = 1.40M, _X2 = 1.70m, _12 = 1.80m }
                     },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Atletico Madrid", League = League_Spain },
                         AwayTeam = new Teams { Name = "Villareal",League = League_Spain },
                         Types = new Types { _1 = 2.20m, _X = 3.50m, _2 = 3.80m, _1X = 1.40M, _X2 = 1.70m, _12 = 1.80m }
                     }
                    );
                context.SaveChanges();
            }
        }
    }
}