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

        public bool PlayerIsAuthorizedToEdit(int eventId, string codeName, string editCode)
        {
            //string sql = @"SELECT lsevents.id FROM lsevent_players INNER JOIN players ON lsevent_players.player_id = players.id INNER JOIN lsevents ON lsevent_players.lsevent_id = lsevents.id WHERE ((code_name='Dark Angel') AND ((players.is_admin = 1) OR ((lsevents.id = 8) AND (edit_code = 'TEST' OR lsevent_players.is_admin = 1))))";
            string sql = @"SELECT *, lsevents.id FROM lsevent_players
  INNER JOIN players
    ON lsevent_players.player_id = players.id
  INNER JOIN lsevents
    ON lsevent_players.lsevent_id = lsevents.id
WHERE
  (
   ((code_name=@0) AND ((players.is_admin = 1))) OR 
   ((lsevents.id = @1) AND (code_name=@0) AND (edit_code = @2 OR lsevent_players.is_admin = 1))
  )";
            List<int> retList = db.Fetch<int>(sql, codeName, eventId, editCode);
            return (retList.Count > 0);
        }
    }


}