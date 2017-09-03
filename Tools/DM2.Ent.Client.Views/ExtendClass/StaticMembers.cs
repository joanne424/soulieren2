namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;

    public static class StaticMembers
    {
        #region 时间

        /// <summary>
        /// 本月月初（第一天）
        /// </summary>
        public static DateTime StartDayOfMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day);

        /// <summary>
        /// 本月月末（最后一天）
        /// </summary>
        public static DateTime EndDayOfMonth = DateTime.Now.AddMonths(1).AddDays(0 - DateTime.Now.Day);

        /// <summary>
        /// 去年的今天
        /// </summary>
        public static DateTime LastYear = DateTime.Now.AddYears(-1);

        /// <summary>
        /// 去年的一月一号
        /// </summary>
        public static DateTime LastYearFirstDay = DateTime.Now.AddYears(-1).AddMonths(1 - DateTime.Now.Month).AddDays(1 - DateTime.Now.Day);
        // Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy/MM/dd HH:mm:cs").Substring(0, 4) + "01/01 00:00:00");

        /// <summary>
        /// 上月的今天
        /// </summary>
        public static DateTime LastMonth = DateTime.Now.AddMonths(-1);

        /// <summary>
        /// 上月的一号
        /// </summary>
        public static DateTime LastMonthFirstDay = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day);

        /// <summary>
        /// 上一个星期的今天
        /// </summary>
        public static DateTime LastWeek = DateTime.Now.AddDays(-7);

        /// <summary>
        /// 上一个星期的星期一
        /// </summary>
        public static DateTime LastWeekFirstDay = DateTime.Now.AddDays(1 - 7 - getDayofWeek(DateTime.Now));

        /// <summary>
        /// 昨天
        /// </summary>
        public static DateTime Yesterday = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 前天
        /// </summary>
        public static DateTime TheDayBeforeYesterday = DateTime.Now.AddDays(-2);

        /// <summary>
        /// 一年后的今天
        /// </summary>
        public static DateTime NextYear = DateTime.Now.AddYears(1);

        /// <summary>
        /// 一年后的一月一号
        /// </summary>
        public static DateTime NextYearFirstDay = DateTime.Now.AddYears(1).AddMonths(1 - DateTime.Now.Month).AddDays(1 - DateTime.Now.Day);

        /// <summary>
        /// 下月的今天
        /// </summary>
        public static DateTime NextMonth = DateTime.Now.AddMonths(1);

        /// <summary>
        /// 下月的一号
        /// </summary>
        public static DateTime NextMonthFirstDay = DateTime.Now.AddMonths(1).AddDays(1 - DateTime.Now.Day);

        /// <summary>
        /// 下星期的今天
        /// </summary>
        public static DateTime NextWeek = DateTime.Now.AddDays(7);

        /// <summary>
        /// 下星期的星期一
        /// </summary>
        public static DateTime NextWeekFirstDay = DateTime.Now.AddDays(8 - getDayofWeek(DateTime.Now));

        /// <summary>
        /// 明天
        /// </summary>
        public static DateTime Tomorrow = DateTime.Now.AddDays(1);

        /// <summary>
        /// 后天
        /// </summary>
        public static DateTime TheDayAfterTomorrow = DateTime.Now.AddDays(2);

        /// <summary>
        /// 获取日期为这一周中的第几天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>第几天</returns>
        private static int getDayofWeek(DateTime dt)
        {
            int days = 0;

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    days = 1;
                    break;
                case DayOfWeek.Tuesday:
                    days = 2;
                    break;
                case DayOfWeek.Wednesday:
                    days = 3;
                    break;
                case DayOfWeek.Thursday:
                    days = 4;
                    break;
                case DayOfWeek.Friday:
                    days = 5;
                    break;
                case DayOfWeek.Saturday:
                    days = 6;
                    break;
                case DayOfWeek.Sunday:
                    days = 7;
                    break;
            }

            return days;
        }

        #endregion
    }
}
