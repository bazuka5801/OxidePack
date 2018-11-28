using System;
using System.Collections.Generic;
using Ether.Network.Common;
using Ether.Network.Packets;
using OxidePack.CoreLib;
using OxidePack.Data;
using OxidePack.Server.App.Data;
using SapphireEngine;
using SilentOrbit.ProtocolBuffers;

namespace OxidePack.Server.App
{
    public class BaseUser : NetUser
    {
        static HashSet<string> ActiveUsers = new HashSet<string>();
        
        public bool     IsAuthed;
        public UserData Data;
        
        public override void HandleMessage(INetPacketStream stream)
        {
            try
            {
                var packet = (NetPacket) stream;
                var msgType = packet.ReadPacketID();
                switch (msgType)
                {
                    case PacketType.GiveUserInformation:
                        var uInfo = UserInformation.Deserialize(packet);
                        OnGiveUserInformation(uInfo);
                        break;
                    case PacketType.RPCMessage:
                        var rpcmessagetype = (RPCMessageType)stream.Read<UInt32>();
                        HandleRPCMessage(rpcmessagetype, packet);
                        break;
                    default:
                        ConsoleSystem.LogError($"{this} sent invalid message");
                        break;
                }
            }
            catch (Exception e)
            {
                ConsoleSystem.LogError($"[HandleMessage] => {e.Message}\n{e.StackTrace}");
//                Server.DisconnectClient();
                throw;
            }
        }

        public virtual void HandleRPCMessage(RPCMessageType type, NetPacket stream)
        {
        }

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

        #region [Mehtod] RequestUserInformation
        public void RequestUserInformation()
        {
            if (this.IsConnected() == false)
            {
                ConsoleSystem.ShowCallerInLog = true;
                ConsoleSystem.LogError("User isn't connected");
                ConsoleSystem.ShowCallerInLog = false;
                return;
            }

            using (NetPacket packet = new NetPacket())
            {
                packet.WritePackketID(PacketType.RequestUserInformation);
                Send(packet);
            }
        }
        #endregion

        #region [Method] OnGiveUserInformation
        public void OnGiveUserInformation(UserInformation uInfo)
        {
            if (string.IsNullOrEmpty(uInfo.key))
            {
                SendGiveUserInformationResult("Invalid key");
                return;
            }
            if (string.IsNullOrEmpty(uInfo.username))
            {
                SendGiveUserInformationResult("Invalid username");
                return;
            }

            if (ActiveUsers.Contains(uInfo.key))
            {
                SendGiveUserInformationResult("Double connection!");
                return;
            }
            
            Data = UserDB.Get(uInfo.key, uInfo.username);
            IsAuthed = true;
            ActiveUsers.Add(uInfo.key);
            SendGiveUserInformationResult("");
        }
        #endregion

        #region [Method] SendAuthError
        public void SendGiveUserInformationResult(string text)
        {
            if (this.IsConnected() == false)
            {
                ConsoleSystem.ShowCallerInLog = true;
                ConsoleSystem.LogError("User isn't connected");
                ConsoleSystem.ShowCallerInLog = false;
                return;
            }
            
            using (NetPacket packet = new NetPacket())
            {
                packet.WritePackketID(PacketType.GiveUserInformationResult);
                packet.Write(text);
                Send(packet);
            }
        }
        #endregion
        
        #region [Method] OnConnected
        public void OnConnected()
        {
            RequestUserInformation();
        }
        #endregion
        
        #region [Method] OnDisconnected
        public void OnDisconnected()
        {
            if (IsAuthed)
            {
                ActiveUsers.Remove(Data.key);
            }
        }
        #endregion
        
        public string GetConsoleStatus()
        {
            if (IsAuthed == false)
                return $"[{this.GetIP()}] User isn't authenticated";
            return $"[{this.GetIP()}] <{Data.username}>";
        }

        public override string ToString()
        {
            if (IsAuthed == false)
                return $"[{this.GetIP()}]";
            return $"[{this.GetIP()}] [{Data.username}]";
        }
    }
}