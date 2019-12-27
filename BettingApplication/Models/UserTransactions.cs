

namespace BettingApplication.Models
{
    public class UserTransactions
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public string UserId { get; set; }
        public string Transactions { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
