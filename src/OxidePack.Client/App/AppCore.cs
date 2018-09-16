using System.Threading;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class AppCore : SapphireType
    {
        public static void Initialize()
        {
            RunFramework();
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
                client.Connect();

                while (true)
                {
                    if (client.IsConnected == false)
                    {
                        client.Connect();
                    }
                    Thread.Sleep(1000);
                }
            }
        }
        #endregion
        
        #region [Methods] Framework
        private static void RunFramework()
        {
            Thread frameworkThread = new Thread(FrameworkWorker) { Name = "Framework", IsBackground = true };
            frameworkThread.Start();
        }

        private static void FrameworkWorker(object o)
        {
            Framework.Initialization<AppCore>(true);
        }
        #endregion
    }
}