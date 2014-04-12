using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using LaserSportDataObjects;
using LaserSportDataAPI.Models;
namespace LaserSportDataAPI.CLI
{
    class LsrXlsTeamData
    {
        public string TeamName { get; set; }
        public string PackSet { get; set; }
        public decimal? TeamScore { get; set; }
    }
    class LsrXlsMatchData
    {

        public DateTime? ScheduledDate { get; set; }
        public string SeriesName { get; set; }
        public string ScoringMethodTypeName { get; set; }
        public Dictionary<string, LsrXlsTeamData> TeamData { get; set; }
        public match MatchObject { get; set; }
        public LsrXlsMatchData()
        {
            TeamData = new Dictionary<string, LsrXlsTeamData>();
        }

    }
    class LsrXlsImport
    {
        public OnStatusChangeDelegate OnStatusChange { get; set; }
        public delegate void OnStatusChangeDelegate(string message);
        protected void StatusChange(string message) {
            if (this.OnStatusChange!=null)
            {
                this.OnStatusChange(message);
            }
        }

        private Dictionary<string, string> dictHeaders;
        private List<string> arrHeaderNames;
        public int RowCount { get; set; }
        public int RecordCount { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        private DateTime scheduledDate;
        public DateTime ScheduledDate { 
               get 
               {
                  return scheduledDate; 
               }
               set 
               {
                  scheduledDate = value; 
               }
        }
        public string ScoringMethod { get; set; }
        public void ParseEventString(string sEvent)
        {
            string[] sarrEvent = sEvent.Split('|');
            if (sarrEvent.Length > 0) this.EventName = sarrEvent[0];
            if (sarrEvent.Length > 1) this.EventCode = sarrEvent[1];
            if (sarrEvent.Length > 2) DateTime.TryParse(sarrEvent[2], out scheduledDate);
            if (sarrEvent.Length > 3) this.ScoringMethod = sarrEvent[3];
        }

        public LsrXlsImport(Dictionary<string, string> vdictHeaders, List<string> varrHeaderNames)
        {
            arrHeaderNames = varrHeaderNames;
            dictHeaders = vdictHeaders;
        }
        private Dictionary<string, series> SeriesList = new Dictionary<string, series>();
        private Dictionary<string, packset> PackSetList = new Dictionary<string, packset>();
        private Dictionary<string, team> TeamList = new Dictionary<string, team>();
        private Dictionary<string, player> PlayerList = new Dictionary<string, player>();
        private Dictionary<string, string> PlayerTeamList = new Dictionary<string, string>();
        private Dictionary<DateTime?, LsrXlsMatchData> MatchList = new Dictionary<DateTime?, LsrXlsMatchData>();
        private List<string> PlayerTeamIdx = new List<string>();
        
        public bool ImportFromExcel(string sExcelFileName) {
            StatusChange("Importing '" + sExcelFileName + "' into the LaserSport Data Repository");
            bool bret = false;
            this.InitDataObjects();
            ISheet sheet;
            LsrXlsRec rec = new LsrXlsRec();
            if (Path.GetExtension(sExcelFileName).Equals(".xlsx"))
            {
                StatusChange("Importing from a newer Excel file format");
                XSSFWorkbook wb;
                using (FileStream file = new FileStream(sExcelFileName, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(file);
                }
                sheet = wb.GetSheetAt(0);
            }
            else if (Path.GetExtension(sExcelFileName).Equals(".xls"))
            {
                StatusChange("This is an an older Excel file format");
                HSSFWorkbook wb;
                using (FileStream file = new FileStream(sExcelFileName, FileMode.Open, FileAccess.Read))
                {
                    wb = new HSSFWorkbook(file);
                }
                sheet = wb.GetSheetAt(0);
            }
            else
            {
                StatusChange("I am not sure what file extention this is.. exiting import");
                throw new Exception("I am not sure what file extention this is");
            }

            int iRowCount = 0;
            StatusChange("Making a first pass to get Series, PackSets, Unique Team Names, and unique Player Names");
            for (int irow = 0; irow <= sheet.LastRowNum; irow++)
            {
                if ((irow > 0) && (irow % 100 == 0)) StatusChange("1st Pass: Processed " + irow.ToString() + " out of " + sheet.LastRowNum.ToString());

                IRow row = sheet.GetRow(irow);
                iRowCount++;
                if (row != null)
                {
                    if (irow == 0)
                    {
                        dictHeaders = HeadersToDict(row, out arrHeaderNames);
                        rec = new LsrXlsRec(dictHeaders, arrHeaderNames);
                    }
                    else
                    {
                        this.RecordCount++;
                        if (rec.FromIRow(row))
                        {
                            if (!this.SeriesList.ContainsKey(rec.Series)) this.SeriesList.Add(rec.Series, null);
                            if (!this.TeamList.ContainsKey(rec.TeamName)) this.TeamList.Add(rec.TeamName, null);
                            if (!this.PlayerList.ContainsKey(rec.PlayerName))
                            {
                                this.PlayerList.Add(rec.PlayerName, null);
                                PlayerTeamList.Add(rec.PlayerName, rec.TeamName);
                            }
                            if (!this.MatchList.ContainsKey(rec.GameTime))
                            {
                                LsrXlsMatchData matchdata = new LsrXlsMatchData() { ScheduledDate = rec.GameTime, ScoringMethodTypeName = rec.ScoringType, SeriesName = rec.Series };
                                this.MatchList.Add(rec.GameTime, matchdata);
                            }
                            if (!this.MatchList[rec.GameTime].TeamData.ContainsKey(rec.TeamName))
                            {
                                var teamData = new LsrXlsTeamData() { PackSet = rec.PackSet, TeamName = rec.TeamName, TeamScore = rec.TeamScore };
                                this.MatchList[rec.GameTime].TeamData.Add(rec.TeamName, teamData);
                            }
                            if (!this.PackSetList.ContainsKey(rec.PackSet)) this.PackSetList.Add(rec.PackSet, null);
                            this.RowCount++;
                        }
                    }

                }
            }
            this.EventSync();
            this.SeriesSync(this.eventCurrent.id, this.SeriesList);
            this.TeamsSync(this.eventCurrent.id, this.TeamList);
            this.PlayersSync(this.eventCurrent.id, this.PlayerList);
            this.PackSetSync(this.eventCurrent.id, this.PackSetList);
            this.MatchSync(this.eventCurrent.id);

            StatusChange("Making a second pass to import Player Data");

            for (int irow = 0; irow <= sheet.LastRowNum; irow++)
            {
                IRow row = sheet.GetRow(irow);
                iRowCount++;
                if ((irow > 0) && (irow % 100 == 0)) StatusChange("2nd Pass: Processed " + irow.ToString() + " out of " + sheet.LastRowNum.ToString());

                if (row != null)
                {
                    if (irow == 0)
                    {
                        dictHeaders = HeadersToDict(row, out arrHeaderNames);
                        rec = new LsrXlsRec(dictHeaders, arrHeaderNames);
                    }
                    else
                    {
                        this.RecordCount++;
                        /*
                         * Note that we are not creating a new instance of this class, which means the previous values will be copied and any non-space values will be written over
                         */
                        if (rec.FromIRow(row))
                        {
                            match matchObject = MatchList[rec.GameTime].MatchObject;
                            player playerObject = PlayerList[rec.PlayerName];
                            match_player matchPlayer = matchPlayerRepo.Get(matchObject.id, playerObject.id);
                            if (matchPlayer==null) {
                                matchPlayer = new match_player() { match_id = matchObject.id, player_id = playerObject.id, score = rec.Score};
                                matchPlayer = matchPlayerRepo.Insert(matchPlayer);
                            } else {
                                matchPlayer.score = rec.Score;
                                matchPlayerRepo.Update(matchPlayer);
                            }
                        }
                    }

                }
            }

            StatusChange("Import of data has completed!");

            bret = true;
            return bret;
        }

        protected EventsRepository eventsRepo;
        protected ScoreMethodRepository scoreMethodRepo;
        protected SeriesRepository seriesRepo;
        protected TeamsRepository teamsRepo;
        protected EventTeamsRepository eventTeamsRepo;
        protected PlayersRepository playersRepo;
        protected EventPlayersRepository eventPlayersRepo;
        protected MatchesRepository matchRepo;
        protected MatchPlayersRepository matchPlayerRepo;
        protected MatchTeamsRepository matchTeamsRepo;
        protected PacksetsRepository packsetRepo;
        protected lsevent eventCurrent;

        protected void InitDataObjects()
        {
            eventsRepo = new EventsRepository();
            scoreMethodRepo = new ScoreMethodRepository();
            seriesRepo = new SeriesRepository();
            teamsRepo = new TeamsRepository();
            eventTeamsRepo = new EventTeamsRepository();
            playersRepo = new PlayersRepository();
            eventPlayersRepo = new EventPlayersRepository();
            matchRepo = new MatchesRepository();
            matchTeamsRepo = new MatchTeamsRepository();
            packsetRepo = new PacksetsRepository();
            matchPlayerRepo = new MatchPlayersRepository();
        }

        private void SeriesSync(int EventId, Dictionary<string, series> SeriesList)
        {
            var SeriesListUpdated = new Dictionary<string, series>();
            var lstSeries = seriesRepo.GetSeriesByEvent(EventId);
            foreach (string seriesName in SeriesList.Keys)
            {
                bool found = false;
                if (lstSeries.Count() > 0)
                {
                    found = (lstSeries.Any(x => x.name.Contains(seriesName)));
                }
                if (!found)
                {
                    var newSeries = new series();
                    newSeries.lsevent_id = EventId;
                    newSeries.name = seriesName;
                    SeriesListUpdated[seriesName] = seriesRepo.Insert(newSeries);
                }
                else
                {
                    SeriesListUpdated[seriesName] = lstSeries.First(x => x.name == seriesName);
                }
            }
            this.SeriesList = SeriesListUpdated;
        }

        private void MatchSync(int EventId)
        {
            StatusChange("Syncing Matches");

            var TeamsListUpdated = new Dictionary<string, team>();
            var lst = matchRepo.GetMatchesByEvent(EventId);
            string LastSeries = "";
            int gameInSeriesCount = 0;
            int gameCount = 0;
            int SeriesCount = 1;
            StatusChange("There are " + MatchList.Count().ToString()  + " matches to syncronize");

            foreach (DateTime matchDateTime in MatchList.Keys)
            {

                string sGameUid = "";
                if ((gameCount > 0) && (gameCount % 20 == 0)) StatusChange("Processed match " + gameCount.ToString() + " out of " + MatchList.Count().ToString());
                gameInSeriesCount++;
                gameCount++;
                LsrXlsMatchData matchData = MatchList[matchDateTime];
                match foundMatch = new match();
                if (LastSeries.Length == 0) LastSeries = matchData.SeriesName;
                bool found = false;

                if (lst.Count() > 0)
                {
                    found = (lst.Any(x => x.scheduled.Equals(matchDateTime)));
                }
                if (!found)
                {
                    var newMatch = new match();
                    newMatch.lsevent_id = EventId;
                    newMatch.scheduled = matchDateTime;
                    var SeriesNameOnlyCapitalLetters = new String(matchData.SeriesName.Where(c => Char.IsLetter(c) && Char.IsUpper(c)).ToArray());
                    if (SeriesNameOnlyCapitalLetters.Length == 0)
                    {
                        sGameUid = "SE" + SeriesCount.ToString().PadLeft(2) + "MA" + gameInSeriesCount.ToString().PadLeft(2, '0');
                    }
                    else
                    {
                        sGameUid = SeriesNameOnlyCapitalLetters + gameInSeriesCount.ToString().PadLeft(2, '0');
                    }
                    newMatch.guid = sGameUid;
                    if (SeriesList.ContainsKey(matchData.SeriesName))
                    {
                        var oSeries = SeriesList[matchData.SeriesName];
                        newMatch.series_id = oSeries.id;
                    }
                    foundMatch = matchRepo.Insert(newMatch);
                }
                else
                {
                    foundMatch = (lst.First(x => x.scheduled.Equals(matchDateTime)));
                }
                if (foundMatch != null)
                {
                    MatchList[matchDateTime].MatchObject = foundMatch;
                    foreach (var teamDataInMatch in matchData.TeamData)
                    {
                        team teamInMatch = TeamList[teamDataInMatch.Key];
                        match_team matchTeam = matchTeamsRepo.GetMatchTeam(foundMatch.id, teamInMatch.id);
                        if (matchTeam == null)
                        {
                            match_team newMatchTeam = new match_team() { match_id = foundMatch.id, team_id = teamInMatch.id};
                            if (teamDataInMatch.Value.TeamScore!=null) {
                                newMatchTeam.score_override = teamDataInMatch.Value.TeamScore;
                            }
                            if (teamDataInMatch.Value.PackSet != null)
                            {
                                packset packSet = PackSetList[teamDataInMatch.Value.PackSet];
                                newMatchTeam.packset_id = packSet.id;
                            }
                            matchTeamsRepo.Insert(newMatchTeam);
                        }
                        else
                        {
                            if (teamDataInMatch.Value.TeamScore != null)
                            {
                                matchTeam.score_override = teamDataInMatch.Value.TeamScore;
                            }
                            if (teamDataInMatch.Value.PackSet != null)
                            {
                                packset packSet = PackSetList[teamDataInMatch.Value.PackSet];
                                matchTeam.packset_id = packSet.id;
                            }
                            matchTeamsRepo.Update(matchTeam);
                        }
                    }
                }
                if (!LastSeries.Equals(matchData.SeriesName))
                {
                    LastSeries = matchData.SeriesName;
                    gameInSeriesCount = 0;
                    SeriesCount++;
                }
            }
            //this.TeamList = TeamsListUpdated;
        }

        private void TeamsSync(int EventId, Dictionary<string, team> TeamsList)
        {
            StatusChange("Syncing Teams");

            var TeamsListUpdated = new Dictionary<string, team>();
            var lst = teamsRepo.GetTeamsByEvent(EventId);
            foreach (string teamName in TeamsList.Keys)
            {
                bool found = false;
                if (lst.Count() > 0)
                {
                    found = (lst.Any(x => x.team_name.Contains(teamName)));
                }
                if (!found)
                {
                    var newTeam = new team();
                    newTeam.lsevent_id = EventId;
                    newTeam.team_name = teamName;
                    TeamsListUpdated[teamName] = teamsRepo.Insert(newTeam);
                }
                else
                {
                    TeamsListUpdated[teamName] = lst.First(x => x.team_name == teamName);
                }
            }
            StatusChange("... " + TeamsListUpdated.Count().ToString() + " Synced");
            this.TeamList = TeamsListUpdated;
        }

        private void PlayersSync(int EventId, Dictionary<string, player> PlayersList)
        {
            StatusChange("Syncing Players");

            var PlayersListUpdated = new Dictionary<string, player>();
            var lst = playersRepo.Get();
            foreach (string playerName in PlayersList.Keys)
            {
                bool found = false;
                if (lst.Count() > 0)
                {
                    found = (lst.Any(x => x.code_name.Equals(playerName)));
                }
                if (!found)
                {
                    var newPlayer = new player();
                    newPlayer.code_name = playerName;
                    PlayersListUpdated[playerName] = playersRepo.Insert(newPlayer);
                }
                else
                {
                    PlayersListUpdated[playerName] = lst.First(x => x.code_name == playerName);
                }

                team Team = this.TeamList[PlayerTeamList[playerName]];
                List<lsevent_player> evtplayers = (List<lsevent_player>)eventPlayersRepo.GetPlayersByNameAndTeam(EventId, playerName, Team.id);
                if (evtplayers.Count == 0)
                {
                    eventPlayersRepo.Insert(new lsevent_player() { lsevent_id = EventId, player_id = PlayersListUpdated[playerName].id, alias = playerName, team_id = Team.id });
                }
            }
            StatusChange("... " + PlayersListUpdated.Count().ToString() + " Synced");

            this.PlayerList = PlayersListUpdated;
        }

        private void PackSetSync(int EventId, Dictionary<string, packset> PackSetList)
        {
            StatusChange("Syncing Packsets");

            var PacksetListUpdated = new Dictionary<string, packset>();
            var lst = packsetRepo.GetPacksetsByEvent(EventId);
            foreach (string packsetName in PackSetList.Keys)
            {
                bool found = false;
                if (lst.Count() > 0)
                {
                    found = (lst.Any(x => x.descr.Equals(packsetName)));
                }
                if (!found)
                {
                    var newPackset = new packset();
                    newPackset.descr = packsetName;
                    newPackset.color = packsetName;
                    newPackset.lsevent_id = EventId;
                    PacksetListUpdated[packsetName] = packsetRepo.Insert(newPackset);
                }
                else
                {
                    PacksetListUpdated[packsetName] = lst.First(x => x.descr.Equals(packsetName));
                }

            }
            StatusChange("... " + PacksetListUpdated.Count().ToString() + " Synced");
            this.PackSetList = PacksetListUpdated;
        }
        private void EventSync()
        {
            StatusChange("Syncing Event");

            var events = eventsRepo.GetByName(this.EventName);
            var scoreMethods = scoreMethodRepo.GetByCode(this.ScoringMethod);
            lsevent evt;
            if (scoreMethods.Count() == 0)
            {
                throw new Exception("Scoring method '" + this.ScoringMethod + "' is unknown.");
            }
            if (events.Count() == 0)
            {
                evt = new lsevent();
                evt.lsevent_name = this.EventName;
                evt.score_method_id = scoreMethods.First().id;
                if (this.scheduledDate != null)
                {
                    evt.scheduled = this.scheduledDate;
                }
                this.eventCurrent = eventsRepo.Insert(evt);
            }
            else
            {
                this.eventCurrent = events.First();
                this.eventCurrent.score_method_id = scoreMethods.First().id;
                if (this.scheduledDate != null)
                {
                    this.eventCurrent.scheduled = this.scheduledDate;
                }
                eventsRepo.Update(this.eventCurrent);
            }
        }

        protected Dictionary<string, string> HeadersToDict(IRow row, out List<string> HeaderNames)
        {
            HeaderNames = new List<string>();
            Dictionary<string, string> dictHeaders = new Dictionary<string, string>();
            if (row != null)
            {
                for (int icell = 0; icell <= row.Cells.Count - 1; icell++)
                {
                    if (row.Cells[icell] != null) //null is wh en the row only contains empty cells 
                    {
                        dictHeaders.Add(row.Cells[icell].StringCellValue, icell.ToString());
                        dictHeaders.Add(icell.ToString(), row.Cells[icell].StringCellValue);
                        HeaderNames.Add(row.Cells[icell].StringCellValue);
                    }
                }
            }
            return dictHeaders;
        }

    }
}
