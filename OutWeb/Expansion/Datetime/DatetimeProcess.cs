using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OutWeb.Expansion.Datetime
{
    public static class DatetimeProcess
    {
        public static string To10CharString(this DateTime? dt)
        {
            return string.Format("{0:yyyy/MM/dd}", (DateTime)dt);
        }

        public static string To10CharString(this DateTime dt)
        {
            return string.Format("{0:yyyy/MM/dd}", dt);
        }

        public static string ToLayDateString(this DateTime dt)
        {
            return string.Format("{0:yyyy-MM-dd}", dt);
        }

        /// <summary>
        /// 將DateTime轉換為10碼字串 (ex. 2017/05/05)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeTo10CodeString(this DateTime dt)
        {
            return string.Format("{0:yyyy\\/MM\\/dd}", dt);
        }

        /// <summary>
        /// 比較兩個日期比取回差異秒數
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static double ComparedDateTimeGetSeconds(this DateTime dt, DateTime dt2)
        {
            double s = new TimeSpan(dt.Ticks - dt2.Ticks).TotalSeconds;
            return s;
        }

        /// <summary>
        /// 比較兩個日期比取回差異天數
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static double ComparedDateTimeGetDays(this DateTime dt, DateTime dt2)
        {
            double s = new TimeSpan(dt.Ticks - dt2.Ticks).TotalDays;
            return s;
        }

        /// <summary>
        /// 將字串 yyyy/MM/dd轉為時間
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDatetime(this string date)
        {
            var addDay = false;

            if (date.Contains("24:"))
            {
                addDay = true;
                date = date.Replace("24:", "00:");
            }

            List<string> dateTimeString = new List<string>()
            {
                "yyyyMMdd",
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "yyyy-MM-dd HH:mm",
                "yyyy/MM/dd HH:mm",
                "yyyy-MM-dd HH:mm:ss"
            };
            DateTime d = new DateTime();
            foreach (var str in dateTimeString)
            {

                bool valid = DateTime.TryParseExact(date, str,
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out d);



                if (valid)
                {
                    if (addDay)
                        d = d.AddDays(1);
                    return d;
                }

            }
            throw new FormatException("Convert To Datetime Has Some Error.");
        }
    }

}