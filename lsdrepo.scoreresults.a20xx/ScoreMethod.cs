using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSportDataAPI.ScoreMethod;
namespace LaserSportDataAPI.ScoreMethod.A20XXv2
{
    public class ScoreMethod : IScoreMethod
    {
        public IEventSummary GetEventSummary(int lsevent_id, int series_id)
        {
            throw new NotImplementedException();
        }

        public IMatchSummary GetMatchSummary(int lsevent_id, int series_id)
        {
            throw new NotImplementedException();
        }

        public IMatchDetails GetMatchDetails(int lsevent_id, int series_id)
        {
            throw new NotImplementedException();
        }
    }
}
