using System;
using System.Collections;
using System.Collections.Generic;
namespace Game
{
    /// <summary>
    /// 时间戳工具
    /// 20220427
    /// </summary>
    public static class TimeStamp
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime">可选参数当前日期</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp(DateTime dateTime = default)
        {
            DateTime utcTime;
            if (dateTime == default)
            {
                utcTime = DateTime.UtcNow;
            }
            else
            {
                utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            }
            TimeSpan ts = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime">可选参数当前日期</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStampUTC(DateTime dateTime = default)
        {
            DateTime utcTime;
            if (dateTime == default)
            {
                utcTime = DateTime.Now;
            }
            else
            {
                utcTime = dateTime;
            }
            TimeSpan ts = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime">可选参数当前日期</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStampHM(DateTime dateTime = default)
        {
            DateTime utcTime;
            if (dateTime == default)
            {
                utcTime = DateTime.UtcNow;
            }
            else
            {
                utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            }
            TimeSpan ts = utcTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }


        /// <summary>
        /// 时间戳转换为DataTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDataTime(long timeStamp)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
            DateTime dt = startTime.AddSeconds(timeStamp);
            return dt;
        }


        /// <summary>
        /// 时间戳转换为DataTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDataTimeUTC(long timeStamp)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Utc);
            DateTime dt = startTime.AddSeconds(timeStamp);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);
            return dt;
        }
    }
}