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
    public class SystemTargetTypesController : ApiController
    {
        private SystemTargetTypesRepository rep = new SystemTargetTypesRepository();

        [GET("systems/{systemid:int}/targets")]
        public IEnumerable<system_target_type> Get(int systemid)
        {
            var lst = rep.Get();
            if ((lst == null) || (lst.ToList().Count == 0))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return lst;
        }

        [GET("systems/{systemid:int}/targets/{id:int}")]
        public system_target_type Get(int id, int targets)
        {
            var a = rep.GetByID(id);
            if (a == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return a;
        }

        [POST("systems/{systemid:int}/targets")]
        public system_target_type Post([FromBody]system_target_type value)
        {
            rep.Insert(value);
            return value;
        }

        [PUT("systems/{systemid:int}/targets/{id:int}")]
        public void Put(int id, [FromBody]system_target_type value)
        {
            int rc = rep.Delete(value);
            if (rc == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        [DELETE("systems/{systemid:int}targets/{id:int}")]
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
