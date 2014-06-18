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
    public class EventSeriesController : ApiController
    {
        private SeriesRepository rep = new SeriesRepository();

        [GET("api/v1/events/{eventid:int}/series")]
        public IEnumerable<series> Get(int eventid)
        {
            IEnumerable<series> lst = rep.GetSeriesByEvent(eventid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/series/{seriesid:int}")]
        public series Get(int eventid, int seriesid)
        {
            var a = rep.GetByID(seriesid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("api/v1/events/{eventid:int}/series")]
        public series Post(int eventid, [FromBody]series value)
        {
            value.lsevent_id = eventid;
            rep.Insert(value);
            return value;
        }

        [PUT("api/v1/events/{eventid:int}/series/{seriesid:int}")]
        public void Put(int eventid, int seriesid, [FromBody]series value)
        {
            value.lsevent_id = eventid;
            value.id = seriesid;
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("api/v1/events/{eventid:int}/series/{seriesid:int}")]
        public void Delete(int eventid, int seriesid)
        {
            int rc = rep.Delete(seriesid);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}