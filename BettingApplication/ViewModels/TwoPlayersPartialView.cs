using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Models
{
    public class TwoPlayersPartialView
    {
        public List<TwoPlayersViewModel> TwoPlayerMatches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
