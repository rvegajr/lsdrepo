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
    public class EventPlayersController : ApiController
    {
        EventPlayersRepository rep = new EventPlayersRepository();

        [GET("api/v1/events/{eventid:int}/players")]
        public IEnumerable<EventPlayer> Get(int eventid)
        {
            IEnumerable<EventPlayer> lst = rep.GetPlayersByEvent(eventid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/teams/{teamid:int}/players")]
        public IEnumerable<EventPlayer> GetPlayersForTeam(int eventid, int teamid)
        {
            IEnumerable<EventPlayer> lst = rep.GetPlayersByEventAndTeam(eventid, teamid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/players/{playerid:int}")]
        [GET("api/v1/events/{eventid:int}/teams/{teamid:int}/players/{playerid:int}")]
        public EventPlayer Get(int eventid, int playerid)
        {
            var a = rep.GetPlayerByEvent(eventid, playerid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("api/v1/events/{eventid:int}/players")]
        public lsevent_player Post([FromBody]lsevent_player value)
        {
            return rep.Insert(value);
        }

        [PUT("api/v1/events/{eventid:int}/players/{playerid:int}")]
        public void Put(int eventid, int playerid, [FromBody]lsevent_player value)
        {
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("api/v1/events/{eventid:int}/players/{playerid:int}")]
        public void Delete(int eventid, int playerid)
        {
            int rc = rep.Delete(playerid);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}