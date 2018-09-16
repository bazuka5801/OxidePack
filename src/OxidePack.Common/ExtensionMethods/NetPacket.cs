using Ether.Network.Packets;

namespace OxidePack
{
    public static class NetPacketEx
    {
        public static void WritePackketID(this NetPacket nPacket, PacketType pType)
        {
            nPacket.WriteByte((byte)pType);
        }
    }
}