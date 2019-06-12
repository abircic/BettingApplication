using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class Leagues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sports Sport { get; set; }
        public virtual ICollection<Teams> Teams{ get; set;}

    }
}
