using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BettingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        [HttpGet]
        public bool GetResult(string homeTeam, string awayTeam, string type)
        {
            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                driver.Navigate().GoToUrl("https://www.psk.hr/Results/Sport?date=2019-12-15");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                List<IWebElement> elements = driver.FindElementsByClassName("result-row").ToList();
                Dictionary<string, string> results = new Dictionary<string, string>();
                foreach (IWebElement el in elements)
                {
                    results[el.FindElement(By.XPath(".//div[contains(@class, 'cell name')]//span")).Text] = el.FindElement(By.XPath(".//div[contains(@class, 'cell result')]//span")).Text;
                }
            }
            return true;
        }
    }
}