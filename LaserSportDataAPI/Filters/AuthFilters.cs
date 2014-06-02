using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LaserSportDataAPI.Models;
namespace LaserSportDataAPI.Filters
{
    //Thank you http://stevescodingblog.co.uk/basic-authentication-with-asp-net-webapi/
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string sEventId = "";
            int EventId = int.MinValue;
            EventsRepository evtRepo = new EventsRepository();

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            } 
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
 
                string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                IEnumerable<string> headerValues;

                if (actionContext.Request.Headers.TryGetValues("event_id", out headerValues))
                {
                    sEventId = headerValues.First();
                    int.TryParse(sEventId, out EventId);
                }
                if (!evtRepo.PlayerIsAuthorizedToEdit(EventId, username, password))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }

            }


        }
    }

    public class AuthFilters
    {
    }
}