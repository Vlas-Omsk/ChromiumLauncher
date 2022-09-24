using System;

namespace ChromiumLauncher
{
    internal static class TimeConverter
    {
        public static DateTime FromUtcTime(long utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * utcTime), TimeZoneInfo.Local);
        }

        public static long ToUtcTime(DateTime time)
        {
            return time == DateTime.MinValue ? 0 : TimeZoneInfo.ConvertTimeToUtc(time, TimeZoneInfo.Local).ToFileTime() / 10;
        }

        public static DateTime FromUnixTimestamp(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        }
    }
}
