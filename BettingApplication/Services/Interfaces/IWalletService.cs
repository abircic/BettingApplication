using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Models;

namespace BettingApplication.Services.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> GetWallet(string userId);
        Task CashIn(decimal walletStake, string stake, string userId);
        Task CashOut(string userId, string stake);
        List<UserTransaction> GetUserTransactions(string userId);
    }
}
