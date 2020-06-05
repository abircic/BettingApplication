
namespace BettingApplication.Models
{
    public class UserBetMatch
    {
        public string Id { get; set; }
        public virtual Match Match { get; set; }
        public string Type { get; set; }
        public decimal Odd { get; set; }
        public string Win { get; set; }
        public string Result { get; set; }
        public virtual UserBet UserBet { get; set; }
    }
}
