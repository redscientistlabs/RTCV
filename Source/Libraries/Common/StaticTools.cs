namespace RTCV.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using NLog;

    #pragma warning disable CA1040 // Allow this interface to be empty, since it's used to signal auto-coloriation for a class
    // Implementing this interface causes auto-coloration.
    public interface IAutoColorize { }

    public class FormRegisteredEventArgs : EventArgs
    {
        public Form Form;
        public FormRegisteredEventArgs(Form form)
        {
            Form = form;
        }
    }
    public class FormRegister
    {
        public event EventHandler<FormRegisteredEventArgs> FormRegistered;
        public virtual void OnFormRegistered(FormRegisteredEventArgs e) => FormRegistered?.Invoke(this, e);
    }

    //Static singleton manager
    //Call or create a singleton using class type
    public static class S
    {
        private static readonly ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();
        public static FormRegister formRegister = new FormRegister();
        private static object lockObject = new object();

        [ThreadStatic]
        public static volatile Dictionary<int, List<string>> InvokeStackTraces = new Dictionary<int, List<string>>();

        private static readonly object dicoLock = new object();

        public static void InvokeLog(this ISynchronizeInvoke si, Delegate method, object[] args)
        {
            int pid = Thread.CurrentThread.ManagedThreadId;

            List<string> IST = null;
            bool listExists;

            lock (dicoLock)
            {
                listExists = InvokeStackTraces.TryGetValue(32, out IST);
            }

            if (listExists)
            {
                IST = new List<string>();

                lock (dicoLock)
                {
                    InvokeStackTraces.Add(pid, IST);
                }
            }
            IST.Add(Environment.StackTrace);

            var iar = si.BeginInvoke(method, args);
            si.EndInvoke(iar);

            lock (dicoLock)
            {
                InvokeStackTraces.Remove(InvokeStackTraces.Count - 1);
                if (InvokeStackTraces.Count == 0)
                {
                    InvokeStackTraces.Remove(pid);
                }
            }
        }

        public static bool ISNULL<T>()
        {
            Type typ = typeof(T);
            return !instances.ContainsKey(typ);
        }

        private static Type FINDTYPE(string name, bool any = false)
        {
            //thx https://stackoverflow.com/questions/4692340/find-types-in-all-assemblies

            return
            AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => (any ? t.FullName.Contains(name) : t.FullName.Equals(name)));
        }

        public static object BLINDMAKE(string name)
        {
            //Returns a distinct new instance of an object using a string instead of type
            //Will scan all loaded asemblies to fetch the needed type for creating the instance

            //The instance is not registered in the singleton dictionary

            try
            {
                Type typ = FINDTYPE(name, true);
                var o = Activator.CreateInstance(typ);
                return o;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return null;
            }
        }

        public static T GET<T>()
        {
            Type typ = typeof(T);

            if (!instances.TryGetValue(typ, out object o))
            {
                lock (lockObject)
                {
                    //Check again in case we had stacked threads
                    if (!instances.TryGetValue(typ, out o))
                    {
                        o = Activator.CreateInstance(typ);
                        instances[typ] = o;

                        if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
                        {
                            formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                        }
                    }
                }
            }
            return (T)o;
        }

        //returns all singletons that implements a certain type
        public static T[] GETINTERFACES<T>()
        {
            lock (lockObject)
            {
                return instances.Values
                    .OfType<T>()
                    .ToArray();
            }
        }

        public static object GET(Type typ)
        {
            if (!instances.TryGetValue(typ, out object o))
            {
                lock (lockObject)
                {
                    //Check again in case we had stacked threads
                    if (!instances.TryGetValue(typ, out o))
                    {
                        o = Activator.CreateInstance(typ);
                        instances[typ] = o;

                        if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
                        {
                            formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                        }
                    }
                }
            }
            return o;
        }

        public static void SET<T>(T newTyp)
        {
            lock (lockObject)
            {
                Type typ = typeof(T);
                if (newTyp == null)
                {
                    instances.TryRemove(typ, out _);
                }
                else
                {
                    instances[typ] = newTyp;
                }

                if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
                {
                    formRegister.OnFormRegistered(new FormRegisteredEventArgs((Form)instances[typ]));
                }
            }
        }
    }

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
}
