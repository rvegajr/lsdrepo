using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using LaserSportDataObjects;
using System.Linq.Expressions;
using LaserSportDataAPI.DAL;
using LaserSportDataObjects;
using PetaPoco;

namespace LaserSportDataAPI.Models
{
    public class MatchTeam : team
    {
        [Column]
        public decimal score_override { get; set; }
        [Column]
        public int sort_order { get; set; }
    }

    public class MatchTeamsRepository : GenericRepository<match_team>
    {
        public MatchTeamsRepository()
            : base("match_teams")
        {
        }
        protected string _SQL2 = @"SELECT
  mt.*,
  t.lsevent_id,
  t.team_name
FROM 
  match_teams mt
  INNER JOIN teams t
    ON t.id = mt.team_id 
";
        public virtual IEnumerable<MatchTeam> GetTeamsByMatch(int matchid)
        {
            var lst = db.Fetch<MatchTeam>(this._SQL2 + " WHERE mt.match_id=@0", matchid);
            return lst;
        }

        public virtual MatchTeam GetMatchTeamObject(int matchid, int teamid)
        {
            return db.FirstOrDefault<MatchTeam>(this._SQL2 + " WHERE mt.match_id=@0 AND mt.team_id=@1 ", matchid, teamid);
        }

        public virtual match_team GetMatchTeam(int matchid, int teamid)
        {
            return db.FirstOrDefault<match_team>(this._SQL + " WHERE match_id=@0 AND team_id=@1 ", matchid, teamid);
        }

    }
}