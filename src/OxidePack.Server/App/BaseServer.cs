using System;
using Ether.Network.Common;
using Ether.Network.Server;
using SapphireEngine;

namespace OxidePack.Server.App
{
    public class BaseServer<T> : NetServer<T>
        where T : BaseUser, new() 
    {
        public BaseServer()
        {
            this.Configuration.Host = Config.Host;
            this.Configuration.Port = Config.Port;
            this.Configuration.Backlog = 128;
            this.Configuration.BufferSize = 8;
            this.Configuration.Blocking = true;
            this.Configuration.MaximumNumberOfConnections = 100;
        }
        
        protected override void Initialize()
        {
            ConsoleSystem.Log("Server is Ready");
        }

        protected override void OnClientConnected(T baseUser)
        {
            baseUser.OnConnected();
        }

        protected override void OnClientDisconnected(T baseUser)
        {
            baseUser.OnDisconnected();
        }

        protected override void OnError(Exception e)
        {
            ConsoleSystem.LogError($"[Server] [Exception] {e.Message}\n{e.StackTrace}");
        }
    }
}