using System;

namespace DaiPhucVinh.Services.Helper
{
    public static class TimeKey
    {
        public const string Custom = "custom";
        public const string Today = "today";
        public const string Yesterday = "yesterday";
        public const string ThisWeek = "thisWeek";
        public const string PreviousWeek = "previousWeek";
        public const string FromThisWeekToToday = "fromThisWeekToToday";
        public const string ThisMonth = "thisMonth";
        public const string PreviousMonth = "previousMonth";
        public const string FromThisMonthToToday = "fromThisMonthToToday";
        public const string ThisQuarter = "thisQuarter";
        public const string PreviousQuarter = "previousQuarter";
        public const string FromThisQuarterToToday = "fromThisQuarterToToday";
        public const string ThisYear = "thisYear";
        public const string FromThisYearToToday = "fromThisYearToToday";
        public const string FirstHalfYear = "firstHalfYear";
        public const string LastHalfYear = "lastHalfYear";
        public const string January = "january";
        public const string February = "february";
        public const string March = "march";
        public const string April = "april";
        public const string May = "may";
        public const string June = "june";
        public const string July = "july";
        public const string August = "august";
        public const string September = "september";
        public const string October = "october";
        public const string November = "november";
        public const string December = "december";
        public const string QuarterI = "quarterI";
        public const string QuarterII = "quarterII";
        public const string QuarterIII = "quarterIII";
        public const string QuarterIV = "quarterIV";
    }
    public static class DateTimeExtensions
    {
        public static Tuple<DateTime, DateTime> GetDateTimeByKey(string key)
        {
            var now = DateTime.Now;
            DateTime fromDt = now, toDt = now;
            switch (key)
            {
                case "today":
                    fromDt = StartOfDay(now);
                    toDt = EndOfDay(now);
                    break;
                case "yesterday":
                    fromDt = StartOfDay(now).AddDays(-1);
                    toDt = EndOfDay(now).AddDays(-1);
                    break;
                case "thisWeek":
                    fromDt = now.StartOfWeek();
                    toDt = fromDt.AddDays(7).AddSeconds(-1);
                    break;
                case "previousWeek":
                    fromDt = now.StartOfWeek().AddDays(-7);
                    toDt = fromDt.AddDays(7).AddSeconds(-1);
                    break;
                case "fromThisWeekToToday":
                    fromDt = now.StartOfWeek();
                    toDt = EndOfDay(now.Date);
                    break;
                case "thisMonth":
                    fromDt = FirstDayOfMonth(now);
                    toDt = LastDayOfMonth(now);
                    break;
                case "previousMonth":
                    fromDt = FirstDayOfMonth(now.AddMonths(-1));
                    toDt = LastDayOfMonth(now.AddMonths(-1));
                    break;
                case "fromThisMonthToToday":
                    fromDt = FirstDayOfMonth(now);
                    toDt = EndOfDay(now.Date);
                    break;
                case "thisQuarter":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, FirstMonthOfQuarter(now), 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, FirstMonthOfQuarter(now) + 2, 1));
                    break;
                case "previousQuarter":
                    now = now.AddMonths(-3);
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, FirstMonthOfQuarter(now), 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, FirstMonthOfQuarter(now) + 2, 1));
                    break;
                case "fromThisQuarterToToday":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, FirstMonthOfQuarter(now), 1));
                    toDt = EndOfDay(now.Date);
                    break;
                case "thisYear":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 1, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 12, 1));
                    break;
                case "fromThisYearToToday":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 1, 1));
                    toDt = EndOfDay(now.Date);
                    break;
                case "firstHalfYear":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 1, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 6, 1));
                    break;
                case "lastHalfYear":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 7, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 12, 1));
                    break;
                case "january":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 1, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 1, 1));
                    break;
                case "february":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 2, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 2, 1));
                    break;
                case "march":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 3, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 3, 1));
                    break;
                case "april":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 4, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 4, 1));
                    break;
                case "may":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 5, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 5, 1));
                    break;
                case "june":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 6, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 6, 1));
                    break;
                case "july":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 7, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 7, 1));
                    break;
                case "august":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 8, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 8, 1));
                    break;
                case "september":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 9, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 9, 1));
                    break;
                case "october":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 10, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 10, 1));
                    break;
                case "november":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 11, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 11, 1));
                    break;
                case "december":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 12, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 12, 1));
                    break;
                case "quarterI":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 1, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 3, 1));
                    break;
                case "quarterII":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 4, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 6, 1));
                    break;
                case "quarterIII":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 7, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 9, 1));
                    break;
                case "quarterIV":
                    fromDt = FirstDayOfMonth(new DateTime(now.Year, 10, 1));
                    toDt = LastDayOfMonth(new DateTime(now.Year, 12, 1));
                    break;
            }
            return new Tuple<DateTime, DateTime>(fromDt, toDt);
        }
        private static DateTime StartOfDay(DateTime day) => day.Date;
        private static DateTime EndOfDay(DateTime day) => day.Date.AddDays(1).AddSeconds(-1);
        private static DateTime LastDayOfMonth(this DateTime day) => EndOfDay(new DateTime(day.Year, day.Month, day.DaysInMonth()));
        private static DateTime FirstDayOfMonth(this DateTime day) => new DateTime(day.Year, day.Month, 1);
        public static int DaysInMonth(this DateTime day) => DateTime.DaysInMonth(day.Year, day.Month);
        public static int FirstMonthOfQuarter(this DateTime day)
        {
            if (day.Month <= 3)
                return 1;
            if (day.Month <= 6)
                return 4;
            if (day.Month <= 9)
                return 7;
            return 10;
        }
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
