using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class TwoPlayersViewModel
    {
        public string Id { get; set; }

        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }

        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public bool TopMatch { get; set; }
    }
}
