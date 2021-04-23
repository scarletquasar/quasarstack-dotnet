using System;
using System.Collections.Generic;

namespace quasarStack
{
    public static class QuasarProductInfo
    {
        public static string Name = "quasarStack";
        public static string Version = "[Alpha] [ReleaseVer] 0.3"; //LAST UPDATE DAY 19 04/2021
    }
    public static class General
    {
        public static string NETWORK_IP_API = "api.ipify.org";

        public static List<string> QuasarStackGeneralLogRaw = new List<string>();
        public static List<string> QuasarStackErrorLogRaw = new List<string>();

        public static string ERR_CONVERSION = "[MEDIAN] An error occurred while performing the data conversion, " +
        "review the code syntax, the entered values or the execution context.";
        public static string ERR_LOGIC = "[COMMON] An undetermined logic error has occurred, " +
        "review the logic used or the syntax implemented. If there's no visible problem in the runtime ignore the warning.";
        public static string ERR_DATAMAKE = "[SEVERE] An undetermined Data Creation error has occurred.";
        public static string ERR_MATH = "[MEDIAN] A mathematical error has occurred, please review the implemented logic";
        public static string ERR_NETWORK = "[MEDIAN] A connection error has occurred, check that the source device is connected to " +
        "the internet and review the network, firewall and antivirus settings.";
        public static string ERR_NETWORK_SERVER = "[COMMON] A connection error has occurred, the target is inaccessible. Please try again.";
        public static string ERR_NETWORK_SYSTEM_INFO = "[MEDIAN] An error occurred when trying to find information about the local network, " +
        "check the program logic, its permissions and if the system is properly configured.";
        public static string ERR_UNKNOWN = "[SEVERE] An unknown error was detected while the program was running.";

        public static string WARN_NOHTTP_RESPONSE = "[WARNING] No HTTP response.";

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
