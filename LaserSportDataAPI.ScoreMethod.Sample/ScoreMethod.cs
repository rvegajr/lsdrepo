using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LaserSportDataAPI.ScoreMethod.Sample
{
    public class ScoreMethod : IScoreMethod
    {
        public IEventSummary GetEventSummary(int lsevent_id, int series_id)
        {
            return GenSampleEvents(lsevent_id, series_id);
        }

        public IEventMatchSummary GetMatchSummary(int lsevent_id, int series_id)
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
            series.Series = "event";
            series.Add(new EventSummaryItem() { Team = "Test Team", TeamScore = 999, TournamentPoints = 9, Rank = 1, GamesPlayed = 2});
            series.Add(new EventSummaryItem() { Team = "Test Team b", TeamScore = 888, TournamentPoints = 8, Rank = 2, GamesPlayed = 2 });
            evtsum.Add("event", series);
            return evtsum;
        }
        private IEventMatchSummary GenMatchEventSummary(int lsevent_id, int series_id)
        {
            EventMatchSummary mtcsum = new EventMatchSummary();
            MatchSummarySeries series = new MatchSummarySeries();
            MatchSummary match = new MatchSummary();
            match.Add(new MatchSummaryItem() { Team = "Test Team a", TeamScore = 999, Rank = 1, RankPoints = 10, Color = "Green" });
            match.Add(new MatchSummaryItem() { Team = "Test Team b", TeamScore = 888, Rank = 2, RankPoints = 1, Color = "Red" });
            series.Add(DateTime.Now, match);

            match.Add(new MatchSummaryItem() { Team = "Test Team b", TeamScore = 1001, Rank = 1, RankPoints = 10, Color = "Green" });
            match.Add(new MatchSummaryItem() { Team = "Test Team a", TeamScore = 1000, Rank = 2, RankPoints = 1, Color = "Red" });
            series.Add(DateTime.Now.AddHours(1), match);

            mtcsum.Add("event", series);
            return mtcsum;
        }

    }
}
