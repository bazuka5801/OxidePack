namespace OxidePack
{
    public enum PacketType : byte
    {
        RequestUserInformation = 1,
        GiveUserInformation = 2,
        GiveUserInformationResult = 3,
        RPCMessage = 4,
        ConnectionAlive = 5
    }
}