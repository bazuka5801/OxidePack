using System;

namespace OxidePack
{
    public static class Epoch
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static ulong Current => (ulong)DateTime.UtcNow.Subtract(epoch).TotalSeconds;

        public static ulong FromDateTime(DateTime time)
        {
            return (ulong)time.Subtract(epoch).TotalSeconds;
        }

        public static DateTime ToDateTime(decimal unixTime)
        {
            return epoch.AddSeconds((long)unixTime);
        }
    }
}