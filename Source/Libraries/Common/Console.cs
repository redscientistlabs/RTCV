namespace RTCV.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ConsoleHelper
    {
        public static ConsoleCopy con;

        public static void CreateConsole(string path = null)
        {
            if (!Debugger.IsAttached) //Don't override debugger's console
            {
                ReleaseConsole();
                AllocConsole();
            }

            if (!string.IsNullOrEmpty(path))
            {
                con = new ConsoleCopy(path);
            }

            //Disable the X button on the console window
            EnableMenuItem(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_DISABLED);
        }

        private static bool ConsoleVisible = true;

        public static void ShowConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
            ConsoleVisible = true;
        }

        public static void HideConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
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

        public static void ReleaseConsole()
        {
            var handle = GetConsoleWindow();
            CloseHandle(handle);
        }
        // P/Invoke required:
        internal const int SW_HIDE = 0;
        internal const int SW_SHOW = 5;

        internal const int SC_CLOSE = 0xF060;           //close button's code in Windows API
        internal const int MF_ENABLED = 0x00000000;     //enabled button status
        internal const int MF_GRAYED = 0x1;             //disabled button status (enabled = false)
        internal const int MF_DISABLED = 0x00000002;    //disabled button status

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(uint nStdHandle, IntPtr handle);

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr HWNDValue, bool isRevert);

        [DllImport("user32.dll")]
        public static extern int EnableMenuItem(IntPtr tMenu, int targetItem, int targetStatus);
    }

    public class ConsoleCopy : IDisposable
    {
        private FileStream fileStream;
        public StreamWriter FileWriter;
        private TextWriter doubleWriter;
        private TextWriter oldOut;

        private class DoubleWriter : TextWriter
        {
            private TextWriter one;
            private TextWriter two;

            public DoubleWriter(TextWriter one, TextWriter two)
            {
                this.one = one;
                this.two = two;
            }

            public override Encoding Encoding => one.Encoding;

            public override void Flush()
            {
                one.Flush();
                two.Flush();
            }

            public override void Write(char value)
            {
                one.Write(value);
                two.Write(value);
            }
        }

        public ConsoleCopy(string path)
        {
            oldOut = Console.Out;

            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Create(path).Close();
                fileStream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read);
                FileWriter = new StreamWriter(fileStream)
                {
                    AutoFlush = true
                };

                doubleWriter = new DoubleWriter(FileWriter, oldOut);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(doubleWriter);
            Console.SetError(doubleWriter);
        }

        public void Dispose()
        {
            Console.SetOut(oldOut);
            if (FileWriter != null)
            {
                FileWriter.Flush();
                FileWriter.Close();
                FileWriter = null;
            }
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
            if (doubleWriter != null)
            {
                doubleWriter.Dispose();
                doubleWriter = null;
            }
        }
    }
}
