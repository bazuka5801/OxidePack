using System;

namespace OxidePack.CoreLib
{
    public partial class Plugin : IDisposable
    {
        public Plugin(string pluginname)
            : this(pluginname, new PluginOptions())
        {
        }

        public Plugin(string pluginname, PluginOptions options)
        {
            PluginName = pluginname;
            Options = options;
        }

        public PluginOptions Options { get; }
        public string PluginName { get; }

        public void Dispose()
        {
            Workspace?.Dispose();
        }
    }
}