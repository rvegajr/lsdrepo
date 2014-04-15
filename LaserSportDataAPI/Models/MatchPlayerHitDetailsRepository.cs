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

    public class MatchPlayerHitDetailsRepository :GenericRepositoryReadOnly<match_team_player_score>
    {
        public MatchPlayerHitDetailsRepository()
            : base("vw_match_team_player_scores")
        {
        }
    }
}