using System;
using System.Linq;
using System.Threading;
using Ether.Network.Packets;
using OxidePack.CoreLib;
using OxidePack.Data;

namespace OxidePack.Server.App
{
    public class OPUser : BaseUser
    {
        public Plugin ActivePlugin;
        
        
        public override void HandleRPCMessage(RPCMessageType type, NetPacket stream)
        {
            switch (type)
            {
                case RPCMessageType.StatusRequest:
                    break;
                case RPCMessageType.BuildRequest:
                    BuildRequest bRequest = BuildRequest.Deserialize(stream);
                    if (ActivePlugin == null || ActivePlugin.PluginName != bRequest.buildOptions.name)
                    {
                        var pOptions = new PluginOptions() { Debug = true, Encrypt = false, ReferencesPath = ".references" };
                        ActivePlugin = new Plugin(bRequest.buildOptions.name, pOptions);
                    }

                    ThreadPool.QueueUserWorkItem((s) =>
                    {
                        string buildResult = ActivePlugin.Build(bRequest);
                        string encryptResult = null;
                        if (bRequest.encryptOptions.enabled)
                        {
                            encryptResult = ActivePlugin.Encrypt(buildResult);
                        }

                        BuildResponse bResponse = new BuildResponse()
                        {
                            pluginname = bRequest.buildOptions.name,
                            content = buildResult,
                            encrypted = encryptResult
                        };
                    
                        SendRPC(RPCMessageType.BuildResponse, bResponse);
                    });
                    break;
                case RPCMessageType.EncryptionRequest:
                    break;
                case RPCMessageType.GeneratedFileRequest:
                    OnRPC_GeneratedFileRequest(GeneratedFileRequest.Deserialize(stream));
                    break;
                case RPCMessageType.ModuleListRequest:
                    SendModuleList();
                    break;
            }
        }

        public void OnRPC_GeneratedFileRequest(GeneratedFileRequest request)
        {
            if (ActivePlugin == null || ActivePlugin.PluginName != request.pluginname)
            {
                var pOptions = new PluginOptions() { Debug = true, Encrypt = false, ReferencesPath = ".references" };
                ActivePlugin = new Plugin(request.pluginname, pOptions);
            }
            var generatedFile = ActivePlugin.GetGeneratedFile(request.modules, request.@namespace);
            var response = new GeneratedFileResponse()
            {
                pluginname = request.pluginname,
                content = generatedFile
            };
            
            SendRPC(RPCMessageType.GeneratedFileResponse, response);
        }

        public void SendModuleList()
        {
            var moduleList = ModuleMgr.GetModuleList().Select(module => new ModuleInfo()
            {
                name = module.Manifest.Name,
                version = module.Manifest.Version,
                description = module.Manifest.Description
            }).ToList();
            
            var moduleListResponse = new ModuleListResponse()
            {
                modules = moduleList
            };
            
            SendRPC(RPCMessageType.ModuleListResponse, moduleListResponse);
        }
    }
}