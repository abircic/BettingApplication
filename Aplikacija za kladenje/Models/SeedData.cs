using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aplikacija_za_kladenje.Data;

namespace Aplikacija_za_kladenje.Models
{
    public static class SeedData
    {
        public static void InitializeAsync(IServiceProvider serviceProvider)
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
                        Saldo = 100.00m
                    }
                );

                context.Sports.AddRange(
                    new Sports
                    {
                        Name = "Football"
                    }
                    ,
                    new Sports
                    {
                        Name = "Tennis"
                    }
                );
                context.SaveChanges();
                var sportFootball = context.Sports.SingleOrDefault(s => s.Name.Contains("Football"));
                context.Leagues.AddRange(
                    new Leagues
                    {
                        Name = "Spain",
                        Sport = sportFootball
                    },
                     new Leagues
                     {
                         Name = "Italy",
                         Sport = sportFootball
                     },
                     new Leagues
                     {
                         Name = "France",
                         Sport = sportFootball
                     },
                      new Leagues
                      {
                          Name = "England",
                          Sport = sportFootball
                      }
                );
                context.SaveChanges();
                var leagueSpain = context.Leagues.SingleOrDefault(l => l.Name.Contains("Spain"));
                var leagueItaly = context.Leagues.SingleOrDefault(l => l.Name.Contains("Italy"));
                var leagueFrance = context.Leagues.SingleOrDefault(l => l.Name.Contains("France"));
                var leagueEngland = context.Leagues.SingleOrDefault(l => l.Name.Contains("England"));
                context.Matches.AddRange(
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Liverpool", League = leagueEngland },
                        AwayTeam = new Teams { Name = "Norwich", League = leagueEngland },
                        Types = new Types { _1 = 1.14m,_X = 6.00m, _2 = 17.00m, _X2 = 11.00m, _12 = 1.30m },
                        TopMatch = true
                    },
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "West Ham", League = leagueEngland },
                        AwayTeam = new Teams { Name = "Man City", League = leagueEngland },
                        Types = new Types { _1 =11.00m, _X = 6.00m, _2 = 1.30m, _1X = 5.00m, _12 = 1.30m },
                        TopMatch = true
                    },
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Bournemouth", League = leagueEngland },
                        AwayTeam = new Teams { Name = "Sheffield Utd", League = leagueEngland },
                        Types = new Types { _1 = 1.85m, _X = 3.80m, _2 = 3.80m, _1X = 1.25m, _X2=1.80M,_12 = 1.30m },
                        TopMatch = false
                    },
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Burnley", League = leagueEngland },
                        AwayTeam = new Teams { Name = "Southampton", League = leagueEngland },
                        Types = new Types { _1 = 2.55m, _X = 3.20m, _2 = 2.80m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                        TopMatch = false
                    },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Crystal Palace", League = leagueEngland },
                         AwayTeam = new Teams { Name = "Everton", League = leagueEngland },
                         Types = new Types { _1 = 2.50m, _X = 3.30m, _2 = 2.75m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                         TopMatch = false
                     },
                      new Matches
                      {
                          HomeTeam = new Teams { Name = "Watford", League = leagueEngland },
                          AwayTeam = new Teams { Name = "Brighton", League = leagueEngland },
                          Types = new Types { _1 = 1.90m, _X = 3.40m, _2 = 4.00m, _1X = 1.35m, _X2 = 1.90M, _12 = 1.50m },
                          TopMatch = false
                      },
                      new Matches
                      {
                          HomeTeam = new Teams { Name = "Tottenham", League = leagueEngland },
                          AwayTeam = new Teams { Name = "Aston Villa", League = leagueEngland },
                          Types = new Types { _1 = 1.25m, _X = 5.75m, _2 = 13.00m, _X2 = 4.00m, _12 = 1.50m },
                          TopMatch = false
                      },
                    new Matches
                    {
                        HomeTeam = new Teams { Name = "Eibar", League = leagueEngland },
                        AwayTeam = new Teams { Name = "Barcelona", League = leagueEngland },
                        Types = new Types { _1 = 4.75m, _X = 4.10m, _2 = 1.70m, _1X = 2.50m, _X2 = 1.20m, _12 = 1.50m },
                        TopMatch= false
                    },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Real Madrid", League = leagueSpain },
                         AwayTeam = new Teams { Name = "Betis", League = leagueSpain },
                         Types = new Types { _1 = 1.30m, _X = 6.00m, _2 = 8.00m,_X2 = 4.50m, _12 = 1.50m },
                         TopMatch = true
                     },
                     new Matches
                     {
                         HomeTeam = new Teams { Name = "Alaves", League = leagueSpain },
                         AwayTeam = new Teams { Name = "Girona", League = leagueSpain },
                         Types = new Types { _1 = 2.10m, _X = 3.50m, _2 = 3.40m, _1X = 1.50m, _X2 = 1.90m, _12 = 1.50m },
                         TopMatch = false
                     },
                      new Matches
                      {
                          HomeTeam = new Teams { Name = "Celta Vigo", League = leagueSpain },
                          AwayTeam = new Teams { Name = "Rayo Vallecano", League = leagueSpain },
                          Types = new Types { _1 = 1.30m, _X = 5.75m, _2 = 9.00m,_X2 = 6.00m, _12 = 1.40m },
                          TopMatch = false
                      },
                      new Matches
                      {
                          HomeTeam = new Teams { Name = "Huesca", League = leagueSpain },
                          AwayTeam = new Teams { Name = "Leganes", League = leagueSpain },
                          Types = new Types { _1 = 2.50m, _X = 3.50m, _2 = 2.60m, _1X = 1.60m, _X2 = 1.70m, _12 = 1.50m },
                          TopMatch = false
                      },
                       new Matches
                       {
                           HomeTeam = new Teams { Name = "Espanyol", League = leagueSpain },
                           AwayTeam = new Teams { Name = "Real Sociedad", League = leagueSpain },
                           Types = new Types { _1 = 1.95m, _X = 3.75m, _2 = 3.60m, _1X = 1.50M, _X2 = 2.70m, _12 = 1.50m },
                           TopMatch = false
                       },
                        new Matches
                        {
                            HomeTeam = new Teams { Name = "Roma", League = leagueItaly },
                            AwayTeam = new Teams { Name = "Parma", League = leagueItaly },
                            Types = new Types { _1 = 1.14m, _X = 9.00m, _2 = 15.00m,_X2 = 6.00m, _12 = 1.30m },
                            TopMatch = false
                        },
                         new Matches
                         {
                             HomeTeam = new Teams { Name = "Atalanta", League = leagueItaly },
                             AwayTeam = new Teams { Name = "Sassuolo", League = leagueItaly },
                             Types = new Types { _1 = 1.25m, _X = 6.00m, _2 = 10.00m, _X2 = 6.00m, _12 = 1.30m },
                             TopMatch = true
                         },
                         new Matches
                         {
                             HomeTeam = new Teams { Name = "Juventus", League = leagueItaly },
                             AwayTeam = new Teams { Name = "Frosinone", League = leagueItaly },
                             Types = new Types { _1 = 1.20m, _X = 3.75m, _2 = 7.50m, _X2 = 3.50m, _12 = 1.50m },
                             TopMatch = true
                         },
                         new Matches
                         {
                             HomeTeam = new Teams { Name = "Cagliari", League = leagueItaly },
                             AwayTeam = new Teams { Name = "Udinese", League = leagueItaly },
                             Types = new Types { _1 = 1.85m, _X = 3.75m, _2 = 4.00m, _1X = 1.30m, _X2 = 2.10m, _12 = 1.50m },
                             TopMatch = false
                         },
                         new Matches
                         {
                             HomeTeam = new Teams { Name = "Fiorentina", League = leagueItaly },
                             AwayTeam = new Teams { Name = "Genoa", League = leagueItaly },
                             Types = new Types { _1 = 2.25m, _X = 2.75m, _2 = 4.00m, _1X=1.50m, _X2 = 2.00m, _12 = 1.30m },
                             TopMatch = false
                         },

                        new Matches
                        {
                            HomeTeam = new Teams { Name = "Roma", League = leagueItaly },
                            AwayTeam = new Teams { Name = "Milan", League = leagueItaly },
                            Types = new Types { _1 = 3.00m, _X = 3.00m, _2 = 3.00m, _1X=1.40m, _X2 = 1.50m, _12 = 1.50m },
                            TopMatch = false
                        },
                         new Matches
                         {
                             HomeTeam = new Teams { Name = "Angers", League = leagueFrance },
                             AwayTeam = new Teams { Name = "Bordeaux", League = leagueFrance },
                             Types = new Types { _1 = 2.40m, _X = 3.00m, _2 = 3.00m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                             TopMatch = false
                         },
                          new Matches
                          {
                              HomeTeam = new Teams { Name = "Brest", League = leagueFrance },
                              AwayTeam = new Teams { Name = "Toulouse", League = leagueFrance },
                              Types = new Types { _1 = 2.30m, _X = 3.00m, _2 = 3.10m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                              TopMatch = false
                          },
                           new Matches
                           {
                               HomeTeam = new Teams { Name = "Dijon", League = leagueFrance },
                               AwayTeam = new Teams { Name = "St. Etienne", League = leagueFrance },
                               Types = new Types { _1 = 3.10m, _X = 3.10m, _2 = 2.20m, _1X = 1.70m, _X2 = 1.40m, _12 = 1.50m },
                               TopMatch = false
                           }
                    );
                context.SaveChanges();
                var sportTennis = context.Sports.SingleOrDefault(s => s.Name.Contains("Tennis"));
                context.TwoPlayersMatches.AddRange(
                    new TwoPlayersMatches
                    {
                        First = new Player { Name = "Querrey S.", Sport = sportTennis },
                        Second = new Player { Name = "Lajovic D.", Sport = sportTennis },
                        Sport = sportTennis,
                        _1 = 1.25m,
                        _2 = 3.75m,
                        TopMatch=true
                    },
                     new TwoPlayersMatches
                     {
                         First = new Player { Name = "Verdasco F.", Sport = sportTennis },
                         Second = new Player { Name = "Londero J. I.", Sport = sportTennis },
                         Sport = sportTennis,
                         _1 = 1.25m,
                         _2 = 3.75m,
                         TopMatch = true
                     },
                      new TwoPlayersMatches
                      {
                          First = new Player { Name = "Fabbiano T.", Sport = sportTennis },
                          Second = new Player { Name = "Djere L.", Sport = sportTennis },
                          Sport = sportTennis,
                          _1 = 1.50m,
                          _2 = 2.40m,
                          TopMatch = false
                      }, new TwoPlayersMatches
                      {
                          First = new Player { Name = "Herbert P.H.", Sport = sportTennis },
                          Second = new Player { Name = "Evans D.", Sport = sportTennis },
                          Sport = sportTennis,
                          _1 = 2.25m,
                          _2 = 1.60m,
                          TopMatch = false
                      },
                       new TwoPlayersMatches
                       {
                           First = new Player { Name = "Simon G.", Sport = sportTennis },
                           Second = new Player { Name = "Jarry N.", Sport = sportTennis },
                           Sport = sportTennis,
                           _1 = 1.50m,
                           _2 = 2.40m,
                           TopMatch = false
                       },
                        new TwoPlayersMatches
                        {
                            First = new Player { Name = "Edmund K.", Sport = sportTennis },
                            Second = new Player { Name = "Norrie C.", Sport = sportTennis },
                            Sport = sportTennis,
                            _1 = 1.50m,
                            _2 = 2.40m,
                            TopMatch = false
                        },
                         new TwoPlayersMatches
                         {
                             First = new Player { Name = "Johnson S.", Sport = sportTennis },
                             Second = new Player { Name = "Hurkacz H.   ", Sport = sportTennis },
                             Sport = sportTennis,
                             _1 = 1.50m,
                             _2 = 2.40m,
                             TopMatch = false
                         },
                          new TwoPlayersMatches
                          {
                              First = new Player { Name = "Pella G.", Sport = sportTennis },
                              Second = new Player { Name = "Fritz T.", Sport = sportTennis },
                              Sport = sportTennis,
                              _1 = 1.50m,
                              _2 = 2.40m,
                              TopMatch = false
                          },
                           new TwoPlayersMatches
                           {
                               First = new Player { Name = "Tomic B.", Sport = sportTennis },
                               Second = new Player { Name = "Gojowczyk P.", Sport = sportTennis },
                               Sport = sportTennis,
                               _1 = 1.65m,
                               _2 = 2.10m,
                               TopMatch = false
                           }
                    );
                context.SaveChanges();
            }
        }
    }
}