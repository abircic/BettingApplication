using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PSKScraperTest.Services.Hosted
{
    public class PSKScraperService : IHostedService
    {
        private static Timer _timer;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //string username = accs[(int)Math.Floor(new Random().NextDouble() * accs.Count())];
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                driver.Navigate().GoToUrl("https://www.psk.hr/Results/Sport?date=2019-12-19");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                List<IWebElement> elements = driver.FindElementsByClassName("result-row").ToList();
                Dictionary<string, string> results = new Dictionary<string, string>();
                foreach (IWebElement el in elements)
                {
                    results[el.FindElement(By.XPath(".//div[contains(@class, 'cell name')]//span")).Text] = el.FindElement(By.XPath(".//div[contains(@class, 'cell result')]//span")).Text;
                }

            }
        }
        
        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }

    }

    public class Result
    {
        public Result(string team1, string team2, int score1, int score2, string sport)
        {
            _teams = new string[] {team1, team2};
            _score = new int[] {score1, score2};
        }
        private string[] _teams;
        private int[] _score;
        

        public string[] Teams
        {
            get { return _teams; }
        }

        public int[] Score
        {
            get { return _score; }
        }

        public string[] GetWinningTypes()
        {
            return null;
        }
    }
}
