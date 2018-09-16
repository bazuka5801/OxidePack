using Ether.Network.Packets;

namespace OxidePack
{
    public static class INetPacketStreamEx
    {
        public static PacketType ReadPacketID(this INetPacketStream stream)
        {
            return (PacketType)stream.Read<byte>();
        }
    }
}