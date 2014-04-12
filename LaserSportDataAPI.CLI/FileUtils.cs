using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Text;
namespace LaserSportDataAPI.CLI
{

    public static class PathHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(

                 [MarshalAs(UnmanagedType.LPTStr)]

                   string path,

                 [MarshalAs(UnmanagedType.LPTStr)]

                   StringBuilder shortPath,

                 int shortPathLength

                 );

        public static string GetShortPathName(string pathName)
        {
            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(pathName, shortPath, shortPath.Capacity);
            return shortPath.ToString();
        }
    }
    public static class Log
    {
        public static string FileName { get; set; }
        public static bool LogKill()
        {
            System.IO.File.Delete(FileName);
            return true;
        }

        public static bool Write(string strToWrite)
        {
            TextWriter tw = new StreamWriter(FileName, true);
            tw.Write(strToWrite);
            tw.Close();
            return true;
        }

        public static bool WriteLine(string lineToWrite)
        {
            TextWriter tw = new StreamWriter(FileName, true);
            tw.WriteLine(lineToWrite);
            tw.Close();
            return true;
        }
    }
}
