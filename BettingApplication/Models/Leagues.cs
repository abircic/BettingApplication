using System.Collections.Generic;

namespace BettingApplication.Models
{
    public class Leagues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Sports Sport { get; set; }
        public virtual ICollection<Teams> Team { get; set; }

    }
}
