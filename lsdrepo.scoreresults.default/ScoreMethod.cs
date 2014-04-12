using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSportDataAPI.ScoreMethod;
namespace LaserSportDataAPI.ScoreMethod.Sample
{
    public class ScoreMethod : IScoreMethod

    {
        public IEventSummary GetEventSummary(int lsevent_id, int series_id)
        {
            return GenSampleEvents(lsevent_id, series_id);
        }

        public IMatchSummary GetMatchSummary(int lsevent_id, int series_id)
        {
            throw new NotImplementedException();
        }

        public IMatchDetails GetMatchDetails(int lsevent_id, int series_id)
        {
            throw new NotImplementedException();
        }

        private IEventSummary GenSampleEvents(int lsevent_id, int series_id)
        {
            EventSummary evtsum = new EventSummary();
            EventSummarySeries series = new EventSummarySeries();
            series.Add(new EventSummaryItem() { Team = "Test Team", TeamScore = 999, TournamentPoints = 9, Rank = 1, GamesPlayed = 2, Series = "event" });
            series.Add(new EventSummaryItem() { Team = "Test Team b", TeamScore = 888, TournamentPoints = 8, Rank = 2, GamesPlayed = 2, Series = "event" });
            evtsum.Add(series);
            return evtsum;
        }
    }

    public class EventSummary : List<IEventSummarySeries>, IEventSummary 
    {
    }
    public class EventSummarySeries : List<EventSummaryItem>, IEventSummarySeries
    {
    }
    public class EventSummaryItem : IEventSummaryItem
    {
        public string Series {get; set;}

        public int Rank {get; set;}

        public string Team {get; set;}

        public int GamesPlayed {get; set;}

        public int TournamentPoints {get; set;}

        public double TeamScore {get; set;}
    }

}
