using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;

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
            BuildingInit();
        }

        public void Dispose()
        {
            _workspace?.Dispose();
        }
    }
}