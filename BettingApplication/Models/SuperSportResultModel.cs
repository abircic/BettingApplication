using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BettingApplication.Models
{
    public class SuperSportResultModel
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

    public class SuperSportResultModelDto
    {
        [JsonProperty("n")]
        public string Teams { get; set; }
        [JsonProperty("v")]
        public string Time { get; set; }
        [JsonProperty("dt")]
        public string WinningTypes { get; set; }
        [JsonProperty("r")]
        public string Result { get; set; }
    }

    public class SuperSportLeagueResultsModelDto
    {
        [JsonProperty("n")]
        public string LeagueName { get; set; }
        [JsonProperty("p")]
        public SuperSportResultModelDto[] Results { get; set; }
    }

    public class SuperSportSportResultsModelDto
    {
        [JsonProperty("n")]
        public string SportName { get; set; }
        [JsonProperty("l")]
        public SuperSportLeagueResultsModelDto[] Leagues { get; set; }
    }

    public class SuperSportResultsModelDto
    {
        [JsonProperty("d")]
        public string Date { get; set; }
        [JsonProperty("s")]
        public SuperSportSportResultsModelDto[] Sports { get; set; }
    }
}
