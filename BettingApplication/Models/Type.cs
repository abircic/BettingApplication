﻿
using System.ComponentModel.DataAnnotations;

namespace BettingApplication.Models
{
    public class Type
    {
        public string Id { get; set; }
        [Display(Name = "1")]
        public decimal _1 {get; set;}
        [Display(Name = "X")]
        public decimal _X { get; set; }
        [Display(Name = "2")]
        public decimal _2 { get; set; }
        [Display(Name = "1X")]
        public decimal _1X { get; set; }
        [Display(Name = "X2")]
        public decimal _X2 { get; set; }
        [Display(Name = "12")]
        public decimal _12 { get; set; }
    }
}
