using System.Net.Sockets;
using System.Threading;
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
        private bool _needReconnect;
        
        public Client(string host, int port, int bufferSize)
        {
            this.Configuration.Host = host;
            this.Configuration.Port = port;
            this.Configuration.BufferSize = bufferSize;
            this.Configuration.RetryMode = NetClientRetryOptions.Limited;
            this.Configuration.MaxRetryAttempts = 5;
            this.Configuration.TimeOut = 1000;
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
            MainForm.UpdateStatus("(3/3) Authentication...");
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
                MainForm.UpdateStatus($"(3/3) Auth error: {result}");
                MainForm.UpdateProgressBar(value: 3, max: 3);
                return;
            }
            MainForm.UpdateStatus($"Authenticated!");
            MainForm.UpdateProgressBar(value: 0);
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
            _needReconnect = true;
        }

        protected override void OnSocketError(SocketError socketError)
        {
            ConsoleSystem.LogError($"[Client] SocketError: {socketError}");
        }

        public void WorkingLoop()
        {
            Connect();
            while (true)
            {
                Thread.Sleep(1000);
                
                if (IsConnected)
                    continue;
                
                if (_needReconnect)
                {
                    _needReconnect = false;
                    MainForm.UpdateStatus("(2/3) Connecting to server...");
                    MainForm.UpdateProgressBar(value: 2, max: 3, ProgressBarStyle.Continuous);
                    Connect();
                }
            }
            MainForm.UpdateStatus("Connection Failed - Unresponsive");
        }
    }
}