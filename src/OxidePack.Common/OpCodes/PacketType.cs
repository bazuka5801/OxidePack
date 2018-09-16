namespace OxidePack
{
    public enum PacketType : byte
    {
        RequestUserInformation = 1,
        GiveUserInformation = 2,
        GiveUserInformationResult = 3,
        RequestStatus = 4,
        GiveStatus = 5,
        RequestCompilation = 6,
        GiveCompilationResult = 7,
        RequestEncryption = 8,
        GiveEncryptionResult = 9,
    }
}