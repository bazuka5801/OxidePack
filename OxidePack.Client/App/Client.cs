using System.Net.Sockets;
using System.Windows.Forms;
using Ether.Network.Client;
using Ether.Network.Packets;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class Client : NetClient
    {
        public Client(string host, int port, int bufferSize)
        {
            this.Configuration.Host = host;
            this.Configuration.Port = port;
            this.Configuration.BufferSize = bufferSize;
        }

        
        
        public override void HandleMessage(INetPacketStream packet)
        {
            var response = packet.Read<string>();
            ConsoleSystem.Log($"-> Server response: '{response}'");
        }

        protected override void OnConnected()
        {
            ConsoleSystem.Log("Connected");
        }

        protected override void OnDisconnected()
        {
            ConsoleSystem.Log("Disconnected");
        }

        protected override void OnSocketError(SocketError socketError)
        {
            ConsoleSystem.LogError($"[Client] SocketError: {socketError}");
        }
    }
}