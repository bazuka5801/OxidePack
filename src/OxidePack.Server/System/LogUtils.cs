using System;
using SapphireEngine;

namespace OxidePack.Server
{
    public static class LogUtils
    {
        public static void PutsException(Exception exception, string modulename)
        {
            ConsoleSystem.LogError($"[Exception] [{modulename}] {exception.Message}\n{exception.StackTrace}");
            if (exception.InnerException != null)
                ConsoleSystem.LogError($"[Exception] [{modulename}] [Inner] {exception.InnerException.Message}\n{exception.InnerException.StackTrace}");
        }
    }
}