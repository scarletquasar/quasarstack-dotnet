using System;
using System.Collections.Generic;
using System.Text;

namespace quasarStack.Data
{
    public static class Standardify
    {
        public static int[] GenInt(int xfrom, int to)
        {
            List<int> FinalList = new List<int>();
            while (xfrom < to)
            {
                FinalList.Add(xfrom);
                xfrom++;
            }
            while (xfrom > to)
            {
                FinalList.Add(xfrom);
                xfrom--;
            }
            if (xfrom == to) { FinalList.Add(to); }
            return FinalList.ToArray();
        }

        public static string[] GenStandard(string type, int start, int stop)
        {

            switch(type)
            {
                case "month":
                    return InternalGetMonthStandard(start, stop);

                case "week":
                    return InternalGetWeekStandard(start, stop);
            }

            General.ReportError(General.ERR_DATAMAKE);
            return null;
            
        }

        static string[] InternalGetMonthStandard(int start, int stop)
        {
            int[] monthNumList = GenInt(start, stop);
            List<string> FinalList = new List<string>();
            int Tester = 0;

            foreach (int m in monthNumList)
            {
                Tester++;
                if (Tester > 12) { Tester = 1; }
                switch (Tester)
                {
                    case 1:
                        FinalList.Add("January");
                        break;

                    case 2:
                        FinalList.Add("February");
                        break;

                    case 3:
                        FinalList.Add("March");
                        break;

                    case 4:
                        FinalList.Add("April");
                        break;

                    case 5:
                        FinalList.Add("May");
                        break;

                    case 6:
                        FinalList.Add("June");
                        break;

                    case 7:
                        FinalList.Add("July");
                        break;

                    case 8:
                        FinalList.Add("August");
                        break;

                    case 9:
                        FinalList.Add("September");
                        break;

                    case 10:
                        FinalList.Add("October");
                        break;

                    case 11:
                        FinalList.Add("November");
                        break;

                    case 12:
                        FinalList.Add("December");
                        break;

                    default:
                        General.ReportError(General.ERR_LOGIC);
                        break;
                }
            }

            return FinalList.ToArray();

        }

        static string[] InternalGetWeekStandard(int start, int stop)
        {
            int[] weekNumList = GenInt(start, stop);
            List<string> FinalList = new List<string>();
            int Tester = 0;

            foreach (int m in weekNumList)
            {
                Tester++;
                if (Tester > 7) { Tester = 1; }
                switch (Tester)
                {
                    case 1:
                        FinalList.Add("Sunday");
                        break;

                    case 2:
                        FinalList.Add("Monday");
                        break;

                    case 3:
                        FinalList.Add("Tuesday");
                        break;

                    case 4:
                        FinalList.Add("Wednesday");
                        break;

                    case 5:
                        FinalList.Add("Thursday");
                        break;

                    case 6:
                        FinalList.Add("Friday");
                        break;

                    case 7:
                        FinalList.Add("Saturday");
                        break;

                    default:
                        General.ReportError(General.ERR_LOGIC);
                        break;
                }
            }

            return FinalList.ToArray();

        }
    }
}
