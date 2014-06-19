using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSportDataObjects;
using RestSharp;
namespace LaserSportDataAPI.SystemObjects.A20XXv2
{
    public class ScoreMethod : IScoreMethod
    {
        public IEventSummary GetEventSummary(int lsevent_id, int series_id)
        {
            InitConn(lsevent_id);
            return GenEventSummary(lsevent_id, series_id);
        }

        public IEventMatchSummary GetMatchSummary(int lsevent_id, int series_id)
        {
            InitConn(lsevent_id);
            throw new NotImplementedException();
        }

        public IMatchDetails GetMatchDetails(int lsevent_id, int series_id)
        {
            InitConn(lsevent_id);
            throw new NotImplementedException();
        }

        private IEventSummary GenEventSummary(int lsevent_id, int series_id)
        {

            EventSummary evtsum = new EventSummary();
            EventSummarySeries series = new EventSummarySeries();
            series.Series = "event";
            series.Add(new EventSummaryItem() { Team = "Test Team", TeamScore = 999, TournamentPoints = 9, Rank = 1, GamesPlayed = 2 });
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


        private lsevent Event = null;
        private Dictionary<int, series> SeriesList = new Dictionary<int, series>();
        private Dictionary<int, packset> PackSetList = new Dictionary<int, packset>();
        private Dictionary<int, team> TeamList = new Dictionary<int, team>();
        private Dictionary<int, player> PlayerList = new Dictionary<int, player>();
        private Dictionary<string, match_team_player_score> ScoreList = new Dictionary<string, match_team_player_score>();
        private List<match> MatchList = new List<match>();
 
        protected bool InitConn(int eventId)
        {
            
            var client = new RestClient(this.Url);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var requestEvent = new RestRequest("api/v1/events/{event_id}", Method.GET);
            requestEvent.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<lsevent> responseEvent = client.Execute<lsevent>(requestEvent);
            Event = (lsevent)responseEvent.Data;

            var requestTeams = new RestRequest("api/v1/events/{event_id}/teams", Method.GET);
            requestTeams.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<team>> responseTeams = client.Execute<List<team>>(requestTeams);
            TeamList = (new CollectionUtilities<int, team>()).ListToDictionary((List<team>)responseTeams.Data, "id");

            var requestSeries = new RestRequest("api/v1/events/{event_id}/series", Method.GET);
            requestSeries.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<series>> responseSeries = client.Execute<List<series>>(requestSeries);
            SeriesList = (new CollectionUtilities<int, series>()).ListToDictionary((List<series>)responseSeries.Data, "id");

            var requestPlayers = new RestRequest("api/v1/events/{event_id}/players", Method.GET);
            requestPlayers.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<player>> responsePlayers = client.Execute<List<player>>(requestPlayers);
            PlayerList = (new CollectionUtilities<int, player>()).ListToDictionary((List<player>)responsePlayers.Data, "id");

            var requestPacksets = new RestRequest("api/v1/events/{event_id}/packsets", Method.GET);
            requestPacksets.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<packset>> responsePacksets = client.Execute<List<packset>>(requestPacksets);
            PackSetList = (new CollectionUtilities<int, packset>()).ListToDictionary((List<packset>)responsePacksets.Data, "id");

            var requestMatches = new RestRequest("api/v1/events/{event_id}/matches", Method.GET);
            requestMatches.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<match>> responseMatches = client.Execute<List<match>>(requestMatches);
            MatchList = (List<match>)responseMatches.Data;

            var requestScores = new RestRequest("api/v1/events/{event_id}/scores", Method.GET);
            requestScores.AddUrlSegment("event_id", eventId.ToString()); // replaces matching token in request.Resource
            IRestResponse<List<match_team_player_score>> responseScores = client.Execute<List<match_team_player_score>>(requestScores);
            ScoreList = (new CollectionUtilities<string, match_team_player_score>()).ListToDictionary((List<match_team_player_score>)responseScores.Data, "id");

            return true;
        }

        private RestSharp.RestClient client;
        private string url = "";
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
    }
}
