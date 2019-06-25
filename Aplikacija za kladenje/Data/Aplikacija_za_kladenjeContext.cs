using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aplikacija_za_kladenje.Models;

namespace Aplikacija_za_kladenje.Models
{
    public class Aplikacija_za_kladenjeContext : DbContext
    {
        public Aplikacija_za_kladenjeContext (DbContextOptions<Aplikacija_za_kladenjeContext> options)
            : base(options)
        {
        }

        public DbSet<Aplikacija_za_kladenje.Models.Wallet> Wallet { get; set; }
        public DbSet<Aplikacija_za_kladenje.Models.Teams> Teams { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.Leagues> Leagues { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.Matches> Matches { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.Sports> Sports { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.Types> Types { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.BetSlip> BetSlip { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.UserBets> UserBets { get; set; }

        public DbSet<Aplikacija_za_kladenje.Models.TwoPlayersMatches> TwoPlayersMatches { get; set; }
        public DbSet<Aplikacija_za_kladenje.Models.Player> Players { get; set; }
        public DbSet<Aplikacija_za_kladenje.Models.UserTransactions> UserTransactions { get; set; }

    }
}
