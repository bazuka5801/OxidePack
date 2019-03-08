using System;

namespace OxidePack.CoreLib
{
    public partial class Plugin : IDisposable
    {
        public PluginOptions Options { get; private set; }
        public string PluginName { get; private set; }
        
        public Plugin(string pluginname)
            : this(pluginname, new PluginOptions())
        {
        }

        public Plugin(string pluginname, PluginOptions options)
        {
            this.PluginName = pluginname;
            this.Options = options;
        }

        public void Dispose()
        {
            Workspace?.Dispose();
        }
    }
}