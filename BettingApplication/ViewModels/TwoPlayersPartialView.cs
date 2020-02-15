using System.Collections.Generic;
using BettingApplication.Models;

namespace BettingApplication.ViewModels
{
    public class TwoPlayersPartialView
    {
        public List<TwoPlayersViewModel> TwoPlayerMatches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
