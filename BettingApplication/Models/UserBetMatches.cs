
namespace BettingApplication.Models
{
    public class UserBetMatches
    {
        public int Id { get; set; }
        public virtual Matches Match { get; set; }
        public string Type { get; set; }
        public decimal Odd { get; set; }
        public bool TopMatch { get; set; }
        public bool Result { get; set; }
        public virtual UserBets UserBets { get; set; }
    }
}
