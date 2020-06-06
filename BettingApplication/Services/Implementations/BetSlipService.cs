using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BettingApplication.Services.Implementations
{
    public class BetSlipService : IBetSlipService
    {
        private readonly BettingApplicationContext _context;

        public BetSlipService(BettingApplicationContext context)
        {
            _context = context;
        }

        public async Task<BetSlip> Details(string id)
        {
            return await _context.BetSlip.FirstOrDefaultAsync(m => m.Id == id);
        }
        
        public async Task<List<BetSlip>> GetUserBetSlip(string userId)
        {
            return  await _context.BetSlip.Where(x => x.User.Id == userId).ToListAsync();
        }

        public async Task<BetSlip> GetMatchFromBetSlip(string id)
        {
            var betSlip= await _context.BetSlip.Include(m => m.Match)
                .ThenInclude(h => h.HomeTeam).Include(m => m.Match)
                .ThenInclude(a => a.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betSlip != null)
            {
                _context.BetSlip.Remove(betSlip);
                await _context.SaveChangesAsync();
            }
            return betSlip;
        }

        public async Task DeleteMatchFromBetSlip(string id)
        {
            var betSlip = await _context.BetSlip.FindAsync(id);
            _context.BetSlip.Remove(betSlip);
            await _context.SaveChangesAsync();
        }
    }
}
