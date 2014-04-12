using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using LaserSportDataObjects;
using System.Linq.Expressions;
using LaserSportDataAPI.DAL;

namespace LaserSportDataAPI.Models
{
    public class TeamsRepository : GenericRepository<LaserSportDataObjects.team>
    {
        public TeamsRepository()
            : base("teams")
        {
        }
        public virtual team GetTeamByEvent(int eventid, int teamid)
        {
            return db.SingleOrDefault<team>("SELECT * FROM teams WHERE lsevent_id = @0 AND id=@1", eventid, teamid);
        }

        public virtual IEnumerable<team> GetTeamsByEvent(int event_id = -1)
        {

            string sql = "SELECT * FROM teams";
            if (event_id > -1) sql += " WHERE lsevent_id = @0";
            var lst = db.Fetch<team>("SELECT * FROM teams WHERE lsevent_id = @0", event_id);
            return lst;
        }
    }
}