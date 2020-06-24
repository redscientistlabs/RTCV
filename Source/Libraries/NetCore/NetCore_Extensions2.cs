namespace RTCV.NetCore.NetCore_Extensions2
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using Ceras;

    public static class ObjectCopier
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (source == null)
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public static class ConsoleHelper
    {
        public static ConsoleCopy con;

        public static void CreateConsole(string path = null)
        {
            ReleaseConsole();
            AllocConsole();
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
            }
        }
    }

    //Thanks to Riki, dev of Ceras for writing this
    public class HashSetFormatterThatKeepsItsComparer : Ceras.Formatters.IFormatter<HashSet<byte[]>>
    {
        // Sub-formatters are automatically set by Ceras' dependency injection
        public Ceras.Formatters.IFormatter<byte[]> _byteArrayFormatter;
        public Ceras.Formatters.IFormatter<IEqualityComparer<byte[]>> _comparerFormatter; // auto-implemented by Ceras using DynamicObjectFormatter

        public void Serialize(ref byte[] buffer, ref int offset, HashSet<byte[]> set)
        {
            // What do we need?
            // - The comparer
            // - Number of entries
            // - Actual content

            // Comparer
            _comparerFormatter.Serialize(ref buffer, ref offset, set.Comparer);

            // Count
            // We could use a 'IFormatter<int>' field, but Ceras will resolve it to this method anyway...
            SerializerBinary.WriteInt32(ref buffer, ref offset, set.Count);

            // Actual content
            foreach (var array in set)
            {
                _byteArrayFormatter.Serialize(ref buffer, ref offset, array);
            }
        }

        public void Deserialize(byte[] buffer, ref int offset, ref HashSet<byte[]> set)
        {
            IEqualityComparer<byte[]> equalityComparer = null;
            _comparerFormatter.Deserialize(buffer, ref offset, ref equalityComparer);

            // We can already create the hashset
            set = new HashSet<byte[]>(equalityComparer);

            // Read content...
            var count = SerializerBinary.ReadInt32(buffer, ref offset);
            for (var i = 0; i < count; i++)
            {
                byte[] ar = null;
                _byteArrayFormatter.Deserialize(buffer, ref offset, ref ar);

                set.Add(ar);
            }
        }
    }

    public class NullableByteHashSetFormatterThatKeepsItsComparer : Ceras.Formatters.IFormatter<HashSet<byte?[]>>
    {
        // Sub-formatters are automatically set by Ceras' dependency injection
        public Ceras.Formatters.IFormatter<byte?[]> _byteArrayFormatter;
        public Ceras.Formatters.IFormatter<IEqualityComparer<byte?[]>> _comparerFormatter; // auto-implemented by Ceras using DynamicObjectFormatter

        public void Serialize(ref byte[] buffer, ref int offset, HashSet<byte?[]> set)
        {
            // What do we need?
            // - The comparer
            // - Number of entries
            // - Actual content

            // Comparer
            _comparerFormatter.Serialize(ref buffer, ref offset, set.Comparer);

            // Count
            // We could use a 'IFormatter<int>' field, but Ceras will resolve it to this method anyway...
            SerializerBinary.WriteInt32(ref buffer, ref offset, set.Count);

            // Actual content
            foreach (var array in set)
            {
                _byteArrayFormatter.Serialize(ref buffer, ref offset, array);
            }
        }

        public void Deserialize(byte[] buffer, ref int offset, ref HashSet<byte?[]> set)
        {
            IEqualityComparer<byte?[]> equalityComparer = null;
            _comparerFormatter.Deserialize(buffer, ref offset, ref equalityComparer);

            // We can already create the hashset
            set = new HashSet<byte?[]>(equalityComparer);

            // Read content...
            var count = SerializerBinary.ReadInt32(buffer, ref offset);
            for (var i = 0; i < count; i++)
            {
                byte?[] ar = null;
                _byteArrayFormatter.Deserialize(buffer, ref offset, ref ar);

                set.Add(ar);
            }
        }
    }
}
