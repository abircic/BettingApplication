using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class BetSlip
    {
        public int Id { get; set; }
        public string MatchId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Type { get; set; }
        public decimal Odd { get; set; }
        public Boolean TopMatch { get; set; }
       
        
    }
}
