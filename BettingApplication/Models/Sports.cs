using System.Collections.Generic;

namespace BettingApplication.Models
{
    public class Sports
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Leagues> Leagues { get; set; }
    }
}
