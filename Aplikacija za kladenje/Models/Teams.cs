using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplikacija_za_kladenje.Models
{
    public class Teams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Leagues League { get; set; }

    }
}
