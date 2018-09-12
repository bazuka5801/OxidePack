using System;
using Ether.Network.Server;
using SapphireEngine;

namespace OxidePack.Server.App
{
    public class BaseServer : NetServer<User>
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

        protected override void OnClientConnected(User connection)
        {
            ConsoleSystem.Log($"[Server] New client from {connection.Socket.GetIP()} - {connection.Id}");
        }

        protected override void OnClientDisconnected(User connection)
        {
        }

        protected override void OnError(Exception e)
        {
            ConsoleSystem.LogError($"[Server] [Exception] {e.Message}\n{e.StackTrace}");
        }
    }
}