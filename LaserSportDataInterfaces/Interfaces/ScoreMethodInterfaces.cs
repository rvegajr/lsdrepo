using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

public interface IScoreMethod
{
    IEventSummary GetEventSummary(int lsevent_id, int series_id);
    IEventMatchSummary GetMatchSummary(int lsevent_id, int series_id);
    IMatchDetails GetMatchDetails(int lsevent_id, int series_id);
}

public interface IEventSummary : IDictionary<string, IEventSummarySeries>
{
}

public interface IEventSummarySeries : IList<IEventSummaryItem>
{
    string Series { get; set; }
}


public interface IEventSummaryItem
{
    int Rank { get; set; }
    string Team { get; set; }
    int GamesPlayed { get; set; }
    int TournamentPoints { get; set; }
    double TeamScore { get; set; }
}

public interface IEventMatchSummary : IDictionary<string, IMatchSummarySeries>
{
}

public interface IMatchSummarySeries : IDictionary<DateTime, IMatchSummary>
{
    string Series { get; set; }
}

public interface IMatchSummary : IList<IMatchSummaryItem>
{
    DateTime ScheduledDateTime { get; set; }
}


public interface IMatchSummaryItem
{
    int Rank { get; set; }
    string Team { get; set; }
    string Color { get; set; }
    int TeamScore { get; set; }
    int RankPoints { get; set; }
}
public interface IMatchDetails : IDictionary<string, IMatchDetailsSeries>
{
}

public interface IMatchDetailsSeries : IDictionary<DateTime, IMatchDetailItem>
{
    string Series { get; set; }
}

public interface IMatchDetailItem
{
    int Rank { get; set; }
    string Color { get; set; }
    string Team { get; set; }
    string PlayerName { get; set; }
    int Score { get; set; }
}