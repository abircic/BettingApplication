using System.Collections.Generic;

namespace Aplikacija_za_kladenje.Models
{
    public class MatchesPartialView
    {
        public List<MatchViewModel> Matches { get; set; }
        public List<BetSlip> BetSlip { get; set; }
    }
}
