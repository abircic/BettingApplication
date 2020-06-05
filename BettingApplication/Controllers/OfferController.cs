using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using BettingApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Type = BettingApplication.Models.Type;

namespace BettingApplication.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;


        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _offerService.GenerateOffer();
            return RedirectToAction("Index", "Home");
        }
    }
}
