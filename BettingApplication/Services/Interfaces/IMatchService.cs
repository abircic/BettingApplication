using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Models;
using BettingApplication.ViewModels;

namespace BettingApplication.Services.Interfaces
{
    public interface IMatchService
    {
        Task<List<MatchViewModel>> Index();
        Task<List<TwoPlayersViewModel>> IndexTwoPlayers();
        Task<List<TopMatchesViewModel>> TopMatches();
        Task Create(Match match);
        Task<Match> GetEdit(int? id);
        Task PostEdit(Match match);
        Task<Match> GetDelete(string id);
        Task<Match> DeleteConfirmed(string id);
        bool IsMatchExist(string id);
        Task<List<Team>> GetAdd();
        Task PostAdd(string First, string Second, decimal _1, decimal _X, decimal _2, decimal _1X,
            decimal _X2, decimal _12, string league, bool topMatch, string time);
        Task<int> GetTopMatchValue();
        Task<Match> GetMatch(string matchId);
    }
}
