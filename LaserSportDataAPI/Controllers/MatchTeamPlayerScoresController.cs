using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;
using AttributeRouting.Web.Http;
using LaserSportDataObjects;
using LaserSportDataAPI.Models;


namespace LaserSportDataAPI.Controllers
{
    public class MatchTeamsPlayerScoresController : ApiController
    {
        MatchTeamPlayerScoresRepository rep = new MatchTeamPlayerScoresRepository();

        [GET("events/{event_id:int}/scores")]
        public IEnumerable<match_team_player_score> Get(int event_id)
        {
            var lst = rep.GetScoresByEvent(event_id);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("scores/{id}")]
        public match_team_player_score Get(string id)
        {
            var a = rep.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [GET("events/{event_id:int}/matches/{match_id:int}/scores")]
        public IEnumerable<match_team_player_score> Get(int event_id, int match_id)
        {
            var lst = rep.GetScoresByMatch(event_id, match_id);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("events/{event_id:int}/series/{series_id:int}/scores")]
        public IEnumerable<match_team_player_score> GetSeriesScores(int event_id, int series_id)
        {
            var lst = rep.GetScoresByEventSeries(event_id, series_id);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }


        [GET("events/{event_id:int}/matches/{match_id:int}/teams/{team_id:int}/scores")]
        public IEnumerable<match_team_player_score> Get(int event_id, int match_id, int team_id)
        {
            var lst = rep.GetScoresByMatchTeam(event_id, match_id, team_id);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }


    }
}
