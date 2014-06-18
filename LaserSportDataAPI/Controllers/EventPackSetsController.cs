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
    public class EventPacksetsController : ApiController
    {
        private PacksetsRepository rep = new PacksetsRepository();

        [GET("api/v1/events/{eventid:int}/packsets")]
        public IEnumerable<packset> Get(int eventid)
        {
            IEnumerable<packset> lst = rep.GetPacksetsByEvent(eventid);
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{eventid:int}/packsets/{packsetid:int}")]
        public packset Get(int eventid, int packsetid)
        {
            var a = rep.GetByID(packsetid);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("api/v1/events/{eventid:int}/packsets")]
        public packset Post(int eventid, [FromBody]packset value)
        {
            value.lsevent_id = eventid;
            rep.Insert(value);
            return value;
        }

        [PUT("api/v1/events/{eventid:int}/packsets/{packsetid:int}")]
        public void Put(int eventid, int packsetid, [FromBody]packset value)
        {
            value.lsevent_id = eventid;
            value.id = packsetid;
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("api/v1/events/{eventid:int}/packsets/{packsetid:int}")]
        public void Delete(int eventid, int packsetid)
        {
            int rc = rep.Delete(packsetid);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}