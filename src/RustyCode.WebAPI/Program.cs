using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RustyCode.WebAPI.System.Network;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace RustyCode.WebAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
#if DEBUG
            var url = $"http://localhost:9696";
            #else
            var url = $"http://{IPUtils.GetExternalIPAddress()}:9696";
#endif
            using (var server = new WebServer(url, RoutingStrategy.Regex))
            {
                // First, we will configure our web server by adding Modules.
                // Please note that order DOES matter.
                // ================================================================================================
                // If we want to enable sessions, we simply register the LocalSessionModule
                // Beware that this is an in-memory session storage mechanism so, avoid storing very large objects.
                // You can use the server.GetSession() method to get the SessionInfo object and manupulate it.
                // You could potentially implement a distributed session module using something like Redis
                server.WithLocalSession();
                
                server.RegisterModule(new WebApiModule());
                server.Module<WebApiModule>().RegisterController<Controllers.PluginController>();

                // Once we've registered our modules and configured them, we call the RunAsync() method.
                server.RunAsync();

                // Fire up the browser to show the content if we are debugging!
#if DEBUG
//                var browser = new Process()
//                {
//                    StartInfo = new ProcessStartInfo(url) { UseShellExecute = true }
//                };
//                browser.Start();
#endif
                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                Console.ReadKey(true);
            }
        }
    }
}