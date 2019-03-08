namespace OxidePack.Server.App
{
    public class Config : BaseConfig
    {
        public const  int    Timeout = 5;
        public static string Host = "127.0.0.1";
        public static int    Port = 10000;

        public static string Title = "Oxide Pack [online: {online}]";
    }
}