using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSportDataObjects;
using LaserSportDataAPI.Models;
using RestSharp;

namespace LaserSportDataAPI.SystemObjects.A20XXv2
{
    public abstract class WebObjectResult : WebObject
    {
        public int? Rank { get; set; }
        public int? TournamentPoints { get; set; }
        public int? RankPeers { get; set; }  //Other rank that share this rank (used to check for ties)... if this is 1, then there is no tie, anything greater than 1 is a tie
    }
    public abstract class WebObject
    {
        public OnChangeDelegate OnChange { get; set; }
        public delegate void OnChangeDelegate(WebObject Sender, string fieldName, object oldFieldValue, object newFieldValue);
        protected void RaiseChangeEvent(string fieldName, object oldFieldValue, object newFieldValue)
        {
            this.IsDirty = true;
            if (this.OnChange != null)
            {
                this.OnChange(this, fieldName, oldFieldValue, newFieldValue);
            }
        }
        public void CopyTo<T>(T target) {
            this.CopyTo<T>(target);
        }
        public string URL { get; set; }
        public string URLMask { get; set; }
        public bool IsDirty { get; set; }
        public WebObject Parent { get; set; }
        public void Refresh(bool deep = false) 
        {
        }

        public RestSharp.RestClient client;
        public WebObject(WebObject parent) {
        }
        public WebObject() {
            this.Parent = null;
            this.IsDirty = false;
            this.URL = "";
            this.URLMask = "";
        }
    }
    public class LSEvent : WebObject, Ilsevent
    {
        public static LSEvent Get(string URL, int EventID) {
            LSEvent newEvent = new LSEvent();
            if (InitConn(EventID, URL))
            {
                Event.CopyTo<Ilsevent>(newEvent);
                foreach (series _seriesItem in SeriesList.Values)
                {
                    var newSeries = new LSSeries(_seriesItem);
                    var SeriesMatchList = MatchList.Where(s => s.series_id.Equals(newSeries.id));
                    foreach (match _matchItem in SeriesMatchList)
                    {
                        var newMatch = new LSMatch(_matchItem);
                        var MatchTeamList = MatchList.Where(s => s.series_id.Equals(newSeries.id));
                        var MatchScoreList = ScoreList.Values.Where(s => s.match_id.Equals(newMatch.id));

                        var requestMatches = new RestRequest("events/{event_id}/matches/{match_id}/players", Method.GET);
                        requestMatches.AddUrlSegment("event_id", EventID.ToString()); // replaces matching token in request.Resource
                        requestMatches.AddUrlSegment("match_id", newMatch.id.ToString()); // replaces matching token in request.Resource
                        IRestResponse<List<MatchPlayer>> responseMatches = client.Execute<List<MatchPlayer>>(requestMatches);
                        var PlayersInMatchList = (List<MatchPlayer>)responseMatches.Data;

                        foreach (match_team_player_score teamScore in MatchScoreList)
                        {
                            var team = new LSTeam(TeamList[teamScore.team_id], (teamScore.score_override != null ? teamScore.score_override : teamScore.player_score_sum));
                            var TeamPlayersInMatchList = PlayersInMatchList.Where(s => s.team_id.Equals(teamScore.team_id));
                            foreach (MatchPlayer matchPlayer in TeamPlayersInMatchList)
                            {
                                LSPlayer player = new LSPlayer();
                                player.code_name = matchPlayer.code_name;
                                player.id = matchPlayer.player_id;
                                player.first_name = matchPlayer.first_name;
                                player.last_name = matchPlayer.last_name;
                                player.player_score = matchPlayer.score;
                                //player.player_score_sum = 0;
                                team.Players.Add(player);
                            }
                            newMatch.Teams.Add(team);
                        }
                        newSeries.Matches.Add(AddRankToTeamsInMatch(newMatch));
                    }
                    newEvent.Series.Add(newSeries);
                }
                //((Ilsevent)Event).CopyTo<Ilsevent>(newEvent);
            }
            return newEvent;
        }
        protected static LSMatch AddRankToTeamsInMatch(LSMatch matchWithTeamsToRank)
        {
            var teamPlayersInMatchList =
                from aTeam in matchWithTeamsToRank.Teams
                orderby aTeam.team_score descending
                select aTeam;
            decimal? lastScore = 0.0M;
            int rank = 1;
            int count = 1;
            int rankPeers = 0;
            foreach (LSTeam aTeam in teamPlayersInMatchList)
            {
                if (!(lastScore.Equals(aTeam.team_score))) {
                    rank = count;
                    if (rankPeers > 1) { 
                        foreach (LSTeam aTeamSameScore in teamPlayersInMatchList.Where(s => s.team_score.Equals(lastScore)))
                        {
                            aTeamSameScore.RankPeers = rankPeers;
                        }
                    }
                    rankPeers = 0;
                }
                else
                {
                    rankPeers++;
                    lastScore = aTeam.team_score;
                }
                aTeam.Rank = rank;
                aTeam.RankPeers = rankPeers;
                count++;
            }
            //matchWithTeamsToRank.Teams = matchWithTeamsToRank.Teams.OrderByDescending(o => o.Rank);
            matchWithTeamsToRank.Teams.Sort(
                delegate(LSTeam p1, LSTeam p2)
                {
                    
                    return ((int)p1.Rank).CompareTo((int)p2.Rank);
                }
            );
            return matchWithTeamsToRank;
        }
        public List<LSSeries> Series { get; set; }
        public LSEvent()
        {
            Series = new List<LSSeries>();
        }
        protected static Ilsevent Event = null;
        protected static Dictionary<int, series> SeriesList = new Dictionary<int, series>();
        protected static Dictionary<int, packset> PackSetList = new Dictionary<int, packset>();
        protected static Dictionary<int, team> TeamList = new Dictionary<int, team>();
        protected static Dictionary<int, player> PlayerList = new Dictionary<int, player>();
        protected static Dictionary<string, match_team_player_score> ScoreList = new Dictionary<string, match_team_player_score>();
        protected static List<match> MatchList = new List<match>();
        private static RestClient client;
        protected static bool InitConn(int eventId, string URL)
        {

            client = new RestClient(URL);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var requestEvent = new RestRequest("events/{event_id}", Method.GET);
            requestEvent.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<lsevent> responseEvent = client.Execute<lsevent>(requestEvent);
            Event = (lsevent)responseEvent.Data;
            
            var requestTeams = new RestRequest("events/{event_id}/teams", Method.GET);
            requestTeams.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<team>> responseTeams = client.Execute<List<team>>(requestTeams);
            TeamList = (new CollectionUtilities<int, team>()).ListToDictionary((List<team>)responseTeams.Data, "id");

            var requestSeries = new RestRequest("events/{event_id}/series", Method.GET);
            requestSeries.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<series>> responseSeries = client.Execute<List<series>>(requestSeries);
            SeriesList = (new CollectionUtilities<int, series>()).ListToDictionary((List<series>)responseSeries.Data, "id");

            var requestPlayers = new RestRequest("events/{event_id}/players", Method.GET);
            requestPlayers.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<player>> responsePlayers = client.Execute<List<player>>(requestPlayers);
            PlayerList = (new CollectionUtilities<int, player>()).ListToDictionary((List<player>)responsePlayers.Data, "id");

            var requestPacksets = new RestRequest("events/{event_id}/packsets", Method.GET);
            requestPacksets.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<packset>> responsePacksets = client.Execute<List<packset>>(requestPacksets);
            PackSetList = (new CollectionUtilities<int, packset>()).ListToDictionary((List<packset>)responsePacksets.Data, "id");

            var requestMatches = new RestRequest("events/{event_id}/matches", Method.GET);
            requestMatches.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<match>> responseMatches = client.Execute<List<match>>(requestMatches);
            MatchList = (List<match>)responseMatches.Data;

            var requestScores = new RestRequest("events/{event_id}/scores", Method.GET);
            requestScores.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<match_team_player_score>> responseScores = client.Execute<List<match_team_player_score>>(requestScores);
            ScoreList = (new CollectionUtilities<string, match_team_player_score>()).ListToDictionary((List<match_team_player_score>)responseScores.Data, "id");
            
            return true;
        }

