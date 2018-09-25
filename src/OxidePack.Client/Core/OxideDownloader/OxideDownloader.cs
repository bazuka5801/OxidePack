using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using OxidePack.Client.App;

namespace OxidePack.Client.Core.OxideDownloader
{
    public static class OxideDownloader
    {
        static WebClient wClient = new WebClient();
        
        public static void Download(string directory)
        {
            string zipPath = Path.Combine(directory, "Oxide.Rust.zip");
            
            // Download Oxide build
            wClient.DownloadFile(Config.OxideURL, zipPath);
            
            // Extract all dll files to directory
            using (var stream = File.OpenRead(zipPath))
            {
                using (var zArchive = new ZipArchive(stream))
                {
                    foreach (var file in zArchive.Entries.Where(p=>Regex.IsMatch(p.FullName.Replace('/', '\\'), Properties.Resources.RustReferencesRegex)))
                    {
                        file.ExtractToFile(Path.Combine(directory, file.Name), true);
                    }
                }
            }
        }
    }
}