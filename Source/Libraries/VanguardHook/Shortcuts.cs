using Newtonsoft.Json;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static VanguardHook.VanguardCore;

namespace VanguardHook
{
    class Shortcuts //I'm calling this class "Shortcuts" because it contains "shortcuts" to other functions
    {

        [DllExport("CORESTEP")]
        public static void CORE_STEP()
        {
            STEP_CORRUPT();
        }
        public static void STEP_CORRUPT()
        {
            StepActions.Execute();
            RtcClock.StepCorrupt(true, true);
        }

        public static Dictionary<string, Assembly> assemblies;
        [DllExport("InitVanguard")] 
        public static void InitVanguard([MarshalAs(UnmanagedType.BStr)] string emuDir)
        {
            assemblies = new Dictionary<string, Assembly>();

            //create assembly load and resolve event handlers so that we can correctly load the DLLs when needed
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Load_Dlls();
            emuDir = emuDir + "\\Dolphin.exe";
            EmuDirectory.emuDir = emuDir;

            VanguardCore.Start();
        }

        [DllExport("GAMETOLOAD")]
        public static void GAMETOLOAD([MarshalAs(UnmanagedType.BStr)] string rompath)
        {
            ConsoleEx.WriteLine(rompath);
            VanguardCore.GAME_TO_LOAD = rompath;
        }
        [DllExport("LOADGAMESTART")]
        public static void LOADGAMESTART([MarshalAs(UnmanagedType.BStr)] string rompath)
        {
            VanguardCore.LOAD_GAME_START(rompath);
        }
        [DllExport("LOADGAMEDONE")]
        public static void LOADGAMEDONE([MarshalAs(UnmanagedType.BStr)] string gamename)
        {
            VanguardCore.LOAD_GAME_DONE(gamename);
        }
        [DllExport("GAMECLOSED")]
        public static void GAMECLOSED()
        {
            VanguardCore.GAME_CLOSED();
        }
        [DllExport("EMULATORCLOSING")]
        public static void EMULATORCLOSING()
        {
            VanguardCore.EMULATOR_CLOSING();
        }
        [DllExport("RTCOSDENABLED")]
        public static bool RTCOSDENABLED()
        {
            return VanguardCore.RTC_OSD_ENABLED();
        }

        public static void Load_Dlls()
        {
            Assembly.LoadFrom("..\\RTCV\\Newtonsoft.Json.dll");
            Assembly.LoadFrom("..\\RTCV\\CorruptCore.dll");
            Assembly.LoadFrom("..\\RTCV\\NetCore.dll");
            Assembly.LoadFrom("..\\RTCV\\RTCV.Common.dll");
            Assembly.LoadFrom("..\\RTCV\\Vanguard.dll");
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            Assembly assembly = null;

            assemblies.TryGetValue(args.Name, out assembly);

            return assembly;

        }

        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {

            Assembly assembly = args.LoadedAssembly;
            assemblies[assembly.FullName] = assembly;
        }
    }
}
