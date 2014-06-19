using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;
using System.IO;
using AttributeRouting.Web.Http;
using LaserSportDataObjects;
using LaserSportDataAPI.Models;
using LaserSportDataAPI.DAL;
using System.Reflection;

namespace LaserSportDataAPI.Controllers
{
    public class ResultsController : ApiController
    {

        [GET("api/v1/events/{eventid:int}/results/summary")]
        public IEventSummary Get(int eventid)
        {
            var res = ScoreMethodInstance(eventid);
            res.Url = this.Request.RequestUri.Scheme + "://" + this.Request.RequestUri.Authority + @"/";
            return res.GetEventSummary(eventid, 0);
        }
        [GET("api/v1/events/{eventid:int}/results/summary/")]
        [GET("api/v1/events/{eventid:int}/results/summary/series/{seriesid:int}")]

        [GET("api/v1/events/{eventid:int}/results/summary/games")]
        [GET("api/v1/events/{eventid:int}/results/summary/games/details/")]
        [GET("api/v1/events/{eventid:int}/results/summary/{seriesid:int}/games")]
        [GET("api/v1/events/{eventid:int}/results/summary/{seriesid:int}/games")]

        private IScoreMethod ScoreMethodInstance(int evt)
        {
            string sClassName = "ScoreMethod";
            string sAssemblyName = "";
            string sObjectName = "";
            IScoreMethod inst;
            try
            {
                //get the full location of the assembly with DaoTests in it
                EventsRepository oevtrep = new EventsRepository();
                lsevent oevt = oevtrep.GetByID(evt);
                ScoreMethodRepository ismrep = new ScoreMethodRepository();
                score_method osm = ismrep.GetByID(oevt.score_method_id);
                sAssemblyName = osm.proc;
                if (String.IsNullOrEmpty(sAssemblyName)) {
                    sAssemblyName = "LaserSportDataAPI.SystemObjects.Sample";
                }
                string AssemblyFilePath = AssemblyDirectory + "\\" + sAssemblyName + ".dll";

                Assembly asmCurrent = Assembly.Load(new AssemblyName(sAssemblyName));
                sObjectName = osm.proc + "." + sClassName;
                inst = (IScoreMethod)asmCurrent.CreateInstance(sObjectName);
                if (inst==null) {
                    throw new Exception("Could not create an instance of class " + sObjectName);
                }
                return inst;
            }
            catch (Exception ex)
            {
                sAssemblyName = "LaserSportDataAPI.SystemObjects.Sample";
                sObjectName = sAssemblyName + "." + sClassName;
                Assembly asmCurrent = Assembly.Load(new AssemblyName(sAssemblyName));
                inst = (IScoreMethod)asmCurrent.CreateInstance(sObjectName);
                return inst;
                //return null;
            }
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
