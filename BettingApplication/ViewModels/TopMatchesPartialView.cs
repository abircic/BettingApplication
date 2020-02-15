using System.Collections.Generic;
using BettingApplication.Models;

namespace BettingApplication.ViewModels
{
    public class TopMatchesPartialView
    {
        public List<TopMatchesViewModel> TopMatches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
