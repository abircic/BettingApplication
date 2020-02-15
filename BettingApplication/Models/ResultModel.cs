using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BettingApplication.Models
{
    public class ResultModel
    {
        public string Id { get; set; }
        public string Teams { get; set; }
        public string Time { get; set; }
        public string WinningTypes { get; set; }
        public string Result { get; set; }
        public string SportName { get; set; }
        public string LeagueName { get; set; }
        public string Date { get; set; }
    }
}
