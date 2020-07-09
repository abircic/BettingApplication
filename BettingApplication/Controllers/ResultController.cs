using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using BettingApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BettingApplication.Controllers
{
    [AllowAnonymous]
    public class ResultController : Controller
    {
        private readonly BettingApplicationContext _context;
        private readonly IResultService _resultService;

        public ResultController(BettingApplicationContext context, IResultService resultService)
        {
            _context = context;
            _resultService = resultService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _resultService.GetResults();
            return View(response.OrderBy(l=>l.LeagueName));
        }
        public async Task<IActionResult> YesterdayResult()
        {
            var response = await _resultService.GetYesterdayResults();
            return View(response.OrderBy(l => l.LeagueName));
        }
       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddResult()
        {
            var response = await _resultService.GetMatch();
            return View(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("AddResult")]
        public async Task<IActionResult> AddResult(string MatchId, string result)
        {
            var response = await _resultService.AddResult(MatchId, result);

            return View(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddYesterdayResult()
        {
            var response = await _resultService.GetYesterdayMatch();

            return View(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("AddYesterdayResult")]
        public async Task<IActionResult> AddYesterdayResult(string MatchId, string result)
        {
            var response = await _resultService.AddYesterdayResult(MatchId, result);

            return View(response);
        }
    }
}