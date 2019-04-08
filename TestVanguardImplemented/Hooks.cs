using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.NetCore.NetcoreCommands;
using System.Data;
using System.Runtime.CompilerServices;

namespace TestVanguardImplemented
{
	public static class Hooks
	{
		//Instead of writing code inside bizhawk, hooks are placed inside of it so will be easier
		//to upgrade BizHawk when they'll release a new version.

		// Here are the keywords for searching hooks and fixes: //RTC_HIJACK

		public static bool disableRTC;
		public static bool isNormalAdvance = false;

		private static Guid? loadGameToken = null;
		private static Guid? loadSavestateToken = null;

		public static System.Diagnostics.Stopwatch watch = null;

		public static volatile bool EMU_ALLOWED_DOUBLECLICK_FULLSCREEN = true;

		static int CPU_STEP_Count = 0;

		public static void CPU_STEP(bool isRewinding, bool isFastForwarding, bool isBeforeStep = false)
		{
			try
			{
				if (disableRTC)
					return;

				//Return out if it's being called from before the step and we're not on frame 0. If we're on frame 0, then we go as normal
				//If we can't get runbefore, just assume we don't want to run before
				if (isBeforeStep && CPU_STEP_Count != 0 && ((bool)(RTCV.NetCore.AllSpec.CorruptCoreSpec?[RTCSPEC.STEP_RUNBEFORE.ToString()] ?? false)) == false)
					return;

				isNormalAdvance = !(isRewinding || isFastForwarding);

				// Unique step hooks
				if (!isRewinding && !isFastForwarding)
					STEP_FORWARD();
				else if (isRewinding)
					STEP_REWIND();
				else if (isFastForwarding)
					STEP_FASTFORWARD();

				//Any step hook for corruption
				STEP_CORRUPT(isRewinding, isFastForwarding);
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
				MessageBox.Show("Clearing all step blastunits due to an exception within Core_Step().");
				StepActions.ClearStepBlastUnits();
			}
		}

		private static void STEP_FORWARD() //errors trapped by CPU_STEP
		{
			if (disableRTC) return;
		}

		private static void STEP_REWIND() //errors trapped by CPU_STEP
		{
			if (disableRTC) return;

			if (StepActions.ClearStepActionsOnRewind)
				StepActions.ClearStepBlastUnits();
		}

		private static void STEP_FASTFORWARD() //errors trapped by CPU_STEP
		{
			if (disableRTC) return;
		}

		private static void STEP_CORRUPT(bool _isRewinding, bool _isFastForwarding) //errors trapped by CPU_STEP
		{
			if (disableRTC) return;

			if (!_isRewinding)
			{
				StepActions.Execute();
			}

			if (_isRewinding || _isFastForwarding)
				return;

			CPU_STEP_Count++;

			bool autoCorrupt = CorruptCore.AutoCorrupt;
			long errorDelay = CorruptCore.ErrorDelay;
			if (autoCorrupt && CPU_STEP_Count >= errorDelay)
			{
				CPU_STEP_Count = 0;
				BlastLayer bl = CorruptCore.GenerateBlastLayer((string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"]);
				if (bl != null)
					bl.Apply(false, true);
			}
		}

		public static void MAIN_TESTCLIENT(string[] args)
		{
			//MessageBox.Show("ATTACH DEBUGGER NOW");

			if (!System.Environment.Is64BitOperatingSystem)
			{
				MessageBox.Show("32-bit operating system detected. Bizhawk requires 64-bit to run. Program will shut down");
				Application.Exit();
			}

			try
			{
				VanguardCore.args = args;

				disableRTC = VanguardCore.args.Contains("-DISABLERTC");

				//VanguardCore.attached = true;
				VanguardCore.attached = VanguardCore.args.Contains("-ATTACHED");
				//RTC_E.isStandaloneEmu = VanguardCore.args.Contains("-REMOTERTC");

				//RTC_Unispec.RTCSpec.Update(Spec.HOOKS_SHOWCONSOLE.ToString(), RTC_Core.args.Contains("-CONSOLE"));
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void MAINFORM_FORM_LOAD_END()
		{
			try
			{
				if (disableRTC) return;

				VanguardCore.Start();
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}



		public static void MAINFORM_RESIZEEND()
		{
			try
			{
				if (disableRTC) return;

				VanguardCore.SaveBizhawkWindowState();
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void MAINFORM_CLOSING()
		{

			if (disableRTC) return;

			//Todo
			//RTC_UICore.CloseAllRtcForms();

		}


		public static void LOAD_GAME_BEGIN()
		{
			try
			{
				if (disableRTC) return;

				isNormalAdvance = false;

				StepActions.ClearStepBlastUnits();
				CPU_STEP_Count = 0;
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		static string lastGameName = "";

		public static void LOAD_GAME_DONE()
		{
			try
			{
				if (disableRTC) return;


				//Glitch Harvester warning for archives



				//prepare memory domains in advance on bizhawk side
				bool domainsChanged = RefreshDomains(false);

				PartialSpec gameDone = new PartialSpec("VanguardSpec");
				gameDone[VSPEC.SYSTEM] = EMU_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper();
				gameDone[VSPEC.GAMENAME] = EMU_GET_FILESYSTEMGAMENAME();
				gameDone[VSPEC.SYSTEMPREFIX] = EMU_GET_SAVESTATEPREFIX();
				gameDone[VSPEC.SYSTEMCORE] = EMU_GET_SYSTEMCORENAME("");
				gameDone[VSPEC.SYNCSETTINGS] = EMU_GETSET_SYNCSETTINGS;
				gameDone[VSPEC.OPENROMFILENAME] = Program.RomFilename;
				gameDone[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] = VanguardCore.GetBlacklistedDomains(EMU_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper());
				gameDone[VSPEC.MEMORYDOMAINS_INTERFACES] = GetInterfaces();
				VanguardCore.VanguardSpec.Update(gameDone);

				//This is local. If the domains changed it propgates over netcore
				LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_EVENT_DOMAINSUPDATED, domainsChanged, true);


				if (VanguardCore.GameName != lastGameName)
				{
				}
				else
				{
				}

				lastGameName = VanguardCore.GameName;
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void LOAD_GAME_FAILED()
		{
			if (disableRTC) return;

		}

		static bool CLOSE_GAME_loop_flag = false;

		public static bool AllowCaptureRewindState = true;

		public static void CLOSE_GAME(bool loadDefault = false)
		{
			try
			{
				if (disableRTC) return;

				if (CLOSE_GAME_loop_flag == true)
					return;

				CLOSE_GAME_loop_flag = true;

				//RTC_Core.AutoCorrupt = false;

				StepActions.ClearStepBlastUnits();

				MemoryDomains.Clear();

				VanguardCore.OpenRomFilename = null;

				if (loadDefault)
					VanguardCore.LoadDefaultRom();

				//RTC_RPC.SendToKillSwitch("UNFREEZE");

				CLOSE_GAME_loop_flag = false;
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void RESET()
		{
			if (disableRTC) return;
		}

		public static void LOAD_SAVESTATE_BEGIN()
		{
			if (disableRTC) return;

		}

		public static void LOAD_SAVESTATE_END()
		{
			if (disableRTC) return;


		}

		public static void EMU_CRASH(string msg)
		{
			if (disableRTC) return;

			//MessageBox.Show("SORRY EMULATOR CRASHED\n\n" + msg);

			if (VanguardCore.ShowErrorDialog(new CustomException("SORRY EMULATOR CRASHED", msg), true) == DialogResult.Abort)
				throw new RTCV.NetCore.AbortEverythingException();

		}

		public static bool HOTKEY_CHECK(string trigger)
		{// You can go to the injected Hotkey Hijack by searching #HotkeyHijack
			
				return true;
		}

		public static bool IsAllowedBackgroundInputForm()
		{
			if (disableRTC) return false;

			return RTCV.Vanguard.VanguardConnector.IsUIForm();

		}

		public static Bitmap EMU_GET_SCREENSHOT()
		{
			return new Bitmap(0,0);
		}


		public static string EMU_GET_FILESYSTEMCORENAME()
		{
			try
			{
				return "DEFAULTCORENAME";
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return null;
			}
		}

		public static string EMU_GET_FILESYSTEMGAMENAME()
		{
			try
			{
				return Path.GetFileName(Program.RomFilename);
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return null;
			}
		}

		public static string EMU_GET_CURRENTLYLOADEDSYSTEMNAME()
		{
			try
			{
				return "DEFAULTSYSTEMNAME";
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return null;
			}
		}

		public static string EMU_GET_CURRENTLYOPENEDROM()
		{
			try
			{
				return Path.GetFileName(Program.RomFilename);
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return null;
			}
		}

		public static bool EMU_ISNULLEMULATORCORE()
		{
			try
			{
				return false;
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return false;
			}
		}

		public static bool EMU_ISMAINFORMVISIBLE()
		{
			try
			{
				return true;
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return false;
			}
		}

		public static void EMU_LOADROM(string RomFile)
		{
			try
			{
				LOAD_GAME_BEGIN();
				Program.LoadRom(RomFile);
				LOAD_GAME_DONE();
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
				LOAD_GAME_FAILED();
				return;
			}
		}

		public static void EMU_OPEN_HEXEDITOR_ADDRESS(MemoryDomainProxy mdp, long address)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return;
			}
		}

		public static Size EMU_GETSET_MAINFORMSIZE
		{
			get
			{
				return new Size(100,100);
			}
			set
			{
				
			}
		}

		public static Point EMU_GETSET_MAINFORMLOCATION
		{
			get
			{
				return new Point(100, 100);

			}
			set
			{
				
			}
		}


		public static void EMU_STARTSOUND()
		{
			
		}

		public static void EMU_STOPSOUND()
		{
		}

		public static void EMU_MAINFORM_CLOSE()
		{
			Application.Exit();
		}

		public static void EMU_MAINFORM_FOCUS()
		{
			
		}

		public static void EMU_MAINFORM_SAVECONFIG()
		{

		}

		public static string EMU_GET_SAVESTATEPREFIX()
		{
			return Path.GetFileName(Program.RomFilename);
		}

		public static void EMU_LOADSTATE(string path)
		{
			Console.Write(File.ReadAllLines(path));
		}

		public static void EMU_SAVESTATE(string path, string quickSlotName)
		{
			File.WriteAllText(path, "SAVESTATE");
		}

		public static void EMU_OSDMESSAGE(string message)
		{

		}

		public static void EMU_MERGECONFIGINI(string backupConfigPath, string stockpileConfigPath)
		{
			
		}

		public static void EMU_IMPORTCONFIGINI(string importConfigPath, string stockpileConfigPath)
		{
			
		}


		public static void EMU_SET_SYSTEMCORE(string systemName, string systemCore)
		{

		}

		public static string EMU_GET_SYSTEMCORENAME(string systemName)
		{
			return "DEFAULTSYSTEMNAME";
		}

		public static string EMU_GETSET_SYNCSETTINGS
		{
			get
			{
				return "SYNCSETTINGS";
			}
			set
			{
				
			}
		}

		public static void EMU_STARTRECORDAV(string videowritername, string filename, bool unattended)
		{
			
		}

		public static void EMU_STOPRECORDAV()
		{
			
		}


		public static bool RefreshDomains(bool updateSpecs = true)
		{
			try
			{

				//Compare the old to the new. If the name and sizes are all the same, don't push that there were changes.
				//We need to compare like this because the domains can change from syncsettings.
				//We only check name and size as those are the only things that can change on the fly
				var oldInterfaces = VanguardCore.MemoryInterfacees;
				var newInterfaces = GetInterfaces();
				bool domainsChanged = false;

				if (oldInterfaces.Length != newInterfaces.Length)
					domainsChanged = true;

				for (int i = 0; i < oldInterfaces.Length; i++)
				{
					if (domainsChanged)
						break;
					if (oldInterfaces[i].Name != newInterfaces[i].Name)
						domainsChanged = true;
					if (oldInterfaces[i].Size != newInterfaces[i].Size)
						domainsChanged = true;
				}

				//We gotta push this no matter what since it's new underlying objects
				if (updateSpecs)
				{
					VanguardCore.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES.ToString(), GetInterfaces());
					LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_EVENT_DOMAINSUPDATED, domainsChanged, true);
				}


				return domainsChanged;

			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return false;
			}
		}

		public static MemoryDomainProxy[] GetInterfaces()
		{
			try
			{
				Console.WriteLine($" getInterfaces()");

				List<MemoryDomainProxy> interfaces = new List<MemoryDomainProxy>();

				foreach(var md in Program.MemoryDomains)
					interfaces.Add(new MemoryDomainProxy(md));

				return interfaces.ToArray();
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return new MemoryDomainProxy[] { };
			}

		}


	}
}
