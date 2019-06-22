using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class Matches
    {
        public string Id { get; set; }
        public virtual Teams HomeTeam { get; set; }
        public virtual Teams AwayTeam { get; set; }
        public virtual Types Types { get; set; }
        public string Result { get; set; }
        public virtual Sports Sport { get; set; }
        public Boolean TopMatch { get; set; }

    }
}
