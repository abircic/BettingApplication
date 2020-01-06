using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PSKScraperTest.Services.Hosted;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PSKScraperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<PSKScraperService>();
                }).Build();
            host.Run();
        }
    }
}
