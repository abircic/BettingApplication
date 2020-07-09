
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BettingApplication.ViewModels;

namespace BettingApplication.Data
{
    public class BettingApplicationContext : IdentityDbContext<AppUser>
    {
        public BettingApplicationContext(DbContextOptions<BettingApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<League> League { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Sport> Sport { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<BetSlip> BetSlip { get; set; }
        public DbSet<UserBet> UserBet { get; set; }
        public DbSet<UserTransaction> UserTransaction { get; set; }
        public DbSet<UserBetMatch> UserBetMatch { get; set; }
        public DbSet<AdminTopMatchConfig> AdminTopMatchConfig { get; set; }
        public DbSet<ResultModel> Result { get; set; }
    }
}