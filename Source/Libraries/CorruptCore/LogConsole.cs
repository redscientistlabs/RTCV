namespace RTCV.CorruptCore
{
    //Lifted from Bizhawk https://github.com/TASVideos/BizHawk
    #pragma warning disable 162
    public static class LogConsole
    {
        public static bool ConsoleVisible
        {
            get;
            private set;
        }

        public static void ShowConsole()
        {
            var handle = Win32.GetConsoleWindow();
            Win32.ShowWindow(handle, Win32.SW_SHOW);
            ConsoleVisible = true;
        }

        public static void HideConsole()
        {
            var handle = Win32.GetConsoleWindow();
            Win32.ShowWindow(handle, Win32.SW_HIDE);
            ConsoleVisible = false;
        }

        public static void ToggleConsole()
        {
            if (ConsoleVisible)
            {
                HideConsole();
            }
            else
            {
                ShowConsole();
            }
        }
    }
}