        public int id { get; set; }
        public string lsevent_name  { get; set; }
        public DateTime? scheduled  { get; set; }
        public DateTime? actual  { get; set; }
        public string edit_code  { get; set; }
        public int? score_method_id  { get; set; }
    }
    public class LSPlayer : WebObjectResult, Iplayer
    {
        public LSPlayer()
        {
        }
        public LSPlayer(Iplayer fromPlayer)
        {
            fromPlayer.CopyTo<Iplayer>(this);
        }
        public int id { get; set; }
        public string code_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public sbyte is_admin { get; set; }
        public decimal? player_score { get; set; }
        public decimal? player_score_sum { get; set; }

    }

    public class LSTeam : WebObjectResult, Iteam
    {
        public List<LSPlayer> Players { get; set; }
        public LSTeam()
        {
            Players = new List<LSPlayer>();
        }
        public LSTeam(Iteam fromTeam)
        {
            Players = new List<LSPlayer>();
            fromTeam.CopyTo<Iteam>(this);
        }
        public LSTeam(Iteam fromTeam, decimal? teamScore)
        {
            Players = new List<LSPlayer>();
            team_score = teamScore;
            fromTeam.CopyTo<Iteam>(this);
        }
        public int id { get; set; }
        public int lsevent_id { get; set; }
        public string team_name { get; set; }
        public decimal? team_score { get; set; }
    }

    public class LSMatch : WebObject, Imatch
    {
        public List<LSTeam> Teams { get; set; }
        public LSMatch()
        {
            Teams = new List<LSTeam>();
        }
        public LSMatch(Imatch fromMatch)
        {
            Teams = new List<LSTeam>();
            fromMatch.CopyTo<Imatch>(this);
        }

        public int id { get; set; }
        public int lsevent_id { get; set; }
        public string guid { get; set; }
        public DateTime? scheduled { get; set; }
        public DateTime? actual { get; set; }
        public int? system_id { get; set; }
        public int? series_id { get; set; }
    }

    public class LSSeries : WebObject, Iseries
    {
        public List<LSMatch> Matches { get; set; }
        public LSSeries()
        {
            Matches = new List<LSMatch>();
        }
        public LSSeries(Iseries fromSeries)
        {
            Matches = new List<LSMatch>();
            fromSeries.CopyTo<Iseries>(this);
        }

        public int id  { get; set; }
        public int lsevent_id  { get; set; }
        public string name  { get; set; }
    }

}
