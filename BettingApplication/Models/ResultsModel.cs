using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingApplication.Models
{
    public class ResultsModel
    {
        public FixtureModel Fixture { get; set; }
        public List<ScoreModel> Scores { get; set; }
    }

    public class FixtureModel
    {
        public string EventId { get; set; }
        public List<ParticipantsModel> Participants { get; set; }
        public ResultSportModel Sport { get; set; }
        public ResultLeagueModel League { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }

    public class ScoreModel
    {
        public string Position { get; set; }
        public string Value { get; set; }
    }

    public class Periods
    {
        public int Type { get; set; }
        public string TypeName { get; set; }
        public bool IsFinished { get; set; }
        public bool IsConfirmed { get; set; }
        public List<ScoreModel> Scores { get; set; }
    }
    public class ParticipantsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }

    public class ResultSportModel
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
    }
    public class ResultLeagueModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long Order { get; set; }
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        public long LocationOrder { get; set; }
    }
}
