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
    public class EventsRepository : GenericRepository<LaserSportDataObjects.lsevent>
    {
        public EventsRepository()
            : base("lsevents")
        {
        }

        public IEnumerable<LaserSportDataObjects.lsevent> GetByName(string EventName)
        {
            return db.Fetch<LaserSportDataObjects.lsevent>("SELECT * FROM lsdrep.lsevents WHERE lsevent_name=@0", EventName);
        }
    }


}