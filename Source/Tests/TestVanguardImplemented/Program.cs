using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;

namespace TestVanguardImplemented
{
	public static class Program
	{
		public static Form SyncForm;
		public static volatile bool SpecsSent = false;
		public static string RomFilename;

		public static List<IMemoryDomain> MemoryDomains;
		[STAThread]
		public static void Main(string[] args)
		{
			SyncForm = new Form();

			Hooks.MAIN_TESTCLIENT(args);

			Hooks.MAINFORM_FORM_LOAD_END();
			

			while (!SpecsSent)
			{
				Application.DoEvents();
				Thread.Sleep(16);
			}

			while (true)
			{
				Hooks.CPU_STEP(false, false);
				Application.DoEvents();
				Thread.Sleep(16);
			}
		}

		public static bool LoadRom(string romFilename)
		{
			InitializeMemoryDomains();
			if (!File.Exists(CorruptCore.bizhawkDir + "GAME"))
				File.Create(CorruptCore.bizhawkDir + "GAME");
			RomFilename = CorruptCore.bizhawkDir + "GAME";
			return true;
		}

		public static void InitializeMemoryDomains()
		{
			MemoryDomains = new List<IMemoryDomain>();


			byte[] empty = new byte[2048];
			MemoryDomains.Add(new TestVanguardImplementation.TestMemoryDomain(empty, "Empty", empty.Length, 1, false));

			byte[] ff = new byte[4096];
			for (int i = 0; i < ff.Length; i++)
				ff[i] = 255;
			MemoryDomains.Add(new TestVanguardImplementation.TestMemoryDomain(ff, "FF", ff.Length, 1, false));

			byte[] increment = new byte[1024];
			for (int i = 0; i < increment.Length; i++)
				increment[i] = (byte)(i % 255);
			MemoryDomains.Add(new TestVanguardImplementation.TestMemoryDomain(increment, "Increment", increment.Length, 1, false));

		}
	}
}
