using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
namespace LaserSportDataAPI.CLI
{
    class LaserSportDataAPICLI : ConsoleBase
    {
        protected void ProcessInputExcel()
        {
            int iTaskRecCount = 0;
            int iResourceRecCount = 0;
            string sNewFileName = "";
            Console_Write("Processing  appent to Input XLSX");
            string[] filePathsXLSX = Directory.GetFiles(sPath, "*.xlsx", SearchOption.TopDirectoryOnly);
            string[] filePathsXLS = Directory.GetFiles(sPath, "*.xls", SearchOption.TopDirectoryOnly);
            var filePaths = new string[filePathsXLSX.Length + filePathsXLS.Length];
            filePathsXLSX.CopyTo(filePaths, 0);
            filePathsXLS.CopyTo(filePaths, filePathsXLSX.Length);

            Dictionary<string, string> dictHeaders = new Dictionary<string, string>();

            List<string> arrHeaderNames = new List<string>();
            LsrXlsImport xlimport = new LsrXlsImport(dictHeaders, arrHeaderNames);
            xlimport.OnStatusChange = StatusChange;
            foreach (string sFileName in filePaths)
            {
                xlimport.ParseEventString(sEventMetaData);
                xlimport.ImportFromExcel(sFileName);

                Console_Write("Record Counts" + "\n");
                Console_Write("--  In recount: " + xlimport.RowCount.ToString() + "\n");
                Console_Write("Excel file has been processed and written to:" + "\n");
                Console_Write(" " + sNewFileName);

            }

        }


        private string sAction = "";
        private string sEventMetaData = "";
        private string sPath = "";
        protected override void RunEvent()
        {
            Init();
            if (sAction.Equals("xlsimport"))
            {
                ProcessInputExcel();
            }

        }
        protected override void ShowUsageEvent()
        {
            Console_WriteLine(@"  /a - Action ");
            Console_WriteLine(@"    xlsimport  - Data Import from that Standard Armageddon Import format");
            Console_WriteLine(@"  /p - path to search for files");
            Console_WriteLine(@"  /e - Event Meta Data 'Event Name|Event code|Schedule Date|Score Method'");
            Console_WriteLine(@"  /a:xlsimport /p:<XLS FILE PATH> /e:<Event Meta Data>");
            // /a:xlsimport /e:"Armageddon 2099|A2099US|6/21/2013 12:00:00 AM|a20xx_v2" /p:"C:\Users\Ricky\Documents\Projects\a20xx\lsdrepo\xls"
        }

        protected override void ProcessCommandLineEvent()
        {
            int i = 0;
            Exists("a", "action", out sAction);
            Exists("p", "path", out sPath);
            Exists("e", "event", out sEventMetaData);
        }

        protected void StatusChange(string message)
        {
            Console_WriteLine(message);
        }

        static void Init()
        {
        }

        protected override void ParmProcessEvent(string[] arr, string parmstr, string key, string val)
        {
            //only used if reading from a parm file

        }

        protected override void ValidateParmsEvent(ref bool CancelRun)
        {
            //if (localvar.length = 0)
            //{
            //    Console_WriteLine("Longvar Must exist!");
            //    CancelRun = true;
            //}
        }


    }
    class MainEntry
    {
        string APPNAME = "LaserSport Data Repository Input CLI Utility V1.0";
        public static void Main(string[] args)
        {
            LaserSportDataAPICLI cns = new LaserSportDataAPICLI();
            cns.ExecMain(args);
        }
    }
}
