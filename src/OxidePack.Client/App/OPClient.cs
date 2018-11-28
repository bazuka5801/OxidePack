using System;
using System.IO;
using Ether.Network.Packets;
using OxidePack.Data;
using SapphireEngine;

namespace OxidePack.Client.App
{
    public class OPClient : Client
    {
        public OPClient(string host, int port, int bufferSize) : base(host, port, bufferSize)
        {
            Net.cl = this;
        }

        public override void HandleRPCMessage(RPCMessageType type, NetPacket stream)
        {
            switch (type)
            {
                case RPCMessageType.StatusResponse:
                    break;
                case RPCMessageType.BuildResponse:
                    GeneratedFileResponse gFile = GeneratedFileResponse.Deserialize(stream);
                    File.WriteAllText($"{gFile.pluginname}.compiled.cs", gFile.content);
                    break;
                case RPCMessageType.EncryptionResponse:   
                    break;
                case RPCMessageType.GeneratedFileResponse:
                    OnRPC_GeneratedFileResponse(GeneratedFileResponse.Deserialize(stream));
                    break;
                case RPCMessageType.ModuleListResponse:
                    ModuleMgr.OnModulesUpdate(stream);
                    break;
            }
        }

        private void OnRPC_GeneratedFileResponse(GeneratedFileResponse response)
        {
            var plugin = OPClientCore.PluginsProject.GetPlugin(response.pluginname);
            if (plugin == null)
            {
                var enabled = ConsoleSystem.ShowCallerInLog;
                ConsoleSystem.ShowCallerInLog = true;
                ConsoleSystem.LogError($"plugin '{response.pluginname}' is NULL");
                ConsoleSystem.ShowCallerInLog = enabled;
                return;
            }
            
            plugin.OnGeneratedFileResponse(response.content);
        }
    }
}