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
    public class EventTeamsController : ApiController
    {
        private TeamsRepository rep = new TeamsRepository();

        [GET("events/{eventid:int}/teams")]
        public IEnumerable<team> Get(int eventid)
        {
            IEnumerable<team> lst = rep.GetTeamsByEvent(eventid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("events/{eventid:int}/teams/{teamid:int}")]
        public team Get(int eventid, int teamid)
        {
            var a = rep.GetTeamByEvent(eventid, teamid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("events/{eventid:int}/teams")]
        public team Post(int eventid, [FromBody]team value)
        {
            value.lsevent_id = eventid;
            rep.Insert(value);
            return value;
        }

        [PUT("events/{eventid:int}/teams/{teamid:int}")]
        public void Put(int eventid, int teamid, [FromBody]team value)
        {
            value.lsevent_id = eventid;
            value.id = teamid;
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("events/{eventid:int}/teams/{teamid:int}")]
        public void Delete(int eventid, int teamid)
        {
            int rc = rep.Delete(teamid);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}