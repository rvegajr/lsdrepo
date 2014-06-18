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
    public class TargetTypesController : ApiController
    {
        private TargetTypesRepository rep = new TargetTypesRepository();

        [GET("api/v1/targets")]
        public IEnumerable<target_type> Get()
        {
            var lst = rep.Get();
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("api/v1/targets/{id:int}")]
        public target_type Get(int id)
        {
            var a = rep.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [POST("api/v1/targets")]
        public target_type Post([FromBody]target_type value)
        {
            rep.Insert(value);
            return value;
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [PUT("api/v1/targets/{id:int}")]
        public void Put(int id, [FromBody]target_type value)
        {
            int rc = rep.Delete(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [LaserSportDataAPI.Filters.BasicAuthentication]
        [DELETE("api/v1/targets/{id:int}")]
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
