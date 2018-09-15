using System;
using Ether.Network.Common;
using Ether.Network.Packets;

namespace OxidePack.Server.App
{
    public class User : NetUser
    {
        
        public override void HandleMessage(INetPacketStream packet)
        {
            Console.WriteLine(packet.Read<string>());
        }
    }
}