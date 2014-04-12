using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using LaserSportDataObjects;
using System.Linq.Expressions;
using LaserSportDataObjects;
using PetaPoco;
using LaserSportDataAPI.DAL;
namespace LaserSportDataAPI.Models
{
    public class MatchPlayerHitsRepository : GenericRepository<match_player_hit>
    {
        public MatchPlayerHitsRepository()
            : base("match_player_hits")
        {
        }
    }
}