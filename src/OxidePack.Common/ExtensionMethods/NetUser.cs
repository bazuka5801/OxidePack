using Ether.Network.Common;

namespace OxidePack
{
    public static class NetUserEx
    {
        public static string GetIP(this NetUser user) => user.Socket.GetIP();

        public static bool IsConnected(this NetUser user) => user.Socket.Connected;
    }
}