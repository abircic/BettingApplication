
using System;

namespace BettingApplication.Models
{
    public class TwoPlayersViewModel
    {
        public string Id { get; set; }

        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public DateTime Time { get; set; }
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public bool TopMatch { get; set; }
    }
}
