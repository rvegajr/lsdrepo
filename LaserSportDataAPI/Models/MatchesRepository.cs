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
    public class MatchesRepository : GenericRepository<LaserSportDataObjects.match>
    {
        public MatchesRepository()
            : base("matches")
        {
        }
        public virtual match GetMatchByEvent(int eventid, int matchid)
        {
            return db.SingleOrDefault<match>("SELECT * FROM matches WHERE lsevent_id = @0 AND id=@1", eventid, matchid);
        }

        public virtual IEnumerable<match> GetMatchesByEvent(int event_id = -1)
        {

            string sql = "SELECT * FROM matches";
            if (event_id > -1) sql += " WHERE lsevent_id = @0";
            var lst = db.Fetch<match>(sql, event_id);
            return lst;
        }
    }
}