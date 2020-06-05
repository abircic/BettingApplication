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
               
                if (context.AdminTopMatchConfig.Any())
                {
                    return;   // DB has been seeded
                }
                context.AdminTopMatchConfig.AddRange(
                   new AdminTopMatchConfig
                   {
                       MinimumNumberOfMatches = 5
                   });
                //context.ResultSportModel.AddRange(
                //    new ResultSportModel
                //    {
                //        Name = "Football"
                //    }
                //    ,
                //    new ResultSportModel
                //    {
                //        Name = "Tennis"
                //    }
                //);
                //context.SaveChanges();
                //var sportFootball = context.ResultSportModel.SingleOrDefault(s => s.Name.Contains("Football"));
                //var sportTennis = context.ResultSportModel.SingleOrDefault(s => s.Name.Contains("Tennis"));
                //context.ResultLeagueModel.AddRange(
                //    new ResultLeagueModel
                //    {
                //        Name = "Spain La Liga",
                //        ResultSportModel = sportFootball
                //    },
                //     new ResultLeagueModel
                //     {
                //         Name = "Italy Serie A",
                //         ResultSportModel = sportFootball
                //     },
                //     new ResultLeagueModel
                //     {
                //         Name = "France Ligue 1",
                //         ResultSportModel = sportFootball
                //     },
                //      new ResultLeagueModel
                //      {
                //          Name = "England Premier ResultLeagueModel",
                //          ResultSportModel = sportFootball
                //      }
                //      ,
                //      new ResultLeagueModel
                //      {
                //          Name = "ATP",
                //          ResultSportModel = sportTennis
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
                //var Leaguepain = context.ResultLeagueModel.SingleOrDefault(l => l.Name.Contains("Spain"));
                //var leagueItaly = context.ResultLeagueModel.SingleOrDefault(l => l.Name.Contains("Italy"));
                //var leagueFrance = context.ResultLeagueModel.SingleOrDefault(l => l.Name.Contains("France"));
                //var leagueEngland = context.ResultLeagueModel.SingleOrDefault(l => l.Name.Contains("England"));
                //var leagueATP = context.ResultLeagueModel.SingleOrDefault(l => l.Name.Contains("ATP"));
                //context.Match.AddRange(
                //    new Match
                //    {
                //        HomeTeam = new Team { Name = "Liverpool", ResultLeagueModel = leagueEngland },
                //        AwayTeam = new Team { Name = "Norwich", ResultLeagueModel = leagueEngland },
                //        Time = dateTime,
                //Type = new Type { _1 = 1.14m,_X = 6.00m, _2 = 17.00m, _X2 = 11.00m, _12 = 1.30m },
                //        TopMatch = true,
                //        ResultSportModel=sportFootball
                //    },
                //    new Match
                //    {
                //        HomeTeam = new Team { Name = "West Ham", ResultLeagueModel = leagueEngland },
                //        AwayTeam = new Team { Name = "Man City", ResultLeagueModel = leagueEngland },
                //        Time = dateTime,
                //        Type = new Type { _1 =11.00m, _X = 6.00m, _2 = 1.30m, _1X = 5.00m, _12 = 1.30m },
                //        TopMatch = true,
                //        ResultSportModel = sportFootball
                //    },
                //    new Match
                //    {
                //        HomeTeam = new Team { Name = "Bournemouth", ResultLeagueModel = leagueEngland },
                //        AwayTeam = new Team { Name = "Sheffield Utd", ResultLeagueModel = leagueEngland },
                //        Time = dateTime,
                //        Type = new Type { _1 = 1.85m, _X = 3.80m, _2 = 3.80m, _1X = 1.25m, _X2=1.80M,_12 = 1.30m },
                //        TopMatch = false,
                //        ResultSportModel = sportFootball
                //    },
                //    new Match
                //    {
                //        HomeTeam = new Team { Name = "Burnley", ResultLeagueModel = leagueEngland },
                //        AwayTeam = new Team { Name = "Southampton", ResultLeagueModel = leagueEngland },
                //        Time = dateTime,
                //        Type = new Type { _1 = 2.55m, _X = 3.20m, _2 = 2.80m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                //        TopMatch = false,
                //        ResultSportModel = sportFootball
                //    },
                //     new Match
                //     {
                //         HomeTeam = new Team { Name = "Crystal Palace", ResultLeagueModel = leagueEngland },
                //         AwayTeam = new Team { Name = "Everton", ResultLeagueModel = leagueEngland },
                //         Time = dateTime,
                //         Type = new Type { _1 = 2.50m, _X = 3.30m, _2 = 2.75m, _1X = 1.80m, _X2 = 1.90M, _12 = 1.50m },
                //         TopMatch = false,
                //         ResultSportModel = sportFootball
                //     },
                //      new Match
                //      {
                //          HomeTeam = new Team { Name = "Watford", ResultLeagueModel = leagueEngland },
                //          AwayTeam = new Team { Name = "Brighton", ResultLeagueModel = leagueEngland },
                //          Time = dateTime,
                //          Type = new Type { _1 = 1.90m, _X = 3.40m, _2 = 4.00m, _1X = 1.35m, _X2 = 1.90M, _12 = 1.50m },
                //          TopMatch = false,
                //          ResultSportModel = sportFootball
                //      },
                //      new Match
                //      {
                //          HomeTeam = new Team { Name = "Tottenham", ResultLeagueModel = leagueEngland },
                //          AwayTeam = new Team { Name = "Aston Villa", ResultLeagueModel = leagueEngland },
                //          Time = dateTime,
                //          Type = new Type { _1 = 1.25m, _X = 5.75m, _2 = 13.00m, _X2 = 4.00m, _12 = 1.50m },
                //          TopMatch = false,
                //          ResultSportModel = sportFootball
                //      },
                //    new Match
                //    {
                //        HomeTeam = new Team { Name = "Eibar", ResultLeagueModel = Leaguepain },
                //        AwayTeam = new Team { Name = "Barcelona", ResultLeagueModel = Leaguepain },
                //        Time = dateTime,
                //        Type = new Type { _1 = 4.75m, _X = 4.10m, _2 = 1.70m, _1X = 2.50m, _X2 = 1.20m, _12 = 1.50m },
                //        TopMatch= false,
                //        ResultSportModel = sportFootball
                //    },
                //     new Match
                //     {
                //         HomeTeam = new Team { Name = "Real Madrid", ResultLeagueModel = Leaguepain },
                //         AwayTeam = new Team { Name = "Betis", ResultLeagueModel = Leaguepain },
                //         Time = dateTime,
                //         Type = new Type { _1 = 1.30m, _X = 6.00m, _2 = 8.00m,_X2 = 4.50m, _12 = 1.50m },
                //         TopMatch = true,
                //         ResultSportModel = sportFootball
                //     },
                //     new Match
                //     {
                //         HomeTeam = new Team { Name = "Alaves", ResultLeagueModel = Leaguepain },
                //         AwayTeam = new Team { Name = "Girona", ResultLeagueModel = Leaguepain },
                //         Time = dateTime,
                //         Type = new Type { _1 = 2.10m, _X = 3.50m, _2 = 3.40m, _1X = 1.50m, _X2 = 1.90m, _12 = 1.50m },
                //         TopMatch = false,
                //         ResultSportModel = sportFootball
                //     },
                //      new Match
                //      {
                //          HomeTeam = new Team { Name = "Celta Vigo", ResultLeagueModel = Leaguepain },
                //          AwayTeam = new Team { Name = "Rayo Vallecano", ResultLeagueModel = Leaguepain },
                //          Time = dateTime,
                //          Type = new Type { _1 = 1.30m, _X = 5.75m, _2 = 9.00m,_X2 = 6.00m, _12 = 1.40m },
                //          TopMatch = false,
                //          ResultSportModel = sportFootball
                //      },
                //      new Match
                //      {
                //          HomeTeam = new Team { Name = "Huesca", ResultLeagueModel = Leaguepain },
                //          AwayTeam = new Team { Name = "Leganes", ResultLeagueModel = Leaguepain },
                //          Time = dateTime,
                //          Type = new Type { _1 = 2.50m, _X = 3.50m, _2 = 2.60m, _1X = 1.60m, _X2 = 1.70m, _12 = 1.50m },
                //          TopMatch = false,
                //          ResultSportModel = sportFootball
                //      },
                //       new Match
                //       {
                //           HomeTeam = new Team { Name = "Espanyol", ResultLeagueModel = Leaguepain },
                //           AwayTeam = new Team { Name = "Real Sociedad", ResultLeagueModel = Leaguepain },
                //           Time = dateTime,
                //           Type = new Type { _1 = 1.95m, _X = 3.75m, _2 = 3.60m, _1X = 1.50M, _X2 = 2.70m, _12 = 1.50m },
                //           TopMatch = false,
                //           ResultSportModel = sportFootball
                //       },
                //        new Match
                //        {
                //            HomeTeam = new Team { Name = "Roma", ResultLeagueModel = leagueItaly },
                //            AwayTeam = new Team { Name = "Parma", ResultLeagueModel = leagueItaly },
                //            Time = dateTime,
                //            Type = new Type { _1 = 1.14m, _X = 9.00m, _2 = 15.00m,_X2 = 6.00m, _12 = 1.30m },
                //            TopMatch = false,
                //            ResultSportModel = sportFootball
                //        },
                //         new Match
                //         {
                //             HomeTeam = new Team { Name = "Atalanta", ResultLeagueModel = leagueItaly },
                //             AwayTeam = new Team { Name = "Sassuolo", ResultLeagueModel = leagueItaly },
                //             Time = dateTime,
                //             Type = new Type { _1 = 1.25m, _X = 6.00m, _2 = 10.00m, _X2 = 6.00m, _12 = 1.30m },
                //             TopMatch = true,
                //             ResultSportModel = sportFootball
                //         },
                //         new Match
                //         {
                //             HomeTeam = new Team { Name = "Juventus", ResultLeagueModel = leagueItaly },
                //             AwayTeam = new Team { Name = "Frosinone", ResultLeagueModel = leagueItaly },
                //             Time = dateTime,
                //             Type = new Type { _1 = 1.20m, _X = 3.75m, _2 = 7.50m, _X2 = 3.50m, _12 = 1.50m },
                //             TopMatch = true,
                //             ResultSportModel = sportFootball
                //         },
                //         new Match
                //         {
                //             HomeTeam = new Team { Name = "Cagliari", ResultLeagueModel = leagueItaly },
                //             AwayTeam = new Team { Name = "Udinese", ResultLeagueModel = leagueItaly },
                //             Time = dateTime,
                //             Type = new Type { _1 = 1.85m, _X = 3.75m, _2 = 4.00m, _1X = 1.30m, _X2 = 2.10m, _12 = 1.50m },
                //             TopMatch = false,
                //             ResultSportModel = sportFootball
                //         },
                //         new Match
                //         {
                //             HomeTeam = new Team { Name = "Fiorentina", ResultLeagueModel = leagueItaly },
                //             AwayTeam = new Team { Name = "Genoa", ResultLeagueModel = leagueItaly },
                //             Time = dateTime,
                //             Type = new Type { _1 = 2.25m, _X = 2.75m, _2 = 4.00m, _1X=1.50m, _X2 = 2.00m, _12 = 1.30m },
                //             TopMatch = false,
                //             ResultSportModel = sportFootball
                //         },

                //        new Match
                //        {
                //            HomeTeam = new Team { Name = "Roma", ResultLeagueModel = leagueItaly },
                //            AwayTeam = new Team { Name = "Milan", ResultLeagueModel = leagueItaly },
                //            Time = dateTime,
                //            Type = new Type { _1 = 3.00m, _X = 3.00m, _2 = 3.00m, _1X=1.40m, _X2 = 1.50m, _12 = 1.50m },
                //            TopMatch = false,
                //            ResultSportModel = sportFootball
                //        },
                //         new Match
                //         {
                //             HomeTeam = new Team { Name = "Angers", ResultLeagueModel = leagueFrance },
                //             AwayTeam = new Team { Name = "Bordeaux", ResultLeagueModel = leagueFrance },
                //             Time = dateTime,
                //             Type = new Type { _1 = 2.40m, _X = 3.00m, _2 = 3.00m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                //             TopMatch = false,
                //             ResultSportModel = sportFootball
                //         },
                //          new Match
                //          {
                //              HomeTeam = new Team { Name = "Brest", ResultLeagueModel = leagueFrance },
                //              AwayTeam = new Team { Name = "Toulouse", ResultLeagueModel = leagueFrance },
                //              Time = dateTime,
                //              Type = new Type { _1 = 2.30m, _X = 3.00m, _2 = 3.10m, _1X = 1.50m, _X2 = 1.70m, _12 = 1.50m },
                //              TopMatch = false,
                //              ResultSportModel = sportFootball
                //          },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Dijon", ResultLeagueModel = leagueFrance },
                //               AwayTeam = new Team { Name = "St. Etienne", ResultLeagueModel = leagueFrance },
                //               Time = dateTime,
                //               Type = new Type { _1 = 3.10m, _X = 3.10m, _2 = 2.20m, _1X = 1.70m, _X2 = 1.40m, _12 = 1.50m },
                //               TopMatch = false,
                //               ResultSportModel = sportFootball
                //           },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Querrey S.", ResultLeagueModel = leagueATP },
                //               AwayTeam = new Team { Name = "Lajovic D.", ResultLeagueModel = leagueATP },
                //               Time = dateTime,
                //               Type = new Type { _1 = 1.25m, _2 = 3.75m},
                //               ResultSportModel = sportTennis,
                //               TopMatch = true
                //           },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Verdasco F.", ResultLeagueModel = leagueATP },
                //               AwayTeam = new Team { Name = "Londero J. I.", ResultLeagueModel = leagueATP },
                //               Time = dateTime,
                //               Type = new Type { _1 = 1.25m, _2 = 3.75m },
                //               ResultSportModel = sportTennis,
                //               TopMatch = true
                //           },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Fabbiano T.", ResultLeagueModel = leagueATP },
                //               AwayTeam = new Team {  Name = "Djere L.", ResultLeagueModel = leagueATP },
                //               Time = dateTime,
                //               Type = new Type { _1 = 1.50m, _2 = 2.40m },
                //               ResultSportModel = sportTennis,
                //               TopMatch = false
                //           },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Herbert P.H.", ResultLeagueModel = leagueATP },
                //               AwayTeam = new Team { Name = "Evans D.", ResultLeagueModel = leagueATP },
                //               Time = dateTime,
                //               Type = new Type { _1 = 2.25m, _2 = 1.60m },
                //               ResultSportModel = sportTennis,
                //               TopMatch = false
                //           },
                //           new Match
                //           {
                //               HomeTeam = new Team { Name = "Simon G.", ResultLeagueModel = leagueATP },
                //               AwayTeam = new Team { Name = "Jarry N.", ResultLeagueModel = leagueATP },
                //               Time = dateTime,
                //               Type = new Type { _1 = 1.50m, _2 = 2.40m, },
                //               ResultSportModel = sportTennis,
                //               TopMatch = false
                //           },
                //            new Match
                //            {
                //                HomeTeam = new Team { Name = "Edmund K.", ResultLeagueModel = leagueATP },
                //                AwayTeam = new Team { Name = "Norrie C.", ResultLeagueModel = leagueATP },
                //                Time = dateTime,
                //                Type = new Type { _1 = 1.50m, _2 = 2.40m, },
                //                ResultSportModel = sportTennis,
                //                TopMatch = false
                //            },
                //            new Match
                //            {
                //                HomeTeam = new Team { Name = "Johnson S.", ResultLeagueModel = leagueATP },
                //                AwayTeam = new Team { Name = "Hurkacz H.", ResultLeagueModel = leagueATP },
                //                Time = dateTime,
                //                Type = new Type { _1 = 1.50m, _2 = 2.40m, },
                //                ResultSportModel = sportTennis,
                //                TopMatch = false
                //            },
                //             new Match
                //             {
                //                 HomeTeam = new Team { Name = "Pella G.", ResultLeagueModel = leagueATP },
                //                 AwayTeam = new Team { Name = "Fritz T.", ResultLeagueModel = leagueATP },
                //                 Time = dateTime,
                //                 Type = new Type { _1 = 1.50m, _2 = 2.40m, },
                //                 ResultSportModel = sportTennis,
                //                 TopMatch = false
                //             },
                //             new Match
                //             {
                //                 HomeTeam = new Team { Name = "Tomic B.", ResultLeagueModel = leagueATP },
                //                 AwayTeam = new Team { Name = "Gojowczyk P.", ResultLeagueModel = leagueATP },
                //                 Time = dateTime,
                //                 Type = new Type { _1 = 1.65m, _2 = 2.10m, },
                //                 ResultSportModel = sportTennis,
                //                 TopMatch = false
                //             }
                //    );
                context.SaveChanges();
            }
        }
    }
}