using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using System.Globalization;
using BettingApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BettingApplication.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BettingApplication.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly BettingApplicationContext _context;
        private readonly IAccountService _accountService;

        public WalletService(BettingApplicationContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<Wallet> GetWallet(string userId)
        {
            return await _context.Wallet.Where(x => x.User.Id == userId).FirstOrDefaultAsync();
        }

        public async Task CashIn(decimal walletStake, string stake, string userId)
        {
            var user = await _accountService.GetUserById(userId);
            var wallet = await GetWallet(userId);

            UserTransaction transaction = new UserTransaction();
            List<UserTransaction> listTransactions = new List<UserTransaction>();
            wallet.Saldo += walletStake;
            transaction.UserId = wallet.User.Id;
            transaction.Payment = stake;//check stake i walletstake
            transaction.Transactions = "Uplata u iznosu od " + stake + " kn " + " " + DateTime.Now;
            listTransactions.Add(transaction);
            wallet.Transactions = listTransactions;
            await _context.SaveChangesAsync();
        }
        public async Task CashOut(string userId, string stake)
        {
            var user = await _accountService.GetUserById(userId);
            var wallet = await GetWallet(userId);

            UserTransaction transaction = new UserTransaction();
            List<UserTransaction> listTransactions = new List<UserTransaction>();
            transaction.UserId = userId;
            transaction.Payment = stake;
            transaction.Transactions = "Isplata u iznosu od " + stake + " kn " + " " + DateTime.Now;
            listTransactions.Add(transaction);
            wallet.Transactions = listTransactions;
            await _context.SaveChangesAsync();
        }

        public List<UserTransaction> GetUserTransactions(string userId)
        {
            return _context.UserTransaction.Where(x => x.UserId == userId).ToList();
        }
    }
}
