using System.Collections.Generic;

namespace BettingApplication.Models
{
    public class MatchesPartialView
    {
        public List<MatchViewModel> Matches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
