using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Octokit;
using OxidePack.Client.App;
using SapphireEngine;

namespace OxidePack.Client.Core.OxideDownloader
{
    public static class OxideDownloader
    {
        static WebClient wClient = new WebClient();
        static GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("OxidePack"));
        
        /// <summary>
        /// Getting version from github repository
        /// </summary>
        /// <returns>Oxide Version</returns>
        public static string GetVersion()
        {
            return ghClient.Repository.Release.GetLatest("theumod", "uMod.Rust").Result.TagName;
        }
        
        /// <summary>
        /// Download Available oxide DLLs
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>Oxide Version</returns>
        public static async Task<string> Download(string directory, 
            Action<string,int> progress,
            Action completed)
        {
            string zipPath = Path.Combine(directory, "Oxide.Rust.zip");
            progress?.Invoke($"Getting oxide version...", 5);
            string version = GetVersion();

            string lastProgressLine = "";
            
            void WClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                var downloadingSize = FileSystemUtils.AdjustFileSize(e.BytesReceived);
                var totalSize = FileSystemUtils.AdjustFileSize(e.TotalBytesToReceive);
                
                string progressLine =
                    $"Downloading Oxide... (" +
                    $"{downloadingSize.sizeResult:f2}{downloadingSize.sizeTitle}" +
                    $"/" +
                    $"{totalSize.sizeResult:f2}{totalSize.sizeTitle}" +
                    $")";
                
                if (lastProgressLine == progressLine)
                {
                    return;
                }
                
                lastProgressLine = progressLine;
                
                progress?.Invoke(progressLine, e.ProgressPercentage);
            }
            
            void WClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                progress?.Invoke($"Extracting DLLs...", 100);
            }
            
            // Download Oxide build
            wClient.DownloadProgressChanged += WClientOnDownloadProgressChanged;
            wClient.DownloadFileCompleted += WClientOnDownloadFileCompleted;
            await wClient.DownloadFileTaskAsync(Config.OxideURL.Replace("{tag}", version), zipPath);
            wClient.DownloadProgressChanged -= WClientOnDownloadProgressChanged;
            wClient.DownloadFileCompleted -= WClientOnDownloadFileCompleted;
            
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
            File.Delete(zipPath);
            completed.Invoke();
            return version;
        }
    }
}