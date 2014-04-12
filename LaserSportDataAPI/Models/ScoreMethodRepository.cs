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
    public class ScoreMethodRepository : GenericRepository<LaserSportDataObjects.score_method>
    {
        public ScoreMethodRepository()
            : base("score_method")
        {
        }
        public IEnumerable<LaserSportDataObjects.score_method> GetByCode(string ScoreMethodCode)
        {
            return db.Fetch<LaserSportDataObjects.score_method>("SELECT * FROM lsdrep.score_methods WHERE code=@0", ScoreMethodCode);
        }

    }


}