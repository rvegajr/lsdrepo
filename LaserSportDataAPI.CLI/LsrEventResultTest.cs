using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LaserSportDataObjects;
using LaserSportDataAPI.Models;
using System.Reflection;

namespace LaserSportDataAPI.CLI
{
    class LsrEventResultTest
    {
        public OnStatusChangeDelegate OnStatusChange { get; set; }
        public delegate void OnStatusChangeDelegate(string message);
        protected void StatusChange(string message) {
            if (this.OnStatusChange != null)
            {
                this.OnStatusChange(message);
            }
        }
        public void EventResultRest(int eventId)
        {
            IScoreMethod scoreMethod = ScoreMethodInstance(eventId);
            scoreMethod.Url = "http://localhost:8000";
            if (scoreMethod != null)
            {
                IEventSummary evtSummary = scoreMethod.GetEventSummary(eventId, -1);
            }
        }

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
                IScoreMethod inst = (IScoreMethod)asmCurrent.CreateInstance(sAssemblyName);
                if (inst == null)
                {
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
