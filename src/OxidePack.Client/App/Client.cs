using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ether.Network.Client;
using Ether.Network.Packets;
using OxidePack.Client;
using OxidePack.Data;
using SapphireEngine;
using SilentOrbit.ProtocolBuffers;

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


        #region [Method] HandleMessage
        public override void HandleMessage(INetPacketStream packet)
        {
            var stream = (NetPacket) packet;
            var msgType = stream.ReadPacketID();
            switch (msgType)
            {
                case PacketType.RequestUserInformation:
                    OnRequestUserInformation();
                    break;
                case PacketType.GiveUserInformationResult:
                    string result = stream.Read<string>();
                    OnGiveUserInformationResult(result);
                    break;
                case PacketType.RPCMessage:
                    var rpcmessagetype = (RPCMessageType) stream.Read<UInt32>();
                    HandleRPCMessage(rpcmessagetype, stream);
                    break;
                default:
                    ConsoleSystem.LogError($"{this} sent invalid message");
                    break;
            }
        }
        #endregion

        #region [Method] HandleRPCMessage
        public virtual void HandleRPCMessage(RPCMessageType type, NetPacket stream)
        {
            
        }
        #endregion

        #region [Method] SendRPC
        public void SendRPC(RPCMessageType type, params IProto[] args)
        {
            using (NetPacket packet = new NetPacket())
            {
                packet.WritePackketID(PacketType.RPCMessage);
                packet.Write((uint)type);
                for (var i = 0; i < args.Length; i++)
                {
                    args[i].WriteToStream(packet);
                }
                Send(packet);
            }
        }
        #endregion

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
            Task.Run(MainForm.Instance.CheckForUpdates);
            ConsoleSystem.Log("Authed!");
        }
        #endregion

        #region [Method] OnConnected
        protected override void OnConnected()
        {
            ConsoleSystem.Log("Connected");
        }
        #endregion

        #region [Method] OnDisconnected
        protected override void OnDisconnected()
        {
            ConsoleSystem.Log("Disconnected");
            _needReconnect = true;
        }
        #endregion
        
        #region [Method] OnSocketError
        protected override void OnSocketError(SocketError socketError)
        {
            ConsoleSystem.LogError($"[Client] SocketError: {socketError}");
        }
        #endregion

        #region [Method] WorkingLoop
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
                    if (IsConnected == false)
                    {
                        MainForm.UpdateStatus("Failed connect to server");
                        break;
                    }
                }
            }
        }
        #endregion
    }
}