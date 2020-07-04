using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Models;

namespace BettingApplication.Services.Interfaces
{
    public interface IResultService
    {
        Task<List<ResultModel>> GetResults();
        Task<List<ResultModel>> GetYesterdayResults();
        Task<List<Match>> GetMatch();
        Task<List<Match>> AddResult(string MatchId, string result);
        Task<List<Match>> GetYesterdayMatch();
        Task<List<Match>> AddYesterdayResult(string MatchId, string result);
    }
}
