using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Models
{
    public class Wallet
    {
        public string Id { get; set; }
        public decimal Saldo { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<UserTransaction> Transactions { get; set; }
    }
}
