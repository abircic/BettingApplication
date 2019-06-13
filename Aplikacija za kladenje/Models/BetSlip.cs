using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class BetSlip
    {
        public int Id { get; set; }
        public virtual ICollection<UserBetMatchViewModel> Matches { get; set; }
        public decimal TotalOdd { get; set; }
        public decimal CashOut { get; set; }
        
    }
}
