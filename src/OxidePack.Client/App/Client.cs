using System.Net.Sockets;
using System.Windows.Forms;
using Ether.Network.Client;
using Ether.Network.Packets;
using OxidePack.Client.Forms;
using OxidePack.Data;
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
            var msgType = packet.ReadPacketID();
            switch (msgType)
            {
                case PacketType.RequestUserInformation:
                    OnRequestUserInformation();
                    break;
                case PacketType.GiveUserInformationResult:
                    string result = packet.Read<string>();
                    OnGiveUserInformationResult(result);
                    break;
                default:
                    ConsoleSystem.LogError($"{this} sent invalid message");
                    break;
            }
        }

        #region [Method] OnRequestUserInformation
        public void OnRequestUserInformation()
        {
            AuthForm.UpdateStatus("(3/3) Authentication...");
            using (NetPacket packet = new NetPacket())
            {
                packet.WritePackketID(PacketType.GiveUserInformation);
                var uInfo = Pool.Get<UserInformation>();
                uInfo.key = MachineIdentificator.Value();
                uInfo.username = "Test User";
                uInfo.WriteToStream(packet);
                Send(packet);
            }
        }
        #endregion

        #region [Method] OnGiveUserInformationResult
        private void OnGiveUserInformationResult(string result)
        {
            if (string.IsNullOrEmpty(result) == false)
            {
                AuthForm.UpdateStatus($"(3/3) Auth error: {result}");
                return;
            }
            AuthForm.UpdateStatus($"Authenticated!");
            ConsoleSystem.Log("Authed!");
        }
        #endregion

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