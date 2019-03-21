using System;
using System.Linq;
using System.Text;
using System.Threading;
using OxidePack.CoreLib;
using OxidePack.Server.App.Data;
using SapphireEngine;
using Timer = SapphireEngine.Functions.Timer;

namespace OxidePack.Server.App
{
    public class AppCore : SapphireType
    {
        private OpServer _server;
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
            UserDb.Load();
            Timer.SetInterval(UpdateTitle, 1f);
            RunServer();
        }

        public void Shutdown()
        {
            UserDb.Save();
            _server.Dispose();
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
                UserDb.AddUser(key, username);
            }

            if (line.StartsWith("userlist"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                foreach (var uData in UserDb.All)
                {
                    sb.AppendLine($"[{uData.index}] {uData.username} perms: {string.Join(",", uData.permissions.Select(p=>$"[{p.name}: {p.expired}]"))}");
                }
                ConsoleSystem.Log(sb.ToString());
                sb.Clear();
            }

            if (line.StartsWith("userstats"))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                foreach (var uData in UserDb.All.OrderByDescending(p=>p.millisecondsused))
                {
                    foreach (var perm in uData.permissions)
                    {
                        uData.HasPermission(perm.name);
                    }

                    sb.AppendLine($"[{uData.index}] {uData.username} usedseconds: {TimeSpan.FromMilliseconds(uData.millisecondsused).TotalSeconds.ToString("F3")}");
                }
                ConsoleSystem.Log(sb.ToString());
                sb.Clear();
            }

            if (line.StartsWith("addperm"))
            {
                var data = line.Split(' ');
                string username = data[1];
                string perm = data[2];
                ulong seconds = ulong.Parse(data[3]);
                var user = UserDb.All.FirstOrDefault(p => p.username == username);
                if (user == null)
                {
                    ConsoleSystem.LogError("User not found!");
                    return;
                }

                user.AddPermission(perm, (int)seconds);
                ConsoleSystem.LogError($"User '{username}' granted '{perm}' permission!");
            }
        }

        private void RunServer()
        {
            Thread serverThread = new Thread(ServerWorker) { Name = "Server", IsBackground = true };
            serverThread.Start();
        }

        private void ServerWorker(object o)
        {
            using (_server = new OpServer())
            {
                _server.Start();
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