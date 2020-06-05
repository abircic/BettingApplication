using System.Collections.Generic;

namespace BettingApplication.Models
{
    public class League
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<Team> Team { get; set; }

    }
}
