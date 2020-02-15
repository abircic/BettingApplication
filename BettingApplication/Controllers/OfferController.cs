using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BettingApplication.Data;
using BettingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BettingApplication.Controllers
{
    public class OfferController : Controller
    {
        private readonly BettingApplicationContext _context;

        public OfferController(BettingApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var date = DateTime.Now;
            string url = $"https://sportdataprovider.volcanobet.me/api/public/prematch/SportEvents?SportId=1&from=2020-02-14T23:00:00.000Z&to=2020-02-16T06:00:00.000Z&timezone=-60&clientType=WebConsumer&v=1.1.435&lang=sr-Latn-ME";
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            var offer = JsonConvert.DeserializeObject<OfferModel>(html);
            foreach (var match in offer.Locations)
            {
                var matchModel = new Matches()
                {

                };
            }
            return View();
        }
    }
}