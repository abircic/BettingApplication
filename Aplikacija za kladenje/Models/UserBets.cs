using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class UserBets
    {
        public int Id { get; set; }
        public BetSlip BetSlip { get; set; }
        public DateTime TimeStamp { get; set; }
        public float BetAmount { get; set; }
        public float TotalOdd { get; set; }
    }
}
