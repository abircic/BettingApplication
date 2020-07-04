
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BettingApplication.Models
{
    public class Match
    {
        public string Id { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }
        public DateTime Time { get; set; }
        public virtual Type Type { get; set; }
        public string Result { get; set; }
        public virtual Sport Sport { get; set; }
        public bool TopMatch { get; set; }
        public bool Hide { get; set; }
        public string Competition { get; set; }
    }
}
