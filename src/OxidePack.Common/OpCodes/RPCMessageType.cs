namespace OxidePack
{
    public enum RPCMessageType : uint
    {
        // Status
        StatusRequest = 4,
        StatusResponse = 5,
        
        // Build
        BuildRequest = 6,
        BuildResponse = 7,
        
        // Encryption
        EncryptionRequest = 8,
        EncryptionResponse = 9,
        
        // GeneratedFile
        GeneratedFileRequest = 10,
        GeneratedFileResponse = 11,
        
        // Modules
        ModuleListRequest = 12,
        ModuleListResponse = 13,
    }
}