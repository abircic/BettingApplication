﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Models
{
    public class OfferModel
    { 
        public List<Locations> Locations { get; set; } 
        public Sport Sport { get; set; }
    }
    public class SportModel
    {
        public string Name { get; set; }
    }
    public class Locations
    {
        public string Name { get; set; }
        public List<LeagueModel> Leagues { get; set; }
    }

    public class LeagueModel
    {
        public string Name { get; set; }
        public List<EventDateGroups> EventDateGroups { get; set; }
    }

    public class EventDateGroups
    {
        public DateTimeOffset Date { get; set; }
        public List<Events> Events { get; set; }
    }

    public class Events
    {
        public string Id { get; set; }
        public FixtureModel Fixture { get; set; }
        public List<Markets> Markets { get; set; }
    }

    public class Markets
    {
        public string Name { get; set; }
        public List<Picks> Picks { get; set; }
    }

    public class Picks
    {
        public string Name { get; set; }
        public decimal Odds { get; set; }

    }

}
