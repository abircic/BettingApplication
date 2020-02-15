
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BettingApplication.Data
{
    public class BettingApplicationContext : IdentityDbContext<AppUser>
    {
        public BettingApplicationContext(DbContextOptions<BettingApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Teams> Teams { get; set; }

        public DbSet<Leagues> Leagues { get; set; }

        public DbSet<Matches> Matches { get; set; }

        public DbSet<Sports> Sports { get; set; }

        public DbSet<Types> Types { get; set; }

        public DbSet<BetSlip> BetSlip { get; set; }

        public DbSet<UserBets> UserBets { get; set; }

        public DbSet<UserTransactions> UserTransactions { get; set; }
        public DbSet<UserBetMatches> UserBetMatches { get; set; }
        public DbSet<AdminTopMatchConfig> AdminTopMatchConfigs { get; set; }
        public DbSet<ResultModel> Results { get; set; }


    }
}