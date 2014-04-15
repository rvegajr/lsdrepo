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
        protected override void ShowUsageEvent()
        {
            Console_WriteLine(@"  /a - Action ");
            Console_WriteLine(@"    xlsimport  - Data Import from that Standard Armageddon Import format");
            Console_WriteLine(@"    evtrestest - Event Result Test");
            Console_WriteLine(@"  /p - path to search for files");
            Console_WriteLine(@"  /e - Event Meta Data 'Event Name|Event code|Schedule Date|Score Method'");
            Console_WriteLine(@"  /a:xlsimport /p:<XLS FILE PATH> /e:<Event Meta Data>");
            // /a:xlsimport /e:"Armageddon 2099|A2099US|6/21/2013 12:00:00 AM|a20xx_v2" /p:"C:\Users\Ricky\Documents\Projects\a20xx\lsdrepo\xls"
            Console_WriteLine(@"  /a:evtrestest /e:<event id to test>");  // /a:evtrestest /e:8
        }

        protected void ProcessEventResultTest(int EventId)
        {
            var resTest = new LsrEventResultTest();
            resTest.OnStatusChange = StatusChange;
            resTest.EventResultRest(EventId);
        }
        protected void ProcessInputExcel()
        {
            string sNewFileName = "";
            Console_Write("Processing  appent to Input XLSX");
            string[] filePathsXLSX = Directory.GetFiles(Path, "*.xlsx", SearchOption.TopDirectoryOnly);
            string[] filePathsXLS = Directory.GetFiles(Path, "*.xls", SearchOption.TopDirectoryOnly);
            var filePaths = new string[filePathsXLSX.Length + filePathsXLS.Length];
            filePathsXLSX.CopyTo(filePaths, 0);
            filePathsXLS.CopyTo(filePaths, filePathsXLSX.Length);

            Dictionary<string, string> dictHeaders = new Dictionary<string, string>();

            List<string> arrHeaderNames = new List<string>();
            LsrXlsImport xlimport = new LsrXlsImport(dictHeaders, arrHeaderNames);
            xlimport.OnStatusChange = StatusChange;
            foreach (string sFileName in filePaths)
            {
                xlimport.ParseEventString(EventMetaData);
                xlimport.ImportFromExcel(sFileName);

                Console_Write("Record Counts" + "\n");
                Console_Write("--  In recount: " + xlimport.RowCount.ToString() + "\n");
                Console_Write("Excel file has been processed and written to:" + "\n");
                Console_Write(" " + sNewFileName);

            }

        }


        private string Action = "";
        private string EventMetaData = "";
        private int EventId = int.MinValue;
        private string Path = "";
        protected override void RunEvent()
        {
            Init();
            switch (Action)
            {
                case "xlsimport":
                    ProcessInputExcel();
                    break;
                case "evtrestest":
                    if (EventId.Equals(int.MinValue))
                    {
                        Console_WriteLine("/e was not set to a valid numeric,  ending utility.");
                    }
                    else 
                    {
                        ProcessEventResultTest(EventId);
                    }
                    break;
                default:
                    Console_WriteLine("Action '" + Action + "' is unknown, ending Utility.");
                    break;
            }
        }
        protected override void ProcessCommandLineEvent()
        {
            int i = 0;
            Exists("a", "action", out Action);
            Exists("p", "path", out Path);
            Exists("e", "event", out EventMetaData);
            if (!int.TryParse(EventMetaData, out EventId))
            {
                EventId = int.MinValue;
            }
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
