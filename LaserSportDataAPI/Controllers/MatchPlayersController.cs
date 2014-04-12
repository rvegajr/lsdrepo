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
    public class MatchPlayersController : ApiController
    {
        MatchPlayersRepository rep = new MatchPlayersRepository();

        [GET("events/{eventid:int}/matches/{matchid:int}/players")]
        public IEnumerable<match_player> Get(int eventid, int matchid)
        {
            IEnumerable<match_player> lst = rep.GetPlayersByMatch(matchid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("events/{eventid:int}/matches/{matchid:int}/teams/{teamid:int}/players")]
        public IEnumerable<match_player> GetPlayersForTeam(int eventid, int matchid, int teamid)
        {
            IEnumerable<match_player> lst = rep.GetPlayersByMatchAndTeam(matchid, teamid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("events/{eventid:int}/matches/{matchid:int}/teams/{teamid:int}/players/{playerid:int}")]
        public match_player Get(int eventid, int matchid, int playerid)
        {
            var a = rep.Get(eventid, matchid, playerid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("events/{eventid:int}/matches/{matchid:int}/players")]
        public match_player Post(int eventid, int matchid, [FromBody]match_player value)
        {
            return rep.Insert(value);
        }

        [PUT("events/{eventid:int}/matches/{matchid:int}/players/{playerid:int}")]
        public void Put(int eventid, int matchid, int playerid, [FromBody]match_player value)
        {
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("events/{eventid:int}/matches/{matchid:int}/players/{playerid:int}")]
        public void Delete([FromBody]match_player value)
        {
            int rc = rep.Delete(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}