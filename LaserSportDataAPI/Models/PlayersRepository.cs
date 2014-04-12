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
    public class PacksetsRepository : GenericRepository<LaserSportDataObjects.packset>
    {
        public PacksetsRepository()
            : base("packsets")
        {
        }

        public virtual IEnumerable<packset> GetPacksetsByEvent(int event_id = -1)
        {

            string sql = "SELECT * FROM packsets";
            if (event_id > -1) sql += " WHERE lsevent_id = @0";
            var lst = db.Fetch<packset>(sql, event_id);
            return lst;
        }

    }
}