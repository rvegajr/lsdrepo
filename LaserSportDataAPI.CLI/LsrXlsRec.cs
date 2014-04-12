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

        public bool FromIRow(IRow row)
        {
            int iFieldsChanged = 0;
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
}
