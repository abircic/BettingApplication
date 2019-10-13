
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Aplikacija_za_kladenje.Data
{
    public class Aplikacija_za_kladenjeContext : IdentityDbContext<AppUser>
    {
        public Aplikacija_za_kladenjeContext(DbContextOptions<Aplikacija_za_kladenjeContext> options)
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



    }
}