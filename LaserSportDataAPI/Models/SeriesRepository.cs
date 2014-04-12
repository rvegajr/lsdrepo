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
    public class SeriesRepository : GenericRepository<LaserSportDataObjects.series>
    {
        public SeriesRepository()
            : base("series")
        {
        }

        public virtual IEnumerable<series> GetSeriesByEvent(int event_id = -1)
        {

            string sql = "SELECT * FROM series";
            if (event_id > -1) sql += " WHERE lsevent_id = @0";
            var lst = db.Fetch<series>(sql, event_id);
            return lst;
        }
    }
}