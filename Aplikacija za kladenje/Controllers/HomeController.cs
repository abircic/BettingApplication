using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aplikacija_za_kladenje.Models;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;


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
            return View();
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
