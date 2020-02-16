using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BettingApplication.Data;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace BettingApplication.Models
{
    public static class SeedData
    {
        public static void methodSeedData(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                AppUser user = new AppUser();
                user.FirstName = "Admin";
                user.LastName = "Admin";
                user.Age = 18;
                user.UserName = "Admin";
                user.EmailConfirmed = true;
                IdentityResult result = userManager.CreateAsync(user, "Adminpass1").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                    var wallet = new Wallet();
                    wallet.User = user;
                    wallet.Saldo = 9999999;
                }
            }
        }
        public static void InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new BettingApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BettingApplicationContext>>()))
            {
               
                if (context.AdminTopMatchConfigs.Any())
                {
                    return;   // DB has been seeded
                }
                context.AdminTopMatchConfigs.AddRange(
                   new AdminTopMatchConfig
                   {
                       MinimumNumberOfMatches = 5
                   });
                //context.Sports.AddRange(
                //    new Sports
                //    {
                //        Name = "Football"
                //    }
                //    ,
                //    new Sports
                //    {
                //        Name = "Tennis"
                //    }
                //);
                //context.SaveChanges();
                //var sportFootball = context.Sports.SingleOrDefault(s => s.Name.Contains("Football"));
                //var sportTennis = context.Sports.SingleOrDefault(s => s.Name.Contains("Tennis"));
                //context.Leagues.AddRange(
                //    new Leagues
                //    {
                //        Name = "Spain La Liga",
                //        Sport = sportFootball
                //    },
                //     new Leagues
                //     {
                //         Name = "Italy Serie A",
                //         Sport = sportFootball
                //     },
                //     new Leagues
                //     {
                //         Name = "France Ligue 1",
                //         Sport = sportFootball
                //     },
                //      new Leagues
                //      {
                //          Name = "England Premier League",
                //          Sport = sportFootball
                //      }
                //      ,
                //      new Leagues
                //      {
                //          Name = "ATP",
                //          Sport = sportTennis
                //      }
                //);
                //context.SaveChanges();
                //int year = 2020;
                //int month = 1;
                //int day = 1;
                //int hour = 16;
                //int minute = 00;
                //int second = 00;
                //DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                //var leagueSpain = context.Leagues.SingleOrDefault(l => l.Name.Contains("Spain"));
                //var leagueItaly = context.Leagues.SingleOrDefault(l => l.Name.Contains("Italy"));
                //var leagueFrance = context.Leagues.SingleOrDefault(l => l.Name.Contains("France"));
                //var leagueEngland = context.Leagues.SingleOrDefault(l => l.Name.Contains("England"));
                //var leagueATP = context.Leagues.SingleOrDefault(l => l.Name.Contains("ATP"));
                //context.Matches.AddRange(
                //    new Matches
                //    {
                //        HomeTeam = new Teams { Name = "Liverpool", League = leagueEngland },
                //        AwayTeam = new Teams { Name = "Norwich", League = leagueEngland },
                //        Time = dateTime,
                //Types = new Types { _1 = 1.14m,_X = 6.00m, _2 = 17.00m, _X2 = 11.00m, _12 = 1.30m },
                //        TopMatch = true,
                //        Sport=sportFootball
                //    },
                //    new Matches
                //    {
                //        HomeTeam = new Teams { Name = "West Ham", League = leagueEngland },
                //        AwayTeam = new Teams { Name = "Man City", League = leagueEngland },
                //        Time = dateTime,
                //        Types = new Types { _1 =11.00m, _X = 6.00m, _2 = 1.30m, _1X = 5.00m, _12 = 1.30m },
                //        TopMatch = true,
                //        Sport = sportFootball
                //    },
                //    new Matches
                //    {
                //        HomeTeam = new Teams { Name = "Bournemouth", League = leagueEngland },
                //        AwayTeam = new Teams { Name = "Sheffield Utd", League = leagueEngland },
                //        Time = dateTime,
                //        Types = new Types { _1 = 1.85m, _X = 3.80m, _2 = 3.80m, _1X = 1.25m, _X2=1.80M,_12 = 1.30m },
                //        TopMatch = false,
                //        Sport = sportFootball
                //    },
                //    new Matches
                //    {
                //        HomeTeam = new Teams { Name = "Burnley", League = leagueEngland },
                //        AwayTeam = new Teams { Name = "Southampton", League = leagueEngland },
                //        Time = dateTime,
                //        Types = new Types { _1 = 2.55m, _X = 3.20m, _2 = 2.80m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                //        TopMatch = false,
                //        Sport = sportFootball
                //    },
                //     new Matches
                //     {
                //         HomeTeam = new Teams { Name = "Crystal Palace", League = leagueEngland },
                //         AwayTeam = new Teams { Name = "Everton", League = leagueEngland },
                //         Time = dateTime,
                //         Types = new Types { _1 = 2.50m, _X = 3.30m, _2 = 2.75m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                //         TopMatch = false,
                //         Sport = sportFootball
                //     },
                //      new Matches
                //      {
                //          HomeTeam = new Teams { Name = "Watford", League = leagueEngland },
                //          AwayTeam = new Teams { Name = "Brighton", League = leagueEngland },
                //          Time = dateTime,
                //          Types = new Types { _1 = 1.90m, _X = 3.40m, _2 = 4.00m, _1X = 1.35m, _X2 = 1.90M, _12 = 1.50m },
                //          TopMatch = false,
                //          Sport = sportFootball
                //      },
                //      new Matches
                //      {
                //          HomeTeam = new Teams { Name = "Tottenham", League = leagueEngland },
                //          AwayTeam = new Teams { Name = "Aston Villa", League = leagueEngland },
                //          Time = dateTime,
                //          Types = new Types { _1 = 1.25m, _X = 5.75m, _2 = 13.00m, _X2 = 4.00m, _12 = 1.50m },
                //          TopMatch = false,
                //          Sport = sportFootball
                //      },
                //    new Matches
                //    {
                //        HomeTeam = new Teams { Name = "Eibar", League = leagueSpain },
                //        AwayTeam = new Teams { Name = "Barcelona", League = leagueSpain },
                //        Time = dateTime,
                //        Types = new Types { _1 = 4.75m, _X = 4.10m, _2 = 1.70m, _1X = 2.50m, _X2 = 1.20m, _12 = 1.50m },
                //        TopMatch= false,
                //        Sport = sportFootball
                //    },
                //     new Matches
                //     {
                //         HomeTeam = new Teams { Name = "Real Madrid", League = leagueSpain },
                //         AwayTeam = new Teams { Name = "Betis", League = leagueSpain },
                //         Time = dateTime,
                //         Types = new Types { _1 = 1.30m, _X = 6.00m, _2 = 8.00m,_X2 = 4.50m, _12 = 1.50m },
                //         TopMatch = true,
                //         Sport = sportFootball
                //     },
                //     new Matches
                //     {
                //         HomeTeam = new Teams { Name = "Alaves", League = leagueSpain },
                //         AwayTeam = new Teams { Name = "Girona", League = leagueSpain },
                //         Time = dateTime,
                //         Types = new Types { _1 = 2.10m, _X = 3.50m, _2 = 3.40m, _1X = 1.50m, _X2 = 1.90m, _12 = 1.50m },
                //         TopMatch = false,
                //         Sport = sportFootball
                //     },
                //      new Matches
                //      {
                //          HomeTeam = new Teams { Name = "Celta Vigo", League = leagueSpain },
                //          AwayTeam = new Teams { Name = "Rayo Vallecano", League = leagueSpain },
                //          Time = dateTime,
                //          Types = new Types { _1 = 1.30m, _X = 5.75m, _2 = 9.00m,_X2 = 6.00m, _12 = 1.40m },
                //          TopMatch = false,
                //          Sport = sportFootball
                //      },
                //      new Matches
                //      {
                //          HomeTeam = new Teams { Name = "Huesca", League = leagueSpain },
                //          AwayTeam = new Teams { Name = "Leganes", League = leagueSpain },
                //          Time = dateTime,
                //          Types = new Types { _1 = 2.50m, _X = 3.50m, _2 = 2.60m, _1X = 1.60m, _X2 = 1.70m, _12 = 1.50m },
                //          TopMatch = false,
                //          Sport = sportFootball
                //      },
                //       new Matches
                //       {
                //           HomeTeam = new Teams { Name = "Espanyol", League = leagueSpain },
                //           AwayTeam = new Teams { Name = "Real Sociedad", League = leagueSpain },
                //           Time = dateTime,
                //           Types = new Types { _1 = 1.95m, _X = 3.75m, _2 = 3.60m, _1X = 1.50M, _X2 = 2.70m, _12 = 1.50m },
                //           TopMatch = false,
                //           Sport = sportFootball
                //       },
                //        new Matches
                //        {
                //            HomeTeam = new Teams { Name = "Roma", League = leagueItaly },
                //            AwayTeam = new Teams { Name = "Parma", League = leagueItaly },
                //            Time = dateTime,
                //            Types = new Types { _1 = 1.14m, _X = 9.00m, _2 = 15.00m,_X2 = 6.00m, _12 = 1.30m },
                //            TopMatch = false,
                //            Sport = sportFootball
                //        },
                //         new Matches
                //         {
                //             HomeTeam = new Teams { Name = "Atalanta", League = leagueItaly },
                //             AwayTeam = new Teams { Name = "Sassuolo", League = leagueItaly },
                //             Time = dateTime,
                //             Types = new Types { _1 = 1.25m, _X = 6.00m, _2 = 10.00m, _X2 = 6.00m, _12 = 1.30m },
                //             TopMatch = true,
                //             Sport = sportFootball
                //         },
                //         new Matches
                //         {
                //             HomeTeam = new Teams { Name = "Juventus", League = leagueItaly },
                //             AwayTeam = new Teams { Name = "Frosinone", League = leagueItaly },
                //             Time = dateTime,
                //             Types = new Types { _1 = 1.20m, _X = 3.75m, _2 = 7.50m, _X2 = 3.50m, _12 = 1.50m },
                //             TopMatch = true,
                //             Sport = sportFootball
                //         },
                //         new Matches
                //         {
                //             HomeTeam = new Teams { Name = "Cagliari", League = leagueItaly },
                //             AwayTeam = new Teams { Name = "Udinese", League = leagueItaly },
                //             Time = dateTime,
                //             Types = new Types { _1 = 1.85m, _X = 3.75m, _2 = 4.00m, _1X = 1.30m, _X2 = 2.10m, _12 = 1.50m },
                //             TopMatch = false,
                //             Sport = sportFootball
                //         },
                //         new Matches
                //         {
                //             HomeTeam = new Teams { Name = "Fiorentina", League = leagueItaly },
                //             AwayTeam = new Teams { Name = "Genoa", League = leagueItaly },
                //             Time = dateTime,
                //             Types = new Types { _1 = 2.25m, _X = 2.75m, _2 = 4.00m, _1X=1.50m, _X2 = 2.00m, _12 = 1.30m },
                //             TopMatch = false,
                //             Sport = sportFootball
                //         },

                //        new Matches
                //        {
                //            HomeTeam = new Teams { Name = "Roma", League = leagueItaly },
                //            AwayTeam = new Teams { Name = "Milan", League = leagueItaly },
                //            Time = dateTime,
                //            Types = new Types { _1 = 3.00m, _X = 3.00m, _2 = 3.00m, _1X=1.40m, _X2 = 1.50m, _12 = 1.50m },
                //            TopMatch = false,
                //            Sport = sportFootball
                //        },
                //         new Matches
                //         {
                //             HomeTeam = new Teams { Name = "Angers", League = leagueFrance },
                //             AwayTeam = new Teams { Name = "Bordeaux", League = leagueFrance },
                //             Time = dateTime,
                //             Types = new Types { _1 = 2.40m, _X = 3.00m, _2 = 3.00m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                //             TopMatch = false,
                //             Sport = sportFootball
                //         },
                //          new Matches
                //          {
                //              HomeTeam = new Teams { Name = "Brest", League = leagueFrance },
                //              AwayTeam = new Teams { Name = "Toulouse", League = leagueFrance },
                //              Time = dateTime,
                //              Types = new Types { _1 = 2.30m, _X = 3.00m, _2 = 3.10m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                //              TopMatch = false,
                //              Sport = sportFootball
                //          },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Dijon", League = leagueFrance },
                //               AwayTeam = new Teams { Name = "St. Etienne", League = leagueFrance },
                //               Time = dateTime,
                //               Types = new Types { _1 = 3.10m, _X = 3.10m, _2 = 2.20m, _1X = 1.70m, _X2 = 1.40m, _12 = 1.50m },
                //               TopMatch = false,
                //               Sport = sportFootball
                //           },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Querrey S.", League = leagueATP },
                //               AwayTeam = new Teams { Name = "Lajovic D.", League = leagueATP },
                //               Time = dateTime,
                //               Types = new Types { _1 = 1.25m, _2 = 3.75m},
                //               Sport = sportTennis,
                //               TopMatch = true
                //           },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Verdasco F.", League = leagueATP },
                //               AwayTeam = new Teams { Name = "Londero J. I.", League = leagueATP },
                //               Time = dateTime,
                //               Types = new Types { _1 = 1.25m, _2 = 3.75m },
                //               Sport = sportTennis,
                //               TopMatch = true
                //           },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Fabbiano T.", League = leagueATP },
                //               AwayTeam = new Teams {  Name = "Djere L.", League = leagueATP },
                //               Time = dateTime,
                //               Types = new Types { _1 = 1.50m, _2 = 2.40m },
                //               Sport = sportTennis,
                //               TopMatch = false
                //           },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Herbert P.H.", League = leagueATP },
                //               AwayTeam = new Teams { Name = "Evans D.", League = leagueATP },
                //               Time = dateTime,
                //               Types = new Types { _1 = 2.25m, _2 = 1.60m },
                //               Sport = sportTennis,
                //               TopMatch = false
                //           },
                //           new Matches
                //           {
                //               HomeTeam = new Teams { Name = "Simon G.", League = leagueATP },
                //               AwayTeam = new Teams { Name = "Jarry N.", League = leagueATP },
                //               Time = dateTime,
                //               Types = new Types { _1 = 1.50m, _2 = 2.40m, },
                //               Sport = sportTennis,
                //               TopMatch = false
                //           },
                //            new Matches
                //            {
                //                HomeTeam = new Teams { Name = "Edmund K.", League = leagueATP },
                //                AwayTeam = new Teams { Name = "Norrie C.", League = leagueATP },
                //                Time = dateTime,
                //                Types = new Types { _1 = 1.50m, _2 = 2.40m, },
                //                Sport = sportTennis,
                //                TopMatch = false
                //            },
                //            new Matches
                //            {
                //                HomeTeam = new Teams { Name = "Johnson S.", League = leagueATP },
                //                AwayTeam = new Teams { Name = "Hurkacz H.", League = leagueATP },
                //                Time = dateTime,
                //                Types = new Types { _1 = 1.50m, _2 = 2.40m, },
                //                Sport = sportTennis,
                //                TopMatch = false
                //            },
                //             new Matches
                //             {
                //                 HomeTeam = new Teams { Name = "Pella G.", League = leagueATP },
                //                 AwayTeam = new Teams { Name = "Fritz T.", League = leagueATP },
                //                 Time = dateTime,
                //                 Types = new Types { _1 = 1.50m, _2 = 2.40m, },
                //                 Sport = sportTennis,
                //                 TopMatch = false
                //             },
                //             new Matches
                //             {
                //                 HomeTeam = new Teams { Name = "Tomic B.", League = leagueATP },
                //                 AwayTeam = new Teams { Name = "Gojowczyk P.", League = leagueATP },
                //                 Time = dateTime,
                //                 Types = new Types { _1 = 1.65m, _2 = 2.10m, },
                //                 Sport = sportTennis,
                //                 TopMatch = false
                //             }
                //    );
                context.SaveChanges();
            }
        }
    }
}