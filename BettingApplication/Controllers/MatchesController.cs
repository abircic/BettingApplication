using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        // GET: Match
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _matchService.Index();
            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> IndexTwoPlayers()
        {
            var response = await _matchService.IndexTwoPlayers();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> TopMatches()
        {
            var response = await _matchService.TopMatches();
            return View(response);
        }

        // GET: Match/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HomeTeam,AwayTeam,ResultModel")] Match match)
        {
            if (ModelState.IsValid)
            {
                await _matchService.Create(match);
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Match/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _matchService.GetEdit(id);
            if (matches == null)
            {
                return NotFound();
            }
            return View(matches);
        }

        // POST: Match/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,HomeTeam,AwayTeam,ResultModel")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _matchService.PostEdit(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchesExists(match.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _matchService.GetDelete(id);

            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        // POST: test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var matches = await _matchService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MatchesExists(string id)
        {
            return _matchService.IsMatchExist(id);
        }
        public async Task<IActionResult> Add()
        {
            var response = await _matchService.GetAdd();

            return View(response);
        }

        // POST: Match/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string First, string Second, decimal _1, decimal _X, decimal _2, decimal _1X, decimal _X2, decimal _12, string league, bool topMatch, string time)
        {
            await _matchService.PostAdd(First, Second, _1, _X, _2, _1X, _X2, _12, league, topMatch, time);
            return RedirectToAction("Index", "Home");
        }
    }
}
