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
    public class MatchPlayer : match_player
    {
        [Column]
        public string code_name { get; set; }
        [Column]
        public string first_name { get; set; }
        [Column]
        public string last_name { get; set; }
        [Column]
        public int team_id { get; set; }
        [Column]
        public string team_name { get; set; }
    }
    public class MatchPlayersRepository : GenericRepository<match_player>
    {
        public MatchPlayersRepository()
            : base("match_players")
        {
        }
        protected string _SQL2 = @"
SELECT
  mp.*,
  p.code_name,
  p.first_name,
  p.last_name,
  lp.team_id,
  t.team_name
FROM 
  match_players mp 
  INNER JOIN players p
    ON p.id = mp.player_id
  INNER JOIN matches m
    ON m.id = mp.match_id
  INNER JOIN lsevent_players lp
    ON lp.lsevent_id = m.lsevent_id AND lp.player_id = p.id
  INNER JOIN teams t
    ON t.id = lp.team_id 
";
        public virtual IEnumerable<MatchPlayer> GetPlayersByMatch(int matchid)
        {
            var lst = db.Fetch<MatchPlayer>(this._SQL2 + " WHERE mp.match_id=@0", matchid);
            return lst;
        }

        public virtual IEnumerable<MatchPlayer> GetPlayersByMatchAndTeam(int matchid, int teamid)
        {
            var lst = db.Fetch<MatchPlayer>(this._SQL2 + " WHERE mp.match_id=@0 AND lp.team_id=@1", matchid, teamid);
            return lst;
        }

        public virtual MatchPlayer Get(int matchid, int teamid, int playerid)
        {
            return db.FirstOrDefault<MatchPlayer>(this._SQL2 + " WHERE mp.match_id=@0 AND lp.team_id=@1 AND lp.player_id=@2", matchid, teamid, playerid);
        }
        public virtual match_player Get(int matchid, int playerid)
        {
            return db.FirstOrDefault<match_player>(this._SQL + " WHERE match_id=@0 AND player_id=@1", matchid, playerid);
        }
    }
}