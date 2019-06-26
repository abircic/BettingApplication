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
        //[RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$")]
        public decimal Saldo { get; set; }
        //[DisplayFormat(DataFormatString = "{0.00}kn")]
        
        public virtual ICollection<UserTransactions> Transactions { get; set; }
    }
}
