using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class Wallet
    {
        [Key]
        public string Userid { get; set; }
        public decimal Saldo { get; set; }
        
        public virtual ICollection<UserTransactions> Transactions { get; set; }
    }
}
