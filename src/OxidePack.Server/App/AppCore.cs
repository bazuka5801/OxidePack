using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            this.AddType<ConfigManager>().RunWatcher();
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
            Console.Title = Config.Title.Replace("{online}", online.ToString());
        }
    }
}