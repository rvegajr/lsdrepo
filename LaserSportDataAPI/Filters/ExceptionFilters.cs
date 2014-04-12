using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using System.Diagnostics.Tracing;
using System.IO;
namespace LaserSportDataAPI.Filters
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //Log Critical errors
            //Debug.WriteLine(context.Exception);
            ITraceWriter traceWriter = context.ActionContext.ControllerContext.Configuration.Services.GetTraceWriter();
            traceWriter.Trace(context.ActionContext.Request, "LaserSportDataAPI Exception", TraceLevel.Error, context.Exception, "{0}", context.Exception.Message);
 
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = context.Exception.Message
            });
        }
    }
}