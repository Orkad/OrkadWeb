namespace System
{
    /// <summary>
    /// Define <see cref="DateTime"/> extensions methods
    /// </summary>
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = dateTime.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
