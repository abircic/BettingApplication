using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplikacija_za_kladenje.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
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

                if (context.Sports.Any())
                {
                    return;   // DB has been seeded
                }

                context.Sports.AddRange(
                    new Sports
                    {
                        Name="Football"
                    }
                );
                if (context.Leagues.Any())
                {
                    return;   // DB has been seeded
                }

                context.Leagues.AddRange(
                    new Leagues
                    {
                        Name="Spain"
                    },
                    new Leagues
                    {
                        Name = "England"
                    }
                );

                if (context.Matches.Any())
                {
                    return;   // DB has been seeded
                }

                context.Matches.AddRange(
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Real Madrid"},
                        AwayTeam = new Teams { Name = "Barcelona" },
                        Types = new Types { _1 = 1.40m, _X = 3.50m, _2 = 7.50m, _1X=1.10M, _X2=3.50m, _12=1.50m}
                    },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Valencia" },
                         AwayTeam = new Teams { Name = "Sevilla" },
                         Types = new Types { _1 = 2.50m, _X = 3.50m, _2 = 2.50m, _1X = 1.40M, _X2 = 1.70m, _12 = 1.80m }
                     },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Atletico Madrid" },
                         AwayTeam = new Teams { Name = "Villareal" },
                         Types = new Types { _1 = 2.50m, _X = 3.50m, _2 = 2.50m, _1X = 1.40M, _X2 = 1.70m, _12 = 1.80m }
                     }
                    );
                context.SaveChanges();
            }
        }
    }
}