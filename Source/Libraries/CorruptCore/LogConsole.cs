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
            var handle = NativeWin32APIs.GetConsoleWindow();
            NativeWin32APIs.ShowWindow(handle, NativeWin32APIs.SW_SHOW);
            ConsoleVisible = true;
        }

        public static void HideConsole()
        {
            var handle = NativeWin32APIs.GetConsoleWindow();
            NativeWin32APIs.ShowWindow(handle, NativeWin32APIs.SW_HIDE);
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
