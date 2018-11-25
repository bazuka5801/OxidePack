using System.Collections.Generic;
using Ether.Network.Packets;
using OxidePack.Data;

namespace OxidePack.Client
{
    public static class ModuleMgr
    {
        public static List<ModuleInfo> Modules = new List<ModuleInfo>();

        public static void OnModulesUpdate(NetPacket stream)
        {
            // Not dispose, because using data in code
            ModuleListResponse mlResponse = ModuleListResponse.Deserialize(stream);
            Modules = mlResponse.modules;
        }
    }
}