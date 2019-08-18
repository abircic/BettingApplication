

namespace Aplikacija_za_kladenje.Models
{
    public class UserTransactions
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public string UserId { get; set; }
        public string Transactions { get; set; }
        public Wallet Wallet { get; set; }
    }
}
