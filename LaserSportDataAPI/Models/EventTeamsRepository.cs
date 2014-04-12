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
    public class EventTeamsRepository : GenericRepository<LaserSportDataObjects.team>
    {
        public EventTeamsRepository()
            : base("teams")
        {
            _SQL = "S";
        }
    }
}