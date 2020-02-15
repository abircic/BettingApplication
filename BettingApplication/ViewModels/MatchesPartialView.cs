using System.Collections.Generic;
using BettingApplication.Models;

namespace BettingApplication.ViewModels
{
    public class MatchesPartialView
    {
        public List<MatchViewModel> Matches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
