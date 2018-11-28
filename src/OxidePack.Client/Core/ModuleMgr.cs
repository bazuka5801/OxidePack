using System.Collections.Generic;
using Ether.Network.Packets;
using OxidePack.Data;

namespace OxidePack.Client
{
    public static class ModuleMgr
    {
        public static List<ModuleInfo> Modules = new List<ModuleInfo>();

        public delegate void OnModulesUpdateDelegate();

        public static event OnModulesUpdateDelegate OnModulesUpdateEvent;

        public static void OnModulesUpdate(NetPacket stream)
        {
            // Not dispose, because using data in code
            ModuleListResponse mlResponse = ModuleListResponse.Deserialize(stream);
            Modules = mlResponse.modules;
            
            OnModulesUpdateEvent?.Invoke();
        }
        
        /// <summary>
        /// Request information about modules
        /// </summary>
        public static void Refresh()
        {
            Net.cl.SendRPC(RPCMessageType.ModuleListRequest);
        }
    }
}