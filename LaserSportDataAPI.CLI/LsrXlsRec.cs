using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace LaserSportDataAPI.CLI
{
    class LsrMatchTeam
    {
        public DateTime? GameTime { get; set; }
        public String Series { get; set; }
        public String Packset { get; set; }
        public String TeamName { get; set; }
    }

    class LsrXlsRec
    {
        public LsrXlsRec()
        {
            arrHeaderNames = new List<string>();
            dictHeaders = new Dictionary<string, string>();
        }
        public LsrXlsRec(Dictionary<string, string> vdictHeaders, List<string> varrHeaderNames)
        {
            arrHeaderNames = varrHeaderNames;
            dictHeaders = vdictHeaders;
        }
        public void Clear()
        {
            this.GameTime = null;
            this.Series = "";
            this.ScoringType = "";
            this.SubGame = "";
            this.TeamName = "";
            this.PlayerName = "";
            this.Score = null;
            this.HitsFor = null;
            this.HitsAgainst = null;
            this.PackId = "";
            this.TeamScore = null;
        }
        private Dictionary<string, string> dictHeaders;
        private List<string> arrHeaderNames;
        public DateTime? GameTime { get; set; }
        public String Series { get; set; }
        public String ScoringType { get; set; }
        public String SubGame { get; set; }
        public String PackSet { get; set; }
        public String TeamName { get; set; }
        public String PlayerName { get; set; }
        public Decimal? Score { get; set; }
        public Decimal? HitsFor { get; set; }
        public Decimal? HitsAgainst { get; set; }
        public String PackId { get; set; }
        public Decimal? TeamScore { get; set; }
        private int iFieldsChanged = 0;
        public int FieldsChanged {
            get
            {
                return iFieldsChanged;
            }
            set
            {
                iFieldsChanged = value;
            } 
        }
        public bool FromIRow(IRow row)
        {
           
            for (int i = 0; i <= this.arrHeaderNames.Count(); i++)
            {
                ICell cell = row.GetCell(i);
                if ((cell != null) && (cell.CellType != NPOI.SS.UserModel.CellType.Blank))
                {
                    try
                    {
                        switch (dictHeaders[cell.ColumnIndex.ToString()])
                        {
                            case "Game Time":
                                this.GameTime = cell.DateCellValue;
                                iFieldsChanged++;
                                break;
                            case "Series":
                                this.Series = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Scoring Type":
                                this.ScoringType = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Sub Game":
                                this.SubGame = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Pack Set":
                                this.PackSet = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Team Name":
                                this.TeamName = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Player Name":
                                this.PlayerName = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Score":
                                this.Score = (decimal)cell.NumericCellValue;
                                iFieldsChanged++;
                                break;
                            case "Hits For":
                                this.HitsFor = (decimal)cell.NumericCellValue;
                                iFieldsChanged++;
                                break;
                            case "Hits Against":
                                this.HitsAgainst = (decimal)cell.NumericCellValue;
                                iFieldsChanged++;
                                break;
                            case "Pack ID":
                                try
                                {
                                    this.PackId = cell.NumericCellValue.ToString();
                                    iFieldsChanged++;
                                }
                                catch (Exception ex2)
                                {
                                    this.PackId = cell.StringCellValue;
                                    iFieldsChanged++;
                                }
                                break;
                            case "Team Score":
                                this.TeamScore = (decimal)cell.NumericCellValue;
                                iFieldsChanged++;
                                break;
                            default:
                                throw new Exception("Field?");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return (iFieldsChanged > 0);
        }
    }

    class ZgsXlsRec
    {
        public ZgsXlsRec()
        {
            arrHeaderNames = new List<string>();
            dictHeaders = new Dictionary<string, string>();
        }
        public ZgsXlsRec(Dictionary<string, string> vdictHeaders, List<string> varrHeaderNames)
        {
            arrHeaderNames = varrHeaderNames;
            dictHeaders = vdictHeaders;
        }
        public void Clear()
        {
            this.GameNo = null;
            this.GameId = "";
            this.StartDate = null;
            this.StartDateTime = null;
            this.EndDateTime = null;
            this.Series = "";
            this.ColorA = "";
            this.TeamA = "";
            this.ColorB = "";
            this.TeamB = "";
            this.ColorC = "";
            this.TeamC = "";
        }
        private int iFieldsChanged = 0;
        public int FieldsChanged
        {
            get
            {
                return iFieldsChanged;
            }
            set
            {
                iFieldsChanged = value;
            }
        }

        private Dictionary<string, string> dictHeaders;
        private List<string> arrHeaderNames;
        public Decimal? GameNo { get; set; }
        public String GameId { get; set; }
        public String Series { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public String ColorA { get; set; }
        public String TeamA { get; set; }
        public String ColorB { get; set; }
        public String TeamB { get; set; }
        public String ColorC { get; set; }
        public String TeamC { get; set; }

        public bool FromIRow(IRow row)
        {
            iFieldsChanged = 0;
            for (int i = 0; i <= this.arrHeaderNames.Count(); i++)
            {
                ICell cell = row.GetCell(i);
                if ((cell != null) && (cell.CellType != NPOI.SS.UserModel.CellType.Blank))
                {
                    try
                    {
                        switch (dictHeaders[cell.ColumnIndex.ToString()].Trim())
                        {
                            case "T#":
                                this.GameNo = (decimal)cell.NumericCellValue;
                                iFieldsChanged++;
                                break;
                            case "Game # title":
                                this.GameId = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Series":
                                this.Series = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Start Date":
                                this.StartDate = cell.DateCellValue;
                                iFieldsChanged++;
                                break;
                            case "Start Time":
                                this.StartDateTime = this.StartDate.Value.Add(new TimeSpan(cell.DateCellValue.Hour, cell.DateCellValue.Minute, cell.DateCellValue.Second));
                                iFieldsChanged++;
                                break;
                            case "End Time":
                                this.EndDateTime = this.StartDate.Value.Add(new TimeSpan(cell.DateCellValue.Hour, cell.DateCellValue.Minute, cell.DateCellValue.Second));
                                iFieldsChanged++;
                                break;
                            case "A color":
                                this.ColorA = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Team A":
                                this.TeamA = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "B color":
                                this.ColorB = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Team B":
                                this.TeamB = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "C color":
                                try
                                {
                                    this.ColorC = cell.StringCellValue.Trim();
                                    iFieldsChanged++;
                                }
                                catch (Exception ex2)
                                {
                                    this.ColorC = "";
                                    iFieldsChanged++;
                                }
                                break;
                            case "Team C":
                                try
                                {
                                    this.TeamC = cell.StringCellValue.Trim();
                                    iFieldsChanged++;
                                }
                                catch (Exception ex2)
                                {
                                    this.TeamC = "";
                                    iFieldsChanged++;
                                }
                                break;
                            default:
                                throw new Exception("Field?");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return (iFieldsChanged > 0);
        }
    }
        
    class ZgsXlsPlayerRec
    {
        public ZgsXlsPlayerRec()
        {
            arrHeaderNames = new List<string>();
            dictHeaders = new Dictionary<string, string>();
        }
        public ZgsXlsPlayerRec(Dictionary<string, string> vdictHeaders, List<string> varrHeaderNames)
        {
            arrHeaderNames = varrHeaderNames;
            dictHeaders = vdictHeaders;
        }

        public void Clear()
        {
            this.TeamName = "";
            this.No = "";
            this.CodeName = "";
            this.FirstName = "";
            this.LastName = "";
        }
        private int iFieldsChanged = 0;
        public int FieldsChanged
        {
            get
            {
                return iFieldsChanged;
            }
            set
            {
                iFieldsChanged = value;
            }
        }

        private Dictionary<string, string> dictHeaders;
        private List<string> arrHeaderNames;
        public String TeamNameKey { get; set; }
        public String TeamName { get; set; }
        public String No { get; set; }
        public String CodeName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        //Team	No.	Code Name	Real Name	Last Name
        public bool FromIRow(IRow row)
        {
            iFieldsChanged = 0;
            for (int i = 0; i <= this.arrHeaderNames.Count(); i++)
            {
                ICell cell = row.GetCell(i);
                if ((cell != null) && (cell.CellType != NPOI.SS.UserModel.CellType.Blank))
                {
                    try
                    {
                        switch (dictHeaders[cell.ColumnIndex.ToString()].TrimEnd())
                        {
                            case "TeamKey":
                                this.TeamNameKey = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Team":
                                this.TeamName = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "No.":
                                this.No = cell.StringCellValue;
                                iFieldsChanged++;
                                break;
                            case "Code Name":
                                this.CodeName = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Real Name":
                                this.FirstName = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            case "Last Name":
                                this.LastName = cell.StringCellValue.Trim();
                                iFieldsChanged++;
                                break;
                            default:
                                throw new Exception("Field?");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return (iFieldsChanged > 0);
        }
    }
}
