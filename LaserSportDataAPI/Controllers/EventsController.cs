using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;
using AttributeRouting.Web.Http;
using LaserSportDataAPI.Models;
using LaserSportDataObjects;

namespace LaserSportDataAPI.Controllers
{
    public class EventsController : ApiController
    {
        private EventsRepository rep = new EventsRepository();
        [GET("api/v1/events")]
        public IEnumerable<lsevent> Get()
        {
            IEnumerable<lsevent> lst = rep.Get();
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/events/{id:int}")]
        public lsevent Get(int id)
        {
            var a = rep.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [POST("api/v1/events")]
        public lsevent Post([FromBody]lsevent value)
        {
            return rep.Insert(value);
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [PUT("api/v1/events/{id:int}")]
        public void Put(int id, [FromBody]lsevent value)
        {
            int rc = rep.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [DELETE("api/v1/events/{id:int}")]
        public void Delete(int id)
        {
            int rc = rep.Delete(id);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}