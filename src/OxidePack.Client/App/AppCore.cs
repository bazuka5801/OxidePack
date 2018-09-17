using System.Threading;
using System.Windows.Forms;
using OxidePack.Client.Forms;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class AppCore : SapphireType
    {
        public static MainForm AuthForm;
        
        public override void OnAwake()
        {
            RunUI();
            this.AddType<ConfigManager>();
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
            using (var client = new Client(Config.Host, Config.Port, Config.BufferSize))
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