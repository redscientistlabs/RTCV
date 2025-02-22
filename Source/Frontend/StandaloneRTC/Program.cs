namespace StandaloneRTC
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    static class Program
    {
        static Form loaderObject;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (Mutex mutex = new Mutex(true, "StandaloneRTC", out bool createdNew))
            {
                if (createdNew)
                {
                    //Make sure we resolve our dlls
                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                    StartLoader(args);
                }
                else
                {
                    MessageBox.Show("RTC cannot run more than once at the time in Detached mode.\nLoading aborted", "StandaloneRTC.exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// Starts the loader. Separated from Main so the AssemblyResolve code can properly function.
        /// </summary>
        /// <param name="args"></param>
        private static void StartLoader(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            loaderObject = new Loader(args);
            Application.Run(loaderObject);
        }

        /// <summary>
        /// Global exceptions in Non User Interface(other thread) anticipated error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Form error = new RTCV.NetCore.CloudDebug(ex);
            var result = error.ShowDialog();
        }

        /// <summary>
        /// Global exceptions in User Interface anticipated error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Form error = new RTCV.NetCore.CloudDebug(ex);
            var result = error.ShowDialog();

            if (result == DialogResult.Abort)
            {
                RTCV.NetCore.SyncObjectSingleton.SyncObjectExecute(loaderObject, (o, ea) => { loaderObject.Close(); });
            }
        }

        //Lifted from Bizhawk
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string requested = args.Name;
                lock (AppDomain.CurrentDomain)
                {
                    var asms = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var asm in asms)
                    {
                        if (asm.FullName == requested)
                        {
                            return asm;
                        }
                    }

                    //load missing assemblies by trying to find them in the dll directory
                    string dllname = new AssemblyName(requested).Name + ".dll";
                    string location = Assembly.GetExecutingAssembly().Location;
                    string directory = Path.GetDirectoryName(location);
                    string simpleName = new AssemblyName(requested).Name;
                    string fname = Path.Combine(directory, dllname);
                    if (!File.Exists(fname))
                    {
                        return null;
                    }

                    //it is important that we use LoadFile here and not load from a byte array; otherwise mixed (managed/unamanged) assemblies can't load
                    return Assembly.UnsafeLoadFrom(fname);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went really wrong in AssemblyResolve. Send this to the devs\n" + e);
                return null;
            }
        }
    }
}
