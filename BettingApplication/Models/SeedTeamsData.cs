using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BettingApplication.Models
{
    public class SeedTeamsData
    {
        public static void methodSeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
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
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
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
            using (var context = new BettingApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<BettingApplicationContext>>()))
            {

                if (context.Sports.Any())
                {
                    return; // DB has been seeded
                }

                context.AdminTopMatchConfigs.AddRange(
                    new AdminTopMatchConfig
                    {
                        MinimumNumberOfMatches = 5
                    });
                context.Sports.AddRange(
                    new Sports
                    {
                        Name = "Nogomet"
                    }
                    ,
                    new Sports
                    {
                        Name = "Tenis"
                    }
                    ,
                    new Sports
                    {
                        Name = "Košarka"
                    }
                );
                context.SaveChanges();
                var sportFootball = context.Sports.SingleOrDefault(s => s.Name.Contains("Nogomet"));
                var sportTennis = context.Sports.SingleOrDefault(s => s.Name.Contains("Tenis"));
                var sportBasketball = context.Sports.SingleOrDefault(s => s.Name.Contains("Košarka"));
                context.Leagues.AddRange(
                    new Leagues
                    {
                        Name = "Španjolska 1.",
                        Sport = sportFootball
                    },
                    new Leagues
                    {
                        Name = "Italija 1.",
                        Sport = sportFootball
                    },
                    new Leagues
                    {
                        Name = "Francuska 1.",
                        Sport = sportFootball
                    },
                    new Leagues
                    {
                        Name = "Engleska 1.",
                        Sport = sportFootball
                    }
                    ,
                    new Leagues
                    {
                        Name = "ATP",
                        Sport = sportTennis
                    }
                );
                context.SaveChanges();
                int year = 2020;
                int month = 1;
                int day = 1;
                int hour = 16;
                int minute = 00;
                int second = 00;
                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                var leagueSpainOne = context.Leagues.SingleOrDefault(l => l.Name.Contains("Španjolska 1."));
                var leagueItalyOne = context.Leagues.SingleOrDefault(l => l.Name.Contains("Italija 1."));
                var leagueFranceOne = context.Leagues.SingleOrDefault(l => l.Name.Contains("Francuska 1."));
                var leagueEnglandOne = context.Leagues.SingleOrDefault(l => l.Name.Contains("Engleska 1."));
                var leagueATP = context.Leagues.SingleOrDefault(l => l.Name.Contains("ATP"));
                context.Teams.AddRange(
                    new Teams { Name = "Brighton", League = leagueEnglandOne}, new Teams { Name = "Bournemouth", League = leagueEnglandOne},
                    new Teams { Name = "Newcastle Utd" , League = leagueEnglandOne}, new Teams { Name = "Everton", League = leagueEnglandOne},
                    new Teams { Name = "Southampton" , League = leagueEnglandOne}, new Teams { Name = "Crystal Palace", League = leagueEnglandOne},
                    new Teams { Name = "Watford", League = leagueEnglandOne}, new Teams { Name = "Aston Villa", League = leagueEnglandOne},
                    new Teams { Name = "Norwich", League = leagueEnglandOne}, new Teams { Name = "Tottenham", League = leagueEnglandOne},
                    new Teams { Name = "West Ham", League = leagueEnglandOne}, new Teams { Name = "Leicester", League = leagueEnglandOne},
                    new Teams { Name = "Burnley", League = leagueEnglandOne}, new Teams { Name = "Man Utd", League = leagueEnglandOne},
                    new Teams { Name = "Wolves", League = leagueEnglandOne}, new Teams { Name = "Man City", League = leagueEnglandOne},
                    new Teams { Name = "Arsenal", League = leagueEnglandOne}, new Teams { Name = "Chelsea", League = leagueEnglandOne},
                    new Teams { Name = " Liverpool", League = leagueEnglandOne}, new Teams { Name = "Sheffield Utd", League = leagueEnglandOne}
                    );
                context.Teams.AddRange(
                    new Teams { Name = "Brescia", League = leagueItalyOne}, new Teams { Name = "Lazio", League = leagueItalyOne},
                    new Teams { Name = "Spal", League = leagueItalyOne}, new Teams { Name = "Verona", League = leagueItalyOne},
                    new Teams { Name = "Genoa", League = leagueItalyOne}, new Teams { Name = "Sassuolo", League = leagueItalyOne},
                    new Teams { Name = "Roma", League = leagueItalyOne}, new Teams { Name = "Torino", League = leagueItalyOne},
                    new Teams { Name = "Bologna", League = leagueItalyOne}, new Teams { Name = "Fiorentina", League = leagueItalyOne},
                    new Teams { Name = "Atalanta", League = leagueItalyOne}, new Teams() { Name = "Parma", League = leagueItalyOne},
                    new Teams { Name = "Juventus", League =leagueItalyOne}, new Teams { Name = "Cagliari", League = leagueItalyOne},
                    new Teams { Name = "Milan", League = leagueItalyOne}, new Teams { Name = "Sampdoria", League = leagueItalyOne},
                    new Teams { Name = "Lecce", League = leagueItalyOne}, new Teams { Name = "Udinese", League = leagueItalyOne},
                    new Teams { Name = "Napoli", League = leagueItalyOne}, new Teams { Name = "Inter", League = leagueItalyOne}
                    );
                context.Teams.AddRange(
                    new Teams{ Name = "St.Rennes", League = leagueFranceOne}, new Teams { Name = "Marseille", League = leagueFranceOne},
                    new Teams { Name = "Bordeaux", League = leagueFranceOne}, new Teams { Name = "Lyon", League = leagueFranceOne },
                    new Teams { Name = "Toulouse", League = leagueFranceOne}, new Teams { Name = "Brest", League = leagueFranceOne},
                    new Teams { Name = "Amiens", League = leagueFranceOne}, new Teams { Name = "Montpellier", League = leagueFranceOne},
                    new Teams { Name = "Angers", League = leagueFranceOne}, new Teams { Name = "Nice", League = leagueFranceOne},
                    new Teams { Name = "Nimes", League = leagueFranceOne}, new Teams { Name = "Reims", League = leagueFranceOne},
                    new Teams { Name = "Metz", League = leagueFranceOne}, new Teams { Name = "Strasbourg", League = leagueFranceOne},
                    new Teams { Name = "Saint Etienne", League = leagueFranceOne}, new Teams { Name = "Nantes", League = leagueFranceOne},
                    new Teams { Name = "Dijon", League = leagueFranceOne}, new Teams { Name = "Lille", League = leagueFranceOne},
                    new Teams { Name = "Paris SG", League = leagueFranceOne}, new Teams { Name = "Monaco", League = leagueFranceOne}
                    );
                context.Teams.AddRange(
                    new Teams { Name = "Valladolid", League = leagueSpainOne}, new Teams { Name = "Leganés", League = leagueSpainOne },
                    new Teams { Name = "Sevilla", League = leagueSpainOne }, new Teams { Name = "Ath.Bilbao", League = leagueSpainOne },
                    new Teams { Name = "Valencia", League = leagueSpainOne }, new Teams { Name = "Eibar", League = leagueSpainOne },
                    new Teams { Name = "Getafe", League = leagueSpainOne }, new Teams { Name = "Real Madrid", League = leagueSpainOne },
                    new Teams { Name = "Atl.Madrid", League = leagueSpainOne }, new Teams { Name = "Levante", League = leagueSpainOne },
                    new Teams { Name = "Espanyol", League = leagueSpainOne }, new Teams { Name = "Barcelona", League = leagueSpainOne },
                    new Teams { Name = "Granada", League = leagueSpainOne }, new Teams { Name = "Mallorca", League = leagueSpainOne },
                    new Teams { Name = "Celta Vigo", League = leagueSpainOne }, new Teams { Name = "Osasuna", League = leagueSpainOne },
                    new Teams { Name = "Real Sociedad", League = leagueSpainOne }, new Teams { Name = "Villareal", League = leagueSpainOne },
                    new Teams { Name = "Alaves", League = leagueSpainOne }, new Teams { Name = "Betis", League = leagueSpainOne }
                    );
                context.SaveChanges();
            }
        }
    }
}
