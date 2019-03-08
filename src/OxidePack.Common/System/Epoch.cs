using System;

namespace OxidePack
{
    public static class Epoch
    {
        private static readonly DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static ulong Current => (ulong)DateTime.UtcNow.Subtract(epochTime).TotalSeconds;

        public static ulong FromDateTime(DateTime time)
        {
            return (ulong)time.Subtract(epochTime).TotalSeconds;
        }

        public static DateTime ToDateTime(decimal unixTime)
        {
            return epochTime.AddSeconds((long)unixTime);
        }
    }
}