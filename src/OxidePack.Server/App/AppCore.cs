using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OxidePack.CoreLib;
using OxidePack.Server.App.Data;
using SapphireEngine;
using Timer = SapphireEngine.Functions.Timer;

namespace OxidePack.Server.App
{
    public class AppCore : SapphireType
    {
        private OPServer Server;
        public override void OnAwake()
        {
            var config = this.AddType<ConfigManager>();
            config.SetConfigType(typeof(Config));
            config.RunWatcher();
            
            this.Initialize();
            ConsoleSystem.OnConsoleInput += OnConsoleCommand;
            ModuleMgr.Init();
        }

        public override void OnDestroy()
        {
            this.Shutdown();
        }
        
        public void Initialize()
        {
            UserDB.Load();
            Timer.SetInterval(UpdateTitle, 1f);
            RunServer();
        }

        public void Shutdown()
        {
            UserDB.Save();
            Server.Dispose();
        }

        private void OnConsoleCommand(string line)
        {
            if (line == ":q")
            {
                ConsoleSystem.Log("Quiting...");
                Framework.Quit();
            }

            if (line.StartsWith("adduser"))
            {
                var data = line.Split(' ');
                string key = data[1];
                string username = data[2];
                UserDB.AddUser(key, username);
            }

            if (line.StartsWith("userlist"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                foreach (var uData in UserDB.All)
                {
                    sb.AppendLine($"[{uData.index}] {uData.username}");
                }
                ConsoleSystem.Log(sb.ToString());
                sb.Clear();
            }
        }
        
        private void RunServer()
        {
            Thread serverThread = new Thread(ServerWorker) { Name = "Server", IsBackground = true };
            serverThread.Start();
        }

        private void ServerWorker(object o)
        {
            using (Server = new OPServer())
            {
                Server.Start();
            }
        }

        public virtual void UpdateTitle()
        {
            // TODO: Set real counter
            int online = 10;
            Console.Title = $"[{Protocol.Version}] " + Config.Title.Replace("{online}", online.ToString());
        }
    }
}