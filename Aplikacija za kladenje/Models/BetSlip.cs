using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class BetSlip
    {
        public int Id { get; set; }
        public float TotalOdd { get; set; }
        public Matches Matches { get; set; }
    }
}
