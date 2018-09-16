using System;
using System.Collections.Generic;
using Ether.Network.Common;
using Ether.Network.Packets;
using OxidePack.Data;
using OxidePack.Server.App.Data;
using SapphireEngine;

namespace OxidePack.Server.App
{
    public class User : NetUser
    {
        static HashSet<string> ActiveUsers = new HashSet<string>();
        
        public bool     IsAuthed   = false;
        public UserData Data     = null;
        
        public override void HandleMessage(INetPacketStream stream)
        {
            var packet = (NetPacket) stream;
            var msgType = packet.ReadPacketID();
            switch (msgType)
            {
                case PacketType.GiveUserInformation:
                    var uInfo = UserInformation.Deserialize(packet);
                    OnGiveUserInformation(uInfo);
                    break;
                default:
                    ConsoleSystem.LogError($"{this} sent invalid message");
                    break;
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
                SendGiveUserInformationResult("Session with the same key already exists!");
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