
namespace BettingApplication.Models
{
    public class BetSlip
    {
        public int Id { get; set; }
        public virtual Matches Match { get; set; }
        public string Type { get; set; }
        public decimal Odd { get; set; }
        public bool TopMatch { get; set; }

        public virtual AppUser User { get; set; }

    }
}
