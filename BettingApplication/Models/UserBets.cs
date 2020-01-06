using System;
using System.Collections.Generic;

namespace BettingApplication.Models
{
    public class UserBets
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal BetAmount { get; set; }
        public decimal TotalOdd { get; set; }
        public decimal CashOut { get; set; }
        public virtual ICollection<UserBetMatches> BetMatches { get; set; }
        public virtual Matches Match {get; set;}
        public virtual AppUser User { get; set; }
        public string Win { get; set; }
    }
}
