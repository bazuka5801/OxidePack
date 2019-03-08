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
                    using (BuildResponse bResponse = BuildResponse.Deserialize(stream))
                    {
                        OnRPC_BuildResponse(bResponse);
                    }
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

        private void OnRPC_BuildResponse(BuildResponse bResponse)
        {
            var plugin = OPClientCore.PluginsProject.GetPlugin(bResponse.pluginname);
            if (plugin == null)
            {
                PluginNotExistError(bResponse.pluginname);
                return;
            }

            plugin.OnBuildResponse(bResponse);
        }

        private void OnRPC_GeneratedFileResponse(GeneratedFileResponse response)
        {
            var plugin = OPClientCore.PluginsProject.GetPlugin(response.pluginname);
            if (plugin == null)
            {
                PluginNotExistError(response.pluginname);
                return;
            }
            
            plugin.OnGeneratedFileResponse(response.content);
        }

        private void PluginNotExistError(string pluginname)
        {
            var enabled = ConsoleSystem.ShowCallerInLog;
            ConsoleSystem.ShowCallerInLog = true;
            ConsoleSystem.LogError($"plugin '{pluginname}' is NULL");
            ConsoleSystem.ShowCallerInLog = enabled;
        }
    }
}