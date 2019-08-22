using System;
using System.Collections.Generic;

namespace Aplikacija_za_kladenje.Models
{
    public class UserBets
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal BetAmount { get; set; }
        public decimal TotalOdd { get; set; }
        public decimal CashOut { get; set; }
        public virtual ICollection<UserBetMatches> Matches { get; set; }
        public  Wallet wallet;
    }
}
