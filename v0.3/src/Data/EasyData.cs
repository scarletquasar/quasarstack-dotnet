using System;
using System.IO;
using System.Text;
using System.Linq;
using quasarStack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters.Binary;

namespace quasarStack.Data
{
    public static class EasyData
    {
        public static string VarDump(object obj)
        {
            long ObjectByteSize = 0;

            string Var_Dump = "";
            string ParsedObject = StringConvert(obj);

            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, obj);
                ObjectByteSize = s.Length;
            }

            Var_Dump += "-> MonoObject: " + nameof(obj) + "\n";
            Var_Dump += "-> Object Length: " + ParsedObject.Length + "\n";
            Var_Dump += "-> Object Type: " + obj.GetType() + "\n";
            Var_Dump += "-> Approx. Object Byte Size: " + ObjectByteSize + "\n";

            return Var_Dump;
        }
        public static string VarDump(object[] obj)
        {
            long ObjectByteSize = 0;
            int CharLength = 0;

            string Var_Dump = "";

            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, obj);
                ObjectByteSize = s.Length;
            }

            foreach(char c in StringConvert(obj))
            {
                CharLength++;
            }

            Var_Dump += "-> MultiObject: " + nameof(obj) + "\n";
            Var_Dump += "-> Object (Total Items) Length: " + obj.Length + "\n";
            Var_Dump += "-> Object (Total Chars) Length: " + CharLength + "\n";
            Var_Dump += "-> Object Type: " + obj.GetType() + "\n";
            Var_Dump += "-> Approx. Object Byte Size: " + ObjectByteSize + "\n";

            return Var_Dump;
        }

        //======================================

        //======================================
        /*
		 * ==================================================================
 		 *  CONVERT VALUE TO STRING ALLOWING OPERATIONS
 		 * ==================================================================
 		 */

        //======================================
        public static string StringConvert(object args)
        {
            return string.Join("", args);
        }

        public static string StringConvert(object[] args)
        {
            string FinalParsed = "";
            foreach (object i in args) { FinalParsed += string.Join("\n", i); }
            return FinalParsed;
        }
        //======================================

        /*
         * ==================================================================
         *  DATA VALIDATION
         * ==================================================================
         */

        public static bool IsEmail(string args)
        {
            var mail = new EmailAddressAttribute();
            return mail.IsValid(args);
        }

        //======================================
        public static bool IsNumber(object args)
        {
            string ParsedObject = StringConvert(args);
            return int.TryParse(ParsedObject, out int n);
        }

        public static bool IsNumber(char args)
        {
            return int.TryParse(args.ToString(), out int n);
        }
        //======================================
        public static bool IsTel(string args)
        {
            string TelOnlyNums = args.Replace(")", "").Replace("(", "").Replace("+", "");

            if(TelOnlyNums.Length <= 15)
            {
                return true;
            }

            return false;
        }
        //======================================
        public static bool IsDate(object args, bool DefaultChars = true)
        {
            string TheDate = StringConvert(args);
            if (DefaultChars)
            {
                TheDate = TheDate.Replace("\\", "/");
                return DateTime.TryParse(StringConvert(args), out DateTime date);
            }
            else
            {
                if (TheDate.Length == 10 || TheDate.Length == 8) { return true; }
                return false;
            }
        }
        //======================================[BRAZILIAN ONLY METHODS]
        public static bool IsCPF(string args)
        {
            string CPFOnlyNums = args.Replace(".", "").Replace("-", "");

            foreach (char c in args)
            {
                if (!IsNumber(c) && c != '.' && c != '-')
                {
                    return false;
                }
            }

            if (CPFOnlyNums.Length == 11) { return true; } else { return false; }
        }


        /*
		 * ==================================================================
 		 *  NUMERIC CONVERSION
 		 * ==================================================================
 		 */
        //======================================
        public static int IntConvert(object args)
        {
            string ParseObject = StringConvert(args);
            try
            {
                if (string.IsNullOrEmpty(ParseObject)) return int.Parse(ParseObject);
                StringBuilder sb = new StringBuilder(ParseObject.Length);
                for (int i = 0; i < ParseObject.Length; ++i)
                {
                    char c = ParseObject[i];
                    if (c < '0') continue;
                    if (c > '9') continue;
                    sb.Append(ParseObject[i]);
                }
                string parsedFinal = sb.ToString();

                return int.Parse(parsedFinal);
            }
            catch
            {
                General.ReportError(General.ERR_CONVERSION);
                return 0;
            }
        }
        //======================================
        public static long LongConvert(object args)
        {
            string ParseObject = StringConvert(args);
            try
            {
                if (string.IsNullOrEmpty(ParseObject)) return long.Parse(ParseObject);
                StringBuilder sb = new StringBuilder(ParseObject.Length);
                for (int i = 0; i < ParseObject.Length; ++i)
                {
                    char c = ParseObject[i];
                    if (c < '0') continue;
                    if (c > '9') continue;
                    sb.Append(ParseObject[i]);
                }
                string parsedFinal = sb.ToString();

                return long.Parse(parsedFinal);
            }
            catch
            {
                General.ReportError(General.ERR_CONVERSION);
                return 0;
            }
        }
        //======================================
        public static double DoubleConvert(object args)
        {
            string ParsedObject = StringConvert(args);
            string FinalObject = "";
            bool Pointed = false;
            
            foreach(char c in ParsedObject)
            {
                if (IsNumber(c)) { FinalObject += c; }
                if (c.Equals('.') && !Pointed) { FinalObject += c; Pointed = true; }
            }

            return double.Parse(FinalObject);
        }
        //======================================
        public static float FloatConvert(object args)
        {
            string ParsedObject = StringConvert(args);
            string FinalObject = "";
            bool Pointed = false;

            foreach (char c in ParsedObject)
            {
                if (IsNumber(c)) { FinalObject += c; }
                if (c.Equals('.') && !Pointed) { FinalObject += c; Pointed = true; }
            }

            return float.Parse(FinalObject);
        }

        /*
         * ==================================================================
         *  ARRAY OPERATIONS, DATA ANALYSIS & FILTERING
         * ==================================================================
         */

        //======================================
        //Filter Array Methods: 1 (EQUALS) | 2 (CONTAINS) | 3 (NOT CONTAINS) | 4 (NOT EQUALS)
        public static object[] FilterArray(object[] array, string content, int method)
        {
            List<string> FilterBase = new List<string>();
            List<object> FilterResult = new List<object>();

            foreach(object o in array) { FilterBase.Add(StringConvert(o)); }

            try
            {
                switch (method)
                {
                    case 1:
                        foreach (string i in FilterBase) { if (i == content) FilterResult.Add(i); }
                        break;

                    case 2:
                        foreach (string i in FilterBase) { if (i.Contains(content)) FilterResult.Add(i); }
                        break;

                    case 3:
                        foreach (string i in FilterBase) { if (!i.Contains(content)) FilterResult.Add(i); }
                        break;

                    case 4:
                        foreach (string i in FilterBase) { if (i != content) FilterResult.Add(i); }
                        break;
                    default:
                        General.ReportError(General.ERR_LOGIC); return null;
                }
            }
            catch { }
            return FilterResult.ToArray();
        }
        //======================================
        /* NOTE: Numeric array filters will return a string array due to some problems when returning #numeric#[]
         * This problem will be solved in future versions */
        public static string[] FilterIntArray(int[] array, int content)
        {
            List<string> FilterResult = new List<string>();
            foreach (int i in array) { if (i == content) FilterResult.Add(StringConvert(i)); }
            return FilterResult.ToArray();
        }
        //======================================
        public static string[] FilterLongArray(long[] array, long content)
        {
            List<string> FilterResult = new List<string>();
            foreach (long i in array) { if (i == content) FilterResult.Add(StringConvert(i)); }
            return FilterResult.ToArray();
        }
        //======================================
        public static string[] FilterDoubleArray(double[] array, double content)
        {
            List<string> FilterResult = new List<string>();
            foreach (double i in array) { if (i == content) FilterResult.Add(StringConvert(i)); }
            return FilterResult.ToArray();
        }
        //======================================
        public static string[] FilterFloatArray(float[] array, float content)
        {
            List<string> FilterResult = new List<string>();
            foreach (float i in array) { if (i == content) FilterResult.Add(StringConvert(i)); }
            return FilterResult.ToArray();
        }
        //======================================
        //Push (add in the end) item to the [String] array
        public static string[] StringPush(string[] toAdd, string whatToAdd)
        {
            try
            {
                //ADD STRING TO THE LAST ARRAY POSITION
                List<string> originList = new List<string>(toAdd);
                List<string> unionList = new List<string>();

                foreach (string item in originList)
                {
                    unionList.Add(item);
                }

                unionList.Add(whatToAdd);

                return unionList.ToArray();
            }
            catch
            {
                General.ReportError(General.ERR_LOGIC);
                return new string[1] { "e0" };
            }
        }
        //Stack (add in the start) item to the [String] array
        public static string[] StringStack(string[] toAdd, string whatToAdd)
        {
            try
            {
                //ADD STRING TO THE FIRST ARRAY POSITION
                List<string> originList = new List<string>(toAdd);
                List<string> unionList = new List<string>();

                unionList.Add(whatToAdd);

                foreach (string item in originList)
                {
                    unionList.Add(item);
                }

                return unionList.ToArray();
            }
            catch
            {
                General.ReportError(General.ERR_LOGIC);
                return null;
            }
        }
        //======================================
        //Push (add in the end) item to the [String] array
        public static int[] IntPush(int[] toAdd, int whatToAdd)
        {
            try
            {
                //ADD INT TO THE FIRST ARRAY POSITION
                List<int> originList = new List<int>(toAdd);
                List<int> unionList = new List<int>();

                unionList.Add(whatToAdd);

                foreach (int item in originList)
                {
                    unionList.Add(item);
                }

                return unionList.ToArray();
            }
            catch
            {
                General.ReportError(General.ERR_LOGIC);
                return new int[1] { 0 };
            }
        }
        //Stack (add in the start) item to the [Int] array
        public static int[] IntStack(int[] toAdd, int whatToAdd)
        {
            try
            {
                //ADD INT TO THE LAST ARRAY POSITION
                List<int> originList = new List<int>(toAdd);
                List<int> unionList = new List<int>();

                foreach (int item in originList)
                {
                    unionList.Add(item);
                }

                unionList.Add(whatToAdd);
                return unionList.ToArray();
            }
            catch
            {
                General.ReportError(General.ERR_LOGIC);
                return new int[1] { 0 };
            }
        }
        //======================================
        public static char[] CharSplit(object ToSplit)
        {
            string SplitObject = StringConvert(ToSplit);
            List<char> SplitList = new List<char>();

            foreach(char c in SplitObject)
            {
                SplitList.Add(c);
            }

            return SplitList.ToArray();
        }
        //======================================
        public static string ArrayImp(object[] toParse, string method = "default")
        {

            string finalParse = "";
            if (method == "default")
            {
                return StringConvert(toParse);
            }

            if (method == "list-array")
            {

                foreach (string s in toParse)
                {
                    finalParse += Environment.NewLine + s;
                }
                return finalParse;
            }

            return null;
        }
        //======================================
        public static string[] StringExp(string ToExplode, string howExplode)
        {
            string tempLine = "";
            List<string> finalList = new List<string>();

            foreach(char c in ToExplode)
            {
                if (c.Equals(howExplode)) 
                {
                    finalList.Add(tempLine);
                    tempLine = "";
                }
                else
                {
                    tempLine += c;
                }
            }

            return finalList.ToArray();
        }
        //======================================
        public static string[] StringArrJoin(string[] arr1, string[] arr2)
        {
            List<string> FinalList = new List<string>();

            foreach (string item in arr1) { FinalList.Add(item); }
            foreach (string item in arr2) { FinalList.Add(item); }

            return FinalList.ToArray();
        }
        //======================================
        public static int[] IntArrJoin(int[] arr1, int[] arr2)
        {
            List<int> FinalList = new List<int>();

            foreach (int item in arr1) { FinalList.Add(item); }
            foreach (int item in arr2) { FinalList.Add(item); }

            return FinalList.ToArray();
        }
        //======================================
        public static long[] LongArrJoin(long[] arr1, long[] arr2)
        {
            List<long> FinalList = new List<long>();

            foreach (long item in arr1) { FinalList.Add(item); }
            foreach (long item in arr2) { FinalList.Add(item); }

            return FinalList.ToArray();
        }
        //======================================
        public static string[] StringLoop(string StringToRepeat, int Times)
        {
            List<string> FinalList = new List<string>();
            for(int i = Times; i > 0; i--)
            {
                FinalList.Add(StringToRepeat);
            }
            return FinalList.ToArray();
        }
        //======================================
        public static int[] IntLoop(int IntToRepeat, int Times)
        {
            List<int> FinalList = new List<int>();
            for (int i = Times; i > 0; i--)
            {
                FinalList.Add(IntToRepeat);
            }
            return FinalList.ToArray();
        }
    }
}
