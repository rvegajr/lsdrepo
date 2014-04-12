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
using LaserSportDataAPI.DAL;
using System.Reflection;

namespace LaserSportDataAPI.Controllers
{
    public class ResultsController : ApiController
    {
        
        [GET("events/{eventid:int}/results/summary")]
        public IEventSummary Get(int eventid)
        {
            var res = ScoreMethodInstance(eventid);
            return res.GetEventSummary(eventid, 0);
        }
        [GET("events/{eventid:int}/results/summary/")]
        [GET("events/{eventid:int}/results/summary/series/{seriesid:int}")]

        [GET("events/{eventid:int}/results/summary/games")]
        [GET("events/{eventid:int}/results/summary/games/details/")]
        [GET("events/{eventid:int}/results/summary/{seriesid:int}/games")]
        [GET("events/{eventid:int}/results/summary/{seriesid:int}/games")]

        private IScoreMethod ScoreMethodInstance(int evt)
        {
            string sClassName = "ScoreMethod";
            string sAssemblyName = "";
            try
            {
                EventsRepository oevtrep = new EventsRepository();
                lsevent oevt = oevtrep.GetByID(evt);
                ScoreMethodRepository ismrep = new ScoreMethodRepository();
                score_method osm = ismrep.GetByID(oevt.score_method_id);
                sAssemblyName = osm.proc;
                Assembly asmCurrent = Assembly.Load(new AssemblyName(sAssemblyName));
                sAssemblyName = osm.proc + "." + sClassName;
                IScoreMethod inst = (IScoreMethod)asmCurrent.CreateInstance(sClassName);
                if (inst==null) {
                    throw new Exception("Could not create an instance of class " + sClassName);
                }
                return inst;
            }
            catch (Exception ex)
            {
                sAssemblyName = "LaserSportDataAPI.ScoreMethod.Sample";
                sClassName = sAssemblyName + ".ScoreMethod";
                Assembly asmCurrent = Assembly.Load(new AssemblyName(sAssemblyName));
                IScoreMethod inst = (IScoreMethod)asmCurrent.CreateInstance(sClassName);
                return inst;
                //return null;
            }
        } 

    }
}
