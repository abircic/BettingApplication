using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Models;

namespace BettingApplication.Services.Interfaces
{
    public interface IBetSlipService
    {
        Task<List<BetSlip>> GetUserBetSlip(string userId);
        Task<BetSlip> Details(string id);
        Task<BetSlip> GetMatchFromBetSlip(string id);
        Task DeleteMatchFromBetSlip(string id);
    }
}
