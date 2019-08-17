using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aplikacija_za_kladenje.Models;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Aplikacija_za_kladenje.Data;


namespace Aplikacija_za_kladenje.Controllers
{
    public class HomeController : Controller
    {
        private readonly Aplikacija_za_kladenjeContext _context;
        public HomeController(Aplikacija_za_kladenjeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Matches> MatchesList = _context.Matches.Include(h => h.HomeTeam).ThenInclude(l => l.League).Include(a => a.AwayTeam).ThenInclude(l => l.League).Include(t => t.Types).ToList();

            MatchViewModel MatchVM = new MatchViewModel();

            List<MatchViewModel> MatchVMList = MatchesList.Select(x => new MatchViewModel
            {
                Id = x.Id,
                League = x.HomeTeam.League.Name,
                HomeTeamName = x.HomeTeam.Name,
                AwayTeamName = x.AwayTeam.Name,
                _1 = x.Types._1,
                _X = x.Types._X,
                _2 = x.Types._2,
                _1X = x.Types._1X,
                _X2 = x.Types._X2,
                _12 = x.Types._12,
            }).ToList();

            BetSlip BetSlip = new BetSlip();
            List<BetSlip> BetSlipList = _context.BetSlip.ToList();

            IndexVM model = new IndexVM();
            model.BetSlips = BetSlipList;
            model.Matches = MatchVMList;
            return View(model);
        }

        public IActionResult Wallets()
        {
            return View();
        }
        public IActionResult Types()
        {
            return View();
        }
        public IActionResult BetSlip()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
