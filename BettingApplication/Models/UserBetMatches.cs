
namespace BettingApplication.Models
{
    public class UserBetMatches
    {
        public int Id { get; set; }
        public virtual Matches Match { get; set; }
        public string Type { get; set; }
        public decimal Odd { get; set; }
        public string Win { get; set; }
        public string Result { get; set; }
        public virtual UserBets UserBets { get; set; }
    }
}
