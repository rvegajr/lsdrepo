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
    public class SystemTargetTypesRepository : GenericRepository<LaserSportDataObjects.system_target_type>
    {
        public SystemTargetTypesRepository()
            : base("system_target_types")
        {
        }
    }
}