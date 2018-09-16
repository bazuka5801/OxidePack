using System.Net;
using System.Net.Sockets;

namespace OxidePack
{
    public static class SocketEx
    {
        public static string GetIP(this Socket socket)
        {
            return ((IPEndPoint) socket.RemoteEndPoint).Address.ToString();
        }
    }
}