using System;
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
                    var pOptions = new PluginOptions() { Debug = true, Encrypt = false, ReferencesPath = ".references" };
                    ActivePlugin = new Plugin(bRequest.options.name, pOptions);
                    
                    GeneratedFileResponse gFile = new GeneratedFileResponse()
                    {
                        pluginname = bRequest.options.name,
                        content = ActivePlugin.Build(bRequest)
                    };
                    
                    SendRPC(RPCMessageType.BuildResponse, gFile);
                    break;
                case RPCMessageType.EncryptionRequest:
                    break;
                case RPCMessageType.GeneratedFileRequest:
                    OnRPC_GeneratedFileRequest(GeneratedFileRequest.Deserialize(stream));
                    break;
            }
        }

        public void OnRPC_GeneratedFileRequest(GeneratedFileRequest request)
        {
            var generatedFile = ActivePlugin.GetGeneratedFile(request.modules);
            var response = new GeneratedFileResponse()
            {
                pluginname = request.pluginname,
                content = generatedFile
            };
            
            SendRPC(RPCMessageType.GeneratedFileResponse, response);
        }
    }
}