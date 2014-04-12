using System;
using System.Collections.Generic;
using System.Text;
using CommandLine.Utility;
using System.IO;

namespace LaserSportDataAPI.CLI
{
    public delegate void ParmProcessEvent(string[] arr, string parmstr, string key, string val);
    public delegate void ValidateParmsEvent(ref bool CancelRun);
    public delegate void RunEvent();
    public delegate void ShowUsageEvent();
    public delegate void ProcessCommandLineEvent();
    public delegate void ConsoleWriteEvent(string strToWrite);
    public interface IConsoleWork
    {
        void DoParmProcess(string[] arr, string parmstr, string key, string val);
        void DoValidateParms(ref bool CancelRun);
        void DoRun();
        void DoShowUsage();
        void DoProcessCommandLine();
    }

    public abstract class ConsoleBase 
    {
        protected CommandLine.Utility.Arguments CommandLine;
        protected bool bSilentRun = false;
        protected bool bShowHelp = false;
        protected string sAppName = "";
        protected string sThisEXEPath = "";
        protected string sThisAssemblyName = "";
        protected string sSourcePath = "";
        protected string sTargetPath = "";
        protected string sParmFileName = "";
        protected bool bDebug = false;
        protected abstract void ParmProcessEvent(string[] arr, string parmstr, string key, string val);
        protected abstract void ValidateParmsEvent(ref bool CancelRun);
        protected abstract void RunEvent();
        protected abstract void ShowUsageEvent();
        protected abstract void ProcessCommandLineEvent();
        public string LogFile
        {
            get
            {
                return Log.FileName;
            }
            set
            {
                Log.FileName = value;
            }
        }
        public void Init()
        {
        }

        public string Var(string commandLineParmName)
        {
            return CommandLine[commandLineParmName];
        }

        public string Var(string commandLineParmName, string commandLineParmName2)
        {
            if (Exists(commandLineParmName))
            {
                return Var(commandLineParmName);
            }
            else if (Exists(commandLineParmName2))
            {
                return Var(commandLineParmName2);
            }
            return null;
        }

        public bool Exists(string commandLineParmName)
        {
            return (CommandLine[commandLineParmName]!=null);
        }

        public bool Exists(string commandLineParmName, string commandLineParmName2, out string val)
        {
            val = "";
            if (Exists(commandLineParmName) || Exists(commandLineParmName2))
            {
                val = Var(commandLineParmName, commandLineParmName2);
                return true;
            }
            return false;
        }

        public bool ExistsInt(string commandLineParmName, string commandLineParmName2, out int intval)
        {
            intval = 0;
            if (Exists(commandLineParmName) || Exists(commandLineParmName2))
            {
                string str = Var(commandLineParmName, commandLineParmName2);
                int.TryParse(str, out intval);
                return true;
            }
            return false;
        }

        public bool Exists(string commandLineParmName, string commandLineParmName2, out bool boolval)
        {
            string str = "";
            if (Exists(commandLineParmName, commandLineParmName2, out str))
            {
                boolval = ((str != null) && (str.ToLower().StartsWith("y")));
                return true;
            }
            else
            {
                boolval = false;
                return false;
            }
        }

        public void ExecMain(string[] args)
        {
            sThisEXEPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            sThisEXEPath = sThisEXEPath.Substring(0, sThisEXEPath.LastIndexOf("\\") + 1);
            sThisAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            sThisAssemblyName = sThisAssemblyName.Substring(0, sThisAssemblyName.IndexOf(","));
            if (sAppName.Length == 0)
            {
                sAppName = sThisAssemblyName;
            }

            Log.FileName = sThisEXEPath + sAppName + ".log";
            CommandLine = new CommandLine.Utility.Arguments(args);
            ProcessCmdArgs(CommandLine);
            Log.LogKill();
            Console_WriteLine("Starting " + sAppName + " on " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString());
            ProcessCommandLineEvent();
            if (args.Length == 0)
            {
                bShowHelp = true;
            }
            if (bShowHelp)
            {
                ShowUsage();
            }
            else
            {
                if (CommandLine["parm"] != null)
                {
                    sParmFileName = CommandLine["parm"];
                    String sParmFilePath = sParmFileName.Substring(0, sParmFileName.LastIndexOf("\\") + 1);
                    if (sParmFilePath.Length == 0)
                    {
                        sParmFileName = sThisEXEPath + sParmFileName;
                    }
                    if (!System.IO.File.Exists(sParmFileName))
                    {
                        throw new ArgumentException("Parm File " + sParmFileName + " that is specified on -p on the commandl line does not exist.");
                    }
                    ProcessParmFile(sParmFileName);
                }

                if (ValidateAndShowRunParameters())
                {

                }
            }
            Console_WriteLine("Ending " + sAppName + " on " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString());
        }

        public  bool ValidateAndShowRunParameters()
        {
            if (!File.Exists(sSourcePath))  //Is the source path a file?  then it should exist
            {
                if (!Directory.Exists(sSourcePath))
                { //Is the source path a directory?  then it should exist
                    if (!sSourcePath.EndsWith("\\"))
                    {
                        sSourcePath += "\\"; //append a path delimiter and see if it exists again?
                    }
                    if (!Directory.Exists(sSourcePath))
                    {
                        throw new ArgumentOutOfRangeException("SourcePath", "The source path needs to either be a file or a directory and it must exist");
                    }
                }
            }

            if (!sTargetPath.EndsWith("\\"))
            {
                sTargetPath += "\\"; //append a path delimiter and see if it exists again?
            }
            bool CancelExec = false;
            this.ValidateParmsEvent(ref CancelExec );
            if (!CancelExec)
            {
                RunEvent();
            }

            return true;
        }

        public  void Console_WriteLine(string outdata)
        {
            Log.WriteLine(outdata);
            Console.WriteLine(outdata);
            System.Diagnostics.Trace.WriteLine(outdata);
        }
        public  void Console_Write(string outdata)
        {
            Log.Write(outdata);
            Console.Write(outdata);
            System.Diagnostics.Trace.Write(outdata);
        }

        public bool WriteToFile(string FileName, string strToWrite)
        {
            TextWriter tw = new StreamWriter(FileName, true);
            tw.Write(strToWrite);
            tw.Close();
            return true;
        }

        public bool WriteLineToFile(string FileName, string lineToWrite)
        {
            TextWriter tw = new StreamWriter(FileName, true);
            tw.WriteLine(lineToWrite);
            tw.Close();
            return true;
        }
        public  void ProcessParmFile(string path)
        {
            string str;
            string key = "";
            string val = "";
            StreamReader strmIn = new StreamReader(path);
            try
            {
                while ((str = strmIn.ReadLine()) != null)
                {
                    if (!str.StartsWith("#"))
                    {
                        if (str.Contains("="))
                        {
                            string[] arr = str.Split('=');
                            key = arr[0].ToLower();
                            if (arr.Length > 1)
                            {
                                val = arr[1];
                            }
                            else
                            {
                                val = "";
                            }
                            ParmProcessEvent(arr, str, key, val);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Argument '" + key + "' in parm file " + path + " was invalid.", ex);
            }
            finally
            {
                strmIn.Close();
            }
        }

        public  void ProcessCmdArgs(CommandLine.Utility.Arguments CommandLine)
        {
            try
            {
                if (CommandLine["source"] != null)
                {
                    sSourcePath = CommandLine["source"];
                }
                if (CommandLine["s"] != null)
                {
                    sSourcePath = CommandLine["s"];
                }
                if (CommandLine["target"] != null)
                {
                    sTargetPath = CommandLine["target"];
                }
                if (CommandLine["t"] != null)
                {
                    sTargetPath = CommandLine["t"];
                }
                bSilentRun = (CommandLine["s"] != null);
                bDebug = (CommandLine["d"] != null);
                bShowHelp = ((CommandLine["help"] != null) || (CommandLine["h"] != null) || (CommandLine["?"] != null));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Argument from the command line was invalid.", ex);
            }
            finally
            {
            }
        }

        public  void ShowUsage()
        {
            Console_WriteLine(@"" + sThisAssemblyName  + " can obtain its values via command line or through the -p <file name> parameter.");
            Console_WriteLine(@"C:\>" + sThisAssemblyName  + ".exe ");
            Console_WriteLine(@"  [-h] [-help] or nothing - This command line output");
            Console_WriteLine(@"  [-p=<path>] - file name of config file ");
            Console_WriteLine(@"  [-s] - Silent Run (without user prompts)");
            Console_WriteLine(@"  [-d] - debugging messages");
            ShowUsageEvent();
            Console_WriteLine(@"");
            Console_WriteLine(@"Press Any Key to Continue....");
            Console.ReadKey();
        }


    }
}
