using System;


namespace Acr {

    public static class DateTimeExtensions {

        public static DateTime SetTime(this DateTime date, int hour = 1, int minute = 0, int second = 0, int millisecond = 0) {
            return new DateTime(date.Year, date.Month, date.Month, hour, minute, second, millisecond);
        }


        public static DateTime BeginningOfMonth(this DateTime dt) {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
        }


        public static DateTime LastDayOfMonth(this DateTime dt) {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59);
        }


        public static DateTime BeginningOfDay(this DateTime dt) {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }


        public static DateTime EndOfDay(this DateTime dt) {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }


        public static DateTime AddWeekdays(this DateTime dt, int days) {
            for (var i = 0; i < days; i++) {
                while (dt.DayOfWeek.IsWeekend()) {
                    dt = dt.AddDays(1);
                }
                dt = dt.AddDays(1);
            }
            return dt;
        }


        public static bool IsWeekday(this DayOfWeek dayOfWeek) {
            return !dayOfWeek.IsWeekend();
        }


        public static bool IsWeekend(this DayOfWeek dayOfWeek) {
            switch (dayOfWeek) {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return true;
                default :
                    return false;
            }
        }

        public static int DifferenceInDays(this DateTime from, DateTime to) {
            long ticks = from.Ticks - to.Ticks;
            return Math.Abs(TimeSpan.FromTicks(ticks).Days);
        }
    }
}
