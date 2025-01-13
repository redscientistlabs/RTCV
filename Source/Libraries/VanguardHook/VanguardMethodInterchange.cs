using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.CorruptCore.Extensions;
using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace VanguardHook
{
    // This class contains all the methods exported for use by the target emulator. These are called by the 
    // "CallImportedFunction" function in the emulator's source code.
    class MethodExports
    {
        // Called when the emulator initally starts up. Loads all DLLs required and then
        // starts the Vanguard connection to RTC
        public static Dictionary<string, Assembly> assemblies;
        [DllExport("InitVanguard")] 
        public static void InitVanguard([MarshalAs(UnmanagedType.BStr)] string emuDir)
        {
            assemblies = new Dictionary<string, Assembly>();

            //create assembly load and resolve event handlers so that we can correctly load the DLLs when needed
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Load_Dlls();
            EmuDirectory.emuDir = emuDir;

            VanguardCore.Start();
        }

        // Called during Vanguard initialization. Loads all RTC DLLs required for Vanguard to operate.
        public static void Load_Dlls()
        {
            Assembly.LoadFrom("..\\RTCV\\Newtonsoft.Json.dll");
            Assembly.LoadFrom("..\\RTCV\\CorruptCore.dll");
            Assembly.LoadFrom("..\\RTCV\\NetCore.dll");
            Assembly.LoadFrom("..\\RTCV\\RTCV.Common.dll");
            Assembly.LoadFrom("..\\RTCV\\Vanguard.dll");
        }

        // Called during Vanguard initilization. Event that triggers when attempting to load a DLL.
        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {

            Assembly assembly = args.LoadedAssembly;
            assemblies[assembly.FullName] = assembly;
        }

        // Called during Vanguard initilization. Event to triggers when attempting to resolve the path to a DLL.
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            Assembly assembly = null;

            assemblies.TryGetValue(args.Name, out assembly);

            return assembly;

        }

        // Called when the emulator initially starts up and if the argument for showing the console
        // was used. Enables the console to view debug messages
        [DllExport("SHOWCONSOLE")]
        public static void ShowConsole()
        {
            ConsoleHelper.ShowConsole();
        }

        // Called when a game is selected on the emulator. Passes the rompath and stores it for
        // later use.
        public static string GAME_TO_LOAD = "";
        [DllExport("GAMETOLOAD")]
        public static void GAMETOLOAD([MarshalAs(UnmanagedType.BStr)] string rompath)
        {
            GAME_TO_LOAD = rompath;
        }

        // Called when a game succesfully starts loading on the emulator. Passes the rompath returned
        // by the emulator. If the path could not be found here, fallback to the rompath stored during
        // the "GAMETOLOAD" method.
        [DllExport("LOADGAMESTART")]
        public static void LOADGAMESTART([MarshalAs(UnmanagedType.BStr)] string rompath)
        {
            ConsoleEx.WriteLine("LOAD_GAME_START");
            StepActions.ClearStepBlastUnits();
            RtcClock.ResetCount();

            if (rompath == "EMPTY")
            {
                rompath = GAME_TO_LOAD;
                GAME_TO_LOAD = "";
            }
            AllSpec.VanguardSpec.Update(VSPEC.OPENROMFILENAME, rompath, true, true);
            ConsoleEx.WriteLine(AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME].ToString());
        }
        
        // Called when a game succesfully finishes loading on the emulator. Finishes updating the
        // spec for Vanguard and loads memory domains.
        [DllExport("LOADGAMEDONE")]
        public static void LOADGAMEDONE([MarshalAs(UnmanagedType.BStr)] string gamename)
        {
            ConsoleEx.WriteLine("LOAD_GAME_DONE");
            if (AllSpec.UISpec == null)
            {
                VanguardCore.StopGame();
                string template = $"It appears you haven't connected to StandaloneRTC. Please make sure that the " +
                "RTC is running and not just {0}. If you have an antivirus, it might be " +
                "blocking the RTC from launching.\n\nIf you keep getting this message, poke " +
                "the RTC devs for help (Discord is in the launcher).";
                string message = string.Format(template, AllSpec.VanguardSpec[VSPEC.NAME]);

                MessageBox.Show(message, "RTC Not Connected");
                return;
            }
            PartialSpec gameDone = new PartialSpec("VanguardSpec");
            gameDone[VSPEC.SYSTEM] = VanguardConfigReader.configFile.VSpecConfig.NAME;
            gameDone[VSPEC.SYSTEMPREFIX] = VanguardConfigReader.configFile.VSpecConfig.NAME;
            gameDone[VSPEC.SYNCSETTINGS] = "";

            // remove any invalid file characters before storing it
            string gamenameFixed = StringExtensions.MakeSafeFilename(gamename, '-');

            gameDone[VSPEC.GAMENAME] = gamenameFixed;

            if (gamenameFixed != AllSpec.VanguardSpec[VSPEC.GAMENAME].ToString())
            {
                gameDone[VSPEC.SYSTEMCORE] = Marshal.PtrToStringAnsi(MethodImports.Vanguard_getSystemCore());
            }

            AllSpec.VanguardSpec.Update(gameDone);
            VanguardImplementation.RefreshDomains();
            RtcCore.InvokeLoadGameDone();
            MethodImports.Vanguard_finishLoading();
        }
        
        // Called when a game is closed on the emulator. Updates the spec so it doesn't
        // think a game is open and removes memory domains.
        [DllExport("GAMECLOSED")]
        public static void GAMECLOSED()
        {
            ConsoleEx.WriteLine("GAMECLOSED");
            PartialSpec gameClosed = new PartialSpec("VanguardSpec");
            gameClosed[VSPEC.OPENROMFILENAME] = "";
            AllSpec.VanguardSpec.Update(gameClosed);
            RtcCore.InvokeGameClosed(true);

            // If we're closing the emulator, don't refresh the domains or else it will hang
            if (VanguardImplementation.waitForEmulatorClose)
            {
                ConsoleEx.WriteLine("closing Vanguard");
                VanguardCore.StopVanguard();
            }
            else
                VanguardImplementation.RefreshDomains();
        }


        // Called when the emulator is closed. Shuts down the Vanguard connection.
        [DllExport("EMULATORCLOSING")]
        public static void EMULATORCLOSING()
        {
            VanguardImplementation.StopClient();
            RtcCore.InvokeGameClosed(true);
        }
        
        // Called when an onscreen display message attempts to display. Checks to see if the spec
        // allows it, then returns the result determining if the message should be displayed.
        [DllExport("RTCOSDENABLED")]
        public static bool RTCOSDENABLED()
        {
            if (!VanguardImplementation.enableRTC)
            {
                return true;
            }
            if (RTCV.NetCore.Params.IsParamSet(RTCSPEC.CORE_EMULATOROSDDISABLED))
            {
                return false;
            }
            return true;
        }

        // Called when the emulator is in a safe state to read/write data, typically when it
        // is also applying patches. Executes all RTC step actions.
        [DllExport("CORESTEP")]
        public static void CORE_STEP()
        {
            StepActions.Execute();
            RtcClock.StepCorrupt(true, true);
        }
    }

    // This class contains all the methods from Windows DLLs needed for importing
    // methods from the target emulator.
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        // These four are used for detecting if the currently selected window is the emulator
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hWnd, IntPtr hModule, StringBuilder lpFileName, int nSize);
    }

    // This class contains all the methods imported for use from the target emulator. These are found in the 
    // "VanguardHelpers.cpp" file in the emulator's source code.
    class MethodImports
    {

        //load the emulator exe and creates a pointer to it
        public static IntPtr pEXE = LoadEmuPointer();

        //
        //Import all supported functions from the emulator
        //
        public delegate byte Vpeekbyte(long addr, int selection = 0);
        public static Vpeekbyte Vanguard_peekbyte = GetMethod<Vpeekbyte>("Vanguard_peekbyte");

        public delegate void Vpokebyte(long addr, byte buf, int selection = 0);
        public static Vpokebyte Vanguard_pokebyte = GetMethod<Vpokebyte>("Vanguard_pokebyte");

        public delegate void Vpause(bool pauseUntilCorrupt = false);
        public static Vpause Vanguard_pause = GetMethod<Vpause>("Vanguard_pause");

        public delegate void Vresume();
        public static Vresume Vanguard_resume = GetMethod<Vresume>("Vanguard_resume");

        public delegate void Vsavesavestate([MarshalAs(UnmanagedType.BStr)] string filename, bool wait = false);
        public static Vsavesavestate Vanguard_savesavestate = GetMethod<Vsavesavestate>("Vanguard_savesavestate");

        public delegate void Vloadsavestate([MarshalAs(UnmanagedType.BStr)] string filename);
        public static Vloadsavestate Vanguard_loadsavestate = GetMethod<Vloadsavestate>("Vanguard_loadsavestate");

        public delegate void VLoadROM([MarshalAs(UnmanagedType.BStr)] string filename);
        public static VLoadROM Vanguard_loadROM = GetMethod<VLoadROM>("Vanguard_loadROM");

        public delegate void VfinishLoading();
        public static VfinishLoading Vanguard_finishLoading = GetMethod<VfinishLoading>("Vanguard_finishLoading");

        public delegate void VcloseGame();
        public static VcloseGame Vanguard_closeGame = GetMethod<VcloseGame>("Vanguard_closeGame");

        public delegate void VprepShutdown();
        public static VprepShutdown Vanguard_prepShutdown = GetMethod<VprepShutdown>("Vanguard_prepShutdown");

        public delegate void VforceStop();
        public static VforceStop Vanguard_forceStop = GetMethod<VforceStop>("Vanguard_forceStop");

        public delegate IntPtr VgetSystemCore();
        public static VgetSystemCore Vanguard_getSystemCore = GetMethod<VgetSystemCore>("Vanguard_getSystemCore");

        // loads the emulator exe and returns a pointer for importing exported functions
        public static IntPtr LoadEmuPointer()
        {
            IntPtr pDll = NativeMethods.LoadLibrary(EmuDirectory.emuEXE);
            return pDll;
        }

        // tries to find a target method to import from the emulator. If it cannot find
        // one it returns default, so extra failsafes will be needed to make sure you
        // don't use a method that only exists for some emulators and not others.
        public static T GetMethod<T>(string MethodName)
        {
            IntPtr procAddr = NativeMethods.GetProcAddress(pEXE, MethodName);
            if (procAddr.ToInt64() != 0)
            {
                T Method = Marshal.GetDelegateForFunctionPointer<T>(procAddr);
                return Method;
            }
            return default;
        }
    }
}
