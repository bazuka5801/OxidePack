using System.IO;
using RustyCode.Core.Data;

namespace RustyCode.WebAPI.Repository
{
    public static class PluginRepository
    {
        public static (bool success, string message) Upload(PluginMeta pluginMeta, byte[] content)
        {
            var pluginPath = GetPluginPath(pluginMeta);
            var pluginDirectory = GetPluginDirectory(pluginMeta);
            
            if (File.Exists(pluginPath))
            {
                return (false, $"Plugin [{pluginMeta.Name}/{pluginMeta.Author}/{pluginMeta.Version}] already exist!");
            }

            if (Directory.Exists(pluginDirectory) == false)
                Directory.CreateDirectory(pluginDirectory);

            File.WriteAllBytes(pluginPath, content);
            return (true, "Success!");
        }

        public static (bool success, string message, byte[] content) Get(PluginMeta pluginMeta)
        {
            var pluginPath = GetPluginPath(pluginMeta);
            if (File.Exists(pluginPath) == false)
            {
                return (false, $"Plugin [{pluginMeta.Name}/{pluginMeta.Author}/{pluginMeta.Version}] doesn't exist!", null);
            }

            return (true, "Success!", File.ReadAllBytes(pluginPath));
        }

        private static string GetPluginDirectory(PluginMeta pluginMeta) =>
            $"uploads/{pluginMeta.Author}/{pluginMeta.Name}/{pluginMeta.Version}";

        private static string GetPluginPath(PluginMeta pluginMeta) =>
            $"{GetPluginDirectory(pluginMeta)}/{pluginMeta.Name}.cs";
    }
}