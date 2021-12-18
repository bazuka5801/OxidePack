using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RustyCode.WebAPI.System.Network
{
    public class IPUtils
    {
        public static string GetExternalIPAddress()
        {
            string result = string.Empty;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers["User-Agent"] =
                    "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                    "(compatible; MSIE 6.0; Windows NT 5.1; " +
                    ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                    try
                    {
                        byte[] arr = client.DownloadData("http://checkip.amazonaws.com/");

                        string response = Encoding.UTF8.GetString(arr);

                        result = response.Trim();
                    }
                    catch (WebException)
                    {                       
                    }
                }
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    result = new WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    result = new WebClient().DownloadString("https://api.ipify.org").Replace("\n", "");
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    result = new WebClient().DownloadString("https://icanhazip.com").Replace("\n", "");
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    result = new WebClient().DownloadString("https://wtfismyip.com/text").Replace("\n", "");
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    result = new WebClient().DownloadString("http://bot.whatismyipaddress.com/").Replace("\n", "");
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                try
                {
                    string url = "http://checkip.dyndns.org";
                    WebRequest req = WebRequest.Create(url);
                    WebResponse resp = req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string response = sr.ReadToEnd().Trim();
                    string[] a = response.Split(':');
                    string a2 = a[1].Substring(1);
                    string[] a3 = a2.Split('<');
                    result = a3[0];
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
    }
}