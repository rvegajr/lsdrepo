using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using LaserSportDataObjects;
using System.Linq.Expressions;
using LaserSportDataAPI.DAL;
using PetaPoco;
namespace LaserSportDataAPI
{
    [TableName("vw_match_team_player_scores")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public partial class match_team_player_score : LSREPConnDB.Record<match_team_player_score>
    {
        [Column]
        public string id { get; set; }
        [Column]
        public int lsevent_id { get; set; }
        [Column]
        public int match_id { get; set; }
        [Column]
        public int team_id { get; set; }
        [Column]
        public int series_id { get; set; }
        [Column]
        public string guid { get; set; }
        [Column]
        public DateTime? scheduled { get; set; }
        [Column]
        public decimal? player_score_sum { get; set; }
        [Column]
        public decimal? score_override { get; set; }
    }
}
namespace LaserSportDataAPI.Models
{

    public class MatchTeamPlayerScoresRepository : GenericRepositoryReadOnly<match_team_player_score>
    {
        public MatchTeamPlayerScoresRepository()
            : base("vw_match_team_player_scores")
        {
        }

        public virtual IEnumerable<match_team_player_score> GetScoresByEvent(int eventid)
        {
            var lst = db.Fetch<match_team_player_score>("SELECT * FROM vw_match_team_player_scores WHERE lsevent_id = @0", eventid);
            return lst;
        }
        public virtual IEnumerable<match_team_player_score> GetScoresByEventSeries(int eventid, int seriesid)
        {
            var lst = db.Fetch<match_team_player_score>("SELECT * FROM vw_match_team_player_scores WHERE lsevent_id = @0 AND series_id=@1", eventid, seriesid);
            return lst;
        }
        public virtual IEnumerable<match_team_player_score> GetScoresByMatch(int eventid, int matchid)
        {
            var lst = db.Fetch<match_team_player_score>("SELECT * FROM vw_match_team_player_scores WHERE lsevent_id = @0 AND match_id = @1", eventid, matchid);
            return lst;
        }
        public virtual IEnumerable<match_team_player_score> GetScoresByMatchTeam(int eventid, int matchid, int teamId)
        {
            var lst = db.Fetch<match_team_player_score>("SELECT * FROM vw_match_team_player_scores WHERE lsevent_id = @0 AND match_id = @1 AND team_id = @2", eventid, matchid, teamId);
            return lst;
        }

    }
}