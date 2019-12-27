
using System;

namespace BettingApplication.Models
{
    public class Matches
    {

        public string Id { get; set; }
        public virtual Teams HomeTeam { get; set; }
        public virtual Teams AwayTeam { get; set; }
        public DateTime Time { get; set; }
        public virtual Types Types { get; set; }
        public string Result { get; set; }
        public virtual Sports Sport { get; set; }
        public bool TopMatch { get; set; }

    }
}
