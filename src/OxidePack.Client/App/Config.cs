using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OxidePack.Client.App
{
    [Config]
    public static class Config
    {
        public const string Host       = "127.0.0.1";
        public const int    Port       = 10000;
        public const int    BufferSize = 512;


        public static string SolutionFile = "...";
        
        [ConfigLoadedEvent]
        private static Action OnConfigLoadedAction;

        #region [CompilerGenerated]
        public static event Action OnConfigLoaded
        {
            add
            {
                Action action = Config.OnConfigLoadedAction;
                Action action2;
                do
                {
                    action2 = action;
                    Action value2 = (Action)Delegate.Combine(action2, value);
                    action = Interlocked.CompareExchange(ref OnConfigLoadedAction, value2, action2);
                }
                while (action != action2);
            }
            remove
            {
                Action action = Config.OnConfigLoadedAction;
                Action action2;
                do
                {
                    action2 = action;
                    Action value2 = (Action)Delegate.Remove(action2, value);
                    action = Interlocked.CompareExchange(ref OnConfigLoadedAction, value2, action2);
                }
                while (action != action2);
            }
        }
        #endregion
    }
}