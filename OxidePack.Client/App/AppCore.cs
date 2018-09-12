using System.Threading;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class AppCore : SapphireType
    {
        public override void OnAwake()
        {
            Initialize();
        }

        private void Initialize()
        {
            RunClient();
        }

        private void RunClient()
        {
            var clientThread = new Thread(ClientWorker) { Name = "Client", IsBackground = true };
            clientThread.Start();
        }

        private void ClientWorker(object o)
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
    }
}