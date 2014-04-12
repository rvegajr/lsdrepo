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
        private EventsRepository eventsRepository = new EventsRepository();
        [GET("events")]
        public IEnumerable<lsevent> Get()
        {
            IEnumerable<lsevent> lst = eventsRepository.Get();
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("events/{id:int}")]
        public lsevent Get(int id)
        {
            var a = eventsRepository.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("events")]
        public lsevent Post([FromBody]lsevent value)
        {
            return eventsRepository.Insert(value);
        }

        [PUT("events/{id:int}")]
        public void Put(int id, [FromBody]lsevent value)
        {
            int rc = eventsRepository.Update(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("events/{id:int}")]
        public void Delete(int id)
        {
            int rc = eventsRepository.Delete(id);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }
    }
}