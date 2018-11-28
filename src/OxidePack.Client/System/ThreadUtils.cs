using System;

namespace OxidePack.Client
{
    public class ThreadUtils
    {
        public static void RunInUI(Action action)
        {
            if (MainForm.Instance.InvokeRequired)
            {
                MainForm.Instance.Invoke(action);
                return;
            }

            action();
        }
    }
}