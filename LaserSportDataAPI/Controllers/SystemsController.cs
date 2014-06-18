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
    public class SystemsController : ApiController
    {

        SystemsRepository rep = new SystemsRepository();
        [GET("api/v1/systems")]
        public IEnumerable<system> Get()
        {
            IEnumerable<system> lst = rep.Get();
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/systems/{id:int}")]
        public system Get(int id)
        {
            var a = rep.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("api/v1/systems")]
        public system Post([FromBody]system value)
        {
            rep.Insert(value);
            return value;
        }

        [PUT("api/v1/systems/{id:int}")]
        public void Put(int id, [FromBody]system value)
        {
            int rc = rep.Delete(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("api/v1/systems/{id:int}")]
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
