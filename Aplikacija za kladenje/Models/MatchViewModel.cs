using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class MatchViewModel
    {
        public string Id { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string League { get; set; }
        public decimal _1 { get; set; }
        public decimal _X { get; set; }
        public decimal _2 { get; set; }
        public decimal _1X { get; set; }
        public decimal _X2 { get; set; }
        public decimal _12 { get; set; }
        public bool TopMatch { get; set; }
        

    }
}
