using System;
using System.Threading;
using System.Threading.Tasks;
using SapphireEngine;
using Timer = SapphireEngine.Functions.Timer;

namespace OxidePack.Server.App
{
    public class AppCore : SapphireType
    {
        public override void OnAwake()
        {
            this.AddType<ConfigManager>();
            this.Initialize();
        }

        public void Initialize()
        {
            Timer.SetInterval(UpdateTitle, 1f);
            RunServer();
        }

        private void RunServer()
        {
            Thread serverThread = new Thread(ServerWorker) { Name = "Server", IsBackground = true };
            serverThread.Start();
        }

        private void ServerWorker(object o)
        {
            using (var server = new BaseServer())
            {
                server.Start();
            }
        }

        public virtual void UpdateTitle()
        {
            int online = 10;
            Console.Title = Config.Title.Replace("{online}", online.ToString());
        }
    }
}