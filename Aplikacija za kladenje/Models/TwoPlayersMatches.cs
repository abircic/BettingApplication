using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class TwoPlayersMatches
    {
        public string Id { get; set; }
        public virtual Player First { get; set; }
        public virtual Player Second { get; set; }
        public virtual Sports Sport { get; set; }
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public bool TopMatch { get; set; }
    }
}
