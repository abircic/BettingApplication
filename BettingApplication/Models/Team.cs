

namespace BettingApplication.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual League League { get; set; }

    }
}
