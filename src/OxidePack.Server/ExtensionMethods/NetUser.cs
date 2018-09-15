using Ether.Network.Common;

namespace OxidePack.Server
{
    public static class NetUserEx
    {
        public static string GetIP(this NetUser user) => user.Socket.GetIP();
    }
}