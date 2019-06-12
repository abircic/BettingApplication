using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class Sports
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Leagues> Leagues { get; set; }
    }
}
