using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class IndexVM
    {
        public List<MatchViewModel> Matches { get; set; }
        public List<BetSlip> BetSlips { get; set; }
    }
}
