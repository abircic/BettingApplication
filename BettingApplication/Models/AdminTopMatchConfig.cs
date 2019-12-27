using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Models
{
    public class AdminTopMatchConfig
    {
        public int Id { get; set; }
        public int MinimumNumberOfMatches { get; set; }
    }
}
