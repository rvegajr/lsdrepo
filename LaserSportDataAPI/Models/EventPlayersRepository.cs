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
    public class EventPlayer : player
    {
        [Column]
        public string alias { get; set; }
        [Column]
        public string team_name { get; set; }
        [Column]
        public int team_id { get; set; }
    }

    public class EventPlayersRepository : GenericRepository<lsevent_player>
    {
        public EventPlayersRepository()
            : base("lsevent_players")
        {
            _SQL2 = @" 
SELECT
  p.*,
  t.team_name,
  t.id as team_id,
  ep.alias
FROM lsevent_players ep
  INNER JOIN players p
    ON ep.player_id = p.id
  INNER JOIN teams t
    ON t.id = ep.team_id AND t.lsevent_id = ep.lsevent_id ";
        }
        private string _SQL2 = "";
        public virtual EventPlayer GetPlayerByEvent(int eventid, int playerid)
        {
            return db.SingleOrDefault<EventPlayer>(_SQL2 + " WHERE ep.lsevent_id = @0 AND p.id=@1", eventid, playerid);
        }

        public virtual IEnumerable<EventPlayer> GetPlayersByEvent(int event_id = -1)
        {

            string sql = _SQL2;
            if (event_id > -1) sql += " WHERE ep.lsevent_id = @0";
            var lst = db.Fetch<EventPlayer>(sql, event_id);
            return lst;
        }

        public virtual IEnumerable<EventPlayer> GetPlayersByEventAndTeam(int event_id = -1, int team_id = -1)
        {

            string sql = _SQL;
            if (event_id > -1) sql += " WHERE ep.lsevent_id = @0 AND ep.team_id = @1";
            var lst = db.Fetch<EventPlayer>(sql, event_id, team_id);
            return lst;
        }

        public virtual IEnumerable<lsevent_player> GetPlayersByNameAndTeam(int event_id, string alias, int team_id)
        {

            string sql = "SELECT * FROM lsevent_players WHERE lsevent_id = @0 AND alias = @1 AND team_id = @2";
            var lst = db.Fetch<lsevent_player>(sql, event_id, alias, team_id);
            return lst;
        }

    }
}