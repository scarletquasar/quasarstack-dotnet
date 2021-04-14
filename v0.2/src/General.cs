using System;
using System.Collections.Generic;

namespace quasarStack
{
    public static class QuasarProductInfo
    {
        public static string Name = "quasarStack";
        public static string Version = "[Alpha] [TestVer] 0.1";
    }
    public static class General
    {
        public static List<string> QuasarStackGeneralLogRaw = new List<string>();
        public static List<string> QuasarStackErrorLogRaw = new List<string>();

        public static string ERR_CONVERSION = "An error occurred while performing the data conversion. " +
        "Review the code syntax, the entered values or the execution context.";

        public static void ReportError(string error)
        {
            QuasarStackErrorLogRaw.Add(DateTime.Now + " " + error);
            QuasarStackGeneralLogRaw.Add(DateTime.Now + " " + error);
        }

        public static string GlobalErrorString()
        {
            string finalString = "";
            foreach (string err in QuasarStackErrorLogRaw) 
            { finalString += err + "\n"; }
            return finalString;
        }
    }
}
