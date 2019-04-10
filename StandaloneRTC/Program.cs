using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace StandaloneRTC
{
	static class Program
	{
		static Form loaderObject;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Make sure we resolve our dlls
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			
			//Remove any zone files on the dlls because Windows likes to append them
			string dllDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RTC", "DLL");
			WhackAllMOTW(dllDir);

			var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

			int nbInstances = processes.Count(prc => prc == "STANDALONERTC");

			if (nbInstances > 1)
			{
				MessageBox.Show("RTC cannot run more than once at the time in Detached mode.\nLoading aborted", "StandaloneRTC.exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			StartLoader(args);
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
		/// Global exceptions in Non User Interfarce(other thread) antipicated error
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
		/// Global exceptions in User Interfarce antipicated error
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
				RTCV.NetCore.SyncObjectSingleton.SyncObjectExecute(loaderObject, (o, ea) =>{
					loaderObject.Close();
				});
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
						if (asm.FullName == requested)
						{
							return asm;
						}

					//load missing assemblies by trying to find them in the dll directory
					string dllname = new AssemblyName(requested).Name + ".dll";
					string directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RTC", "DLL");
					string simpleName = new AssemblyName(requested).Name;
					string fname = Path.Combine(directory, dllname);
					if (!File.Exists(fname))
					{
						return null;
					}
					
					//it is important that we use LoadFile here and not load from a byte array; otherwise mixed (managed/unamanged) assemblies can't load
					return Assembly.LoadFile(fname);
				}
			}catch(Exception e)
			{
				MessageBox.Show("Something went really wrong in AssemblyResolve. Send this to the devs\n" + e);
				return null;
			}
		}

		[DllImport("kernel32.dll", EntryPoint = "DeleteFileW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
		static extern bool DeleteFileW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);
		public static void RemoveMOTW(string path)
		{
			DeleteFileW(path + ":Zone.Identifier");
		}
		//Lifted from Bizhawk
		static void WhackAllMOTW(string dllDir)
		{
			var todo = new Queue<DirectoryInfo>(new[] { new DirectoryInfo(dllDir) });
			while (todo.Count > 0)
			{
				var di = todo.Dequeue();
				foreach (var disub in di.GetDirectories()) todo.Enqueue(disub);
				foreach (var fi in di.GetFiles("*.dll"))
					RemoveMOTW(fi.FullName);
				foreach (var fi in di.GetFiles("*.exe"))
					RemoveMOTW(fi.FullName);
			}
		}
	}
}
