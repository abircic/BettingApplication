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

                if (context.Sport.Any())
                {
                    return; // DB has been seeded
                }

                context.AdminTopMatchConfig.AddRange(
                    new AdminTopMatchConfig
                    {
                        MinimumNumberOfMatches = 5
                    });
                context.Sport.AddRange(
                    new Sport
                    {
                        Name = "Football"
                    }
                    ,
                    new Sport
                    {
                        Name = "Tenis"
                    }
                    ,
                    new Sport
                    {
                        Name = "Košarka"
                    }
                );
                context.SaveChanges();
                var sportFootball = context.Sport.SingleOrDefault(s => s.Name.Contains("Football"));
                var sportTennis = context.Sport.SingleOrDefault(s => s.Name.Contains("Tenis"));
                var sportBasketball = context.Sport.SingleOrDefault(s => s.Name.Contains("Košarka"));
                context.League.AddRange(
                    new League
                    {
                        Name = "Španjolska 1.",
                        Sport = sportFootball
                    },
                    new League
                    {
                        Name = "Italija 1.",
                        Sport = sportFootball
                    },
                    new League
                    {
                        Name = "Francuska 1.",
                        Sport = sportFootball
                    },
                    new League
                    {
                        Name = "Engleska 1.",
                        Sport = sportFootball
                    }
                    ,
                    new League
                    {
                        Name = "Njemačka 1.",
                        Sport = sportFootball
                    },
                    new League
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
                var LeaguepainOne = context.League.SingleOrDefault(l => l.Name.Contains("Španjolska 1."));
                var leagueItalyOne = context.League.SingleOrDefault(l => l.Name.Contains("Italija 1."));
                var leagueFranceOne = context.League.SingleOrDefault(l => l.Name.Contains("Francuska 1."));
                var leagueEnglandOne = context.League.SingleOrDefault(l => l.Name.Contains("Engleska 1."));
                var leagueGermanyOne = context.League.SingleOrDefault(l => l.Name.Contains("Njemačka 1."));
                var leagueATP = context.League.SingleOrDefault(l => l.Name.Contains("ATP"));
                context.Team.AddRange(
                    new Team { Name = "Brighton", League = leagueEnglandOne}, new Team { Name = "Bournemouth", League = leagueEnglandOne},
                    new Team { Name = "Newcastle Utd" , League = leagueEnglandOne}, new Team { Name = "Everton", League = leagueEnglandOne},
                    new Team { Name = "Southampton" , League = leagueEnglandOne}, new Team { Name = "Crystal Palace", League = leagueEnglandOne},
                    new Team { Name = "Watford", League = leagueEnglandOne}, new Team { Name = "Aston Villa", League = leagueEnglandOne},
                    new Team { Name = "Norwich", League = leagueEnglandOne}, new Team { Name = "Tottenham", League = leagueEnglandOne},
                    new Team { Name = "West Ham", League = leagueEnglandOne}, new Team { Name = "Leicester", League = leagueEnglandOne},
                    new Team { Name = "Burnley", League = leagueEnglandOne}, new Team { Name = "Man Utd", League = leagueEnglandOne},
                    new Team { Name = "Wolves", League = leagueEnglandOne}, new Team { Name = "Man City", League = leagueEnglandOne},
                    new Team { Name = "Arsenal", League = leagueEnglandOne}, new Team { Name = "Chelsea", League = leagueEnglandOne},
                    new Team { Name = " Liverpool", League = leagueEnglandOne}, new Team { Name = "Sheffield Utd", League = leagueEnglandOne}
                    );
                context.Team.AddRange(
                    new Team { Name = "Brescia", League = leagueItalyOne}, new Team { Name = "Lazio", League = leagueItalyOne},
                    new Team { Name = "Spal", League = leagueItalyOne}, new Team { Name = "Verona", League = leagueItalyOne},
                    new Team { Name = "Genoa", League = leagueItalyOne}, new Team { Name = "Sassuolo", League = leagueItalyOne},
                    new Team { Name = "Roma", League = leagueItalyOne}, new Team { Name = "Torino", League = leagueItalyOne},
                    new Team { Name = "Bologna", League = leagueItalyOne}, new Team { Name = "Fiorentina", League = leagueItalyOne},
                    new Team { Name = "Atalanta", League = leagueItalyOne}, new Team() { Name = "Parma", League = leagueItalyOne},
                    new Team { Name = "Juventus", League =leagueItalyOne}, new Team { Name = "Cagliari", League = leagueItalyOne},
                    new Team { Name = "Milan", League = leagueItalyOne}, new Team { Name = "Sampdoria", League = leagueItalyOne},
                    new Team { Name = "Lecce", League = leagueItalyOne}, new Team { Name = "Udinese", League = leagueItalyOne},
                    new Team { Name = "Napoli", League = leagueItalyOne}, new Team { Name = "Inter", League = leagueItalyOne}
                    );
                context.Team.AddRange(
                    new Team{ Name = "St.Rennes", League = leagueFranceOne}, new Team { Name = "Marseille", League = leagueFranceOne},
                    new Team { Name = "Bordeaux", League = leagueFranceOne}, new Team { Name = "Lyon", League = leagueFranceOne },
                    new Team { Name = "Toulouse", League = leagueFranceOne}, new Team { Name = "Brest", League = leagueFranceOne},
                    new Team { Name = "Amiens", League = leagueFranceOne}, new Team { Name = "Montpellier", League = leagueFranceOne},
                    new Team { Name = "Angers", League = leagueFranceOne}, new Team { Name = "Nice", League = leagueFranceOne},
                    new Team { Name = "Nimes", League = leagueFranceOne}, new Team { Name = "Reims", League = leagueFranceOne},
                    new Team { Name = "Metz", League = leagueFranceOne}, new Team { Name = "Strasbourg", League = leagueFranceOne},
                    new Team { Name = "Saint Etienne", League = leagueFranceOne}, new Team { Name = "Nantes", League = leagueFranceOne},
                    new Team { Name = "Dijon", League = leagueFranceOne}, new Team { Name = "Lille", League = leagueFranceOne},
                    new Team { Name = "Paris SG", League = leagueFranceOne}, new Team { Name = "Monaco", League = leagueFranceOne}
                    );
                context.Team.AddRange(
                    new Team { Name = "Valladolid", League = LeaguepainOne}, new Team { Name = "Leganés", League = LeaguepainOne },
                    new Team { Name = "Sevilla", League = LeaguepainOne }, new Team { Name = "Ath.Bilbao", League = LeaguepainOne },
                    new Team { Name = "Valencia", League = LeaguepainOne }, new Team { Name = "Eibar", League = LeaguepainOne },
                    new Team { Name = "Getafe", League = LeaguepainOne }, new Team { Name = "Real Madrid", League = LeaguepainOne },
                    new Team { Name = "Atl.Madrid", League = LeaguepainOne }, new Team { Name = "Levante", League = LeaguepainOne },
                    new Team { Name = "Espanyol", League = LeaguepainOne }, new Team { Name = "Barcelona", League = LeaguepainOne },
                    new Team { Name = "Granada", League = LeaguepainOne }, new Team { Name = "Mallorca", League = LeaguepainOne },
                    new Team { Name = "Celta Vigo", League = LeaguepainOne }, new Team { Name = "Osasuna", League = LeaguepainOne },
                    new Team { Name = "Real Sociedad", League = LeaguepainOne }, new Team { Name = "Villareal", League = LeaguepainOne },
                    new Team { Name = "Alaves", League = LeaguepainOne }, new Team { Name = "Betis", League = LeaguepainOne }
                    );
                context.Team.AddRange(
                    new Team { Name = "FC Augsburg", League = leagueGermanyOne }, new Team { Name = "B.Dortmund", League = leagueGermanyOne },
                    new Team { Name = "F.Düsseldorf", League = leagueGermanyOne }, new Team { Name = "Werder Bremen", League = leagueGermanyOne },
                    new Team { Name = "Köln", League = leagueGermanyOne }, new Team { Name = "Wolfsburg", League = leagueGermanyOne },
                    new Team { Name = "Hoffenheim", League = leagueGermanyOne }, new Team { Name = "Frankfurt", League = leagueGermanyOne },
                    new Team { Name = "Mainz", League = leagueGermanyOne }, new Team { Name = "Freiburg", League = leagueGermanyOne },
                    new Team { Name = "Leipzig", League = leagueGermanyOne }, new Team { Name = "Union Berlin", League = leagueGermanyOne },
                    new Team { Name = "Schalke", League = leagueGermanyOne }, new Team { Name = "B.M'gladbach", League = leagueGermanyOne },
                    new Team { Name = "Hertha", League = leagueGermanyOne }, new Team { Name = "Bayern München", League = leagueGermanyOne },
                    new Team { Name = "Paderborn", League = leagueGermanyOne }, new Team { Name = "B.Leverkusen", League = leagueGermanyOne }
                    );
                context.SaveChanges();
            }
        }
    }
}
