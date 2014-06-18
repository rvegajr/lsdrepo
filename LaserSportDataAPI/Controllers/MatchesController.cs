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
    public class MatchesController : ApiController
    {
        private MatchesRepository rep = new MatchesRepository();
        private MatchTeamsRepository repmt = new MatchTeamsRepository();

        [GET("api/v1/events/{eventid:int}/matches")]
        public IEnumerable<match> Get(int eventid)
        {
            IEnumerable<match> lst = rep.GetMatchesByEvent(eventid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/matches/{matchid:int}")]
        public match Get(int eventid, int matchid)
        {
            var a = rep.GetMatchByEvent(eventid, matchid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [GET("api/v1/events/{eventid:int}/matches/{matchid:int}/teams")]
        public IEnumerable<MatchTeam> GetMatchTeams(int eventid, int matchid)
        {
            IEnumerable<MatchTeam> lst = repmt.GetTeamsByMatch(matchid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/matches/{matchid:int}/teams/{teamid:int}")]
        public MatchTeam GetMatchTeam(int eventid, int matchid, int teamid)
        {
            return repmt.GetMatchTeamObject(matchid, teamid);
        }

        [POST("api/v1/events/{eventid:int}/matches")]
        public match Post(int eventid, [FromBody]match value)
        {
            value.lsevent_id = eventid;
            rep.Insert(value);
            return value;
        }

        [PUT("api/v1/events/{eventid:int}/matches/{matchid:int}")]
        public void Put(int eventid, int matchid, [FromBody]match value)
        {
            value.lsevent_id = eventid;
            value.id = matchid;
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("api/v1/events/{eventid:int}/matches/{matchid:int}")]
        public void Delete(int eventid, int matchid)
        {
            int rc = rep.Delete(matchid);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}