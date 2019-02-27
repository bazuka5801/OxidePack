using System.Net;
using System.Threading;
using System.Windows.Forms;
using OxidePack.Client;
using OxidePack.Client.Core.OxideDownloader;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class AppCore : SapphireType
    {
        public static MainForm AuthForm;
        
        public override void OnAwake()
        {
            OxideDownloader.FixWeb();
            ConsoleSystem.IsOutputToFile = false;
            this.AddType<ConfigManager>().SetConfigType(typeof(Config));
            OPClientCore.Init();
            RunUI();
        }

        public static void ConnectToServer()
        {
            RunClient();
        }

        #region [Methods] Client
        private static void RunClient()
        {
            var clientThread = new Thread(ClientWorker) { Name = "Client", IsBackground = true };
            clientThread.Start();
        }

        private static void ClientWorker(object o)
        {
            WebClient webClient = new WebClient();
            var connectionString = webClient.DownloadString("https://pastebin.com/raw/kRxQCGjv");
            var connectionData = connectionString.Split(':');
            var ip = connectionData[0];
            var port = int.Parse(connectionData[1]);
            
            using (var client = new OPClient(ip, port, Config.BufferSize))
            {
                client.WorkingLoop();
            }
        }
        #endregion
        
        #region [Methods] UI
        private static void RunUI()
        {
            Thread uiThread = new Thread(UIWorker) { Name = "UI", IsBackground = true };
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }

        private static void UIWorker(object o)
        {
            Application.EnableVisualStyles();
            Application.Run(AuthForm = new MainForm());
            Framework.RunToMainThread(z => Framework.Quit(), null);
        }
        #endregion
    }
}