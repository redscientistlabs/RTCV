using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace PrereqChecker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var processModule = Process.GetCurrentProcess().MainModule;
            var dir = Path.GetDirectoryName(processModule.FileName);
            var redistDir = Path.Combine(Directory.GetCurrentDirectory(), "REDISTS");
            Directory.CreateDirectory(redistDir);

            var depsToInstall = new List<Dependency>();
            var d3dx9Setup = new Dependency("DirectX9 End-User Runtime", "DXSETUP.exe", "", $"/silent");
            var d3dx9 = new Dependency("DirectX9 End-User Runtime", "d3dx9_43.dll", "https://download.microsoft.com/download/8/4/A/84A35BF1-DAFE-4AE8-82AF-AD2AE20B6B14/directx_Jun2010_redist.exe", $"/Q /T:{redistDir}", d3dx9Setup);
            
            var vc2015 = new Dependency("Visual C++ 2015-2019 x64", "msvcp140.dll", "https://download.visualstudio.microsoft.com/download/pr/9e04d214-5a9d-4515-9960-3d71398d98c3/1e1e62ab57bbb4bf5199e8ce88f040be/vc_redist.x64.exe",
                "/install /passive /norestart");
            var vc2015x86 = new Dependency("Visual C++ 2015-2019 x86", "msvcp140.dll", "https://download.visualstudio.microsoft.com/download/pr/c8edbb87-c7ec-4500-a461-71e8912d25e9/99ba493d660597490cbb8b3211d2cae4/vc_redist.x86.exe",
                "/install /passive /norestart");
            var vc2013 = new Dependency("Visual C++ 2013 x64", "msvcr120.dll", "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe", "/install /passive /norestart");
            var vc2012 = new Dependency("Visual C++ 2012 x64", "msvcr110.dll", "https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe", "/install /passive /norestart");
            var vc2010 = new Dependency("Visual C++ 2010 x64", "msvcr100.dll", "https://download.microsoft.com/download/3/2/2/3224B87F-CFA0-4E70-BDA3-3DE650EFEBA5/vcredist_x64.exe", "/passive /norestart");
            var dotNet471 = new Dependency(".Net Framework 4.7.1", "471", "https://download.visualstudio.microsoft.com/download/pr/7afca223-55d2-470a-8edc-6a1739ae3252/abd170b4b0ec15ad0222a809b761a036/ndp48-x86-x64-allos-enu.exe", "/install /x86 /x64 /passive /norestart");

            if (Environment.Is64BitProcess)
            {
                if (Win32.LoadLibrary(d3dx9.File) == IntPtr.Zero)
                    depsToInstall.Add(d3dx9); //Bizhawk

                if (Win32.LoadLibrary(vc2015.File) == IntPtr.Zero)
                    depsToInstall.Add(vc2015); //PCSX2, MelonDS, Dolphin

                if (Win32.LoadLibrary(vc2013.File) == IntPtr.Zero)
                    depsToInstall.Add(vc2013); //Bizhawk

                if (Win32.LoadLibrary(vc2012.File) == IntPtr.Zero)
                    depsToInstall.Add(vc2012); //Bizhawk

                if (Win32.LoadLibrary(vc2010.File) == IntPtr.Zero)
                    depsToInstall.Add(vc2010); //Bizhawk and RTC (SlimDX)

                if (GetDotNetVersion() < 471)
                    depsToInstall.Add(dotNet471);
            }
            else
            {
                if (Win32.LoadLibrary(vc2015x86.File) == IntPtr.Zero)
                    depsToInstall.Add(vc2015x86); //VC++2015 x86 for PCSX2
            }


            if (depsToInstall.Count > 0)
            {
                Console.WriteLine("You are missing the following dependencies: ");
                foreach (var d in depsToInstall)
                    Console.WriteLine(d.Name);

                Console.WriteLine("Would you like to download and install them? (Y/N). Default Y");
                string s = null;
                while (s != "" || s.Equals("y", StringComparison.OrdinalIgnoreCase) || s.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    s = Console.ReadLine();
                    if (s.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("The RTC may not run properly without these dependencies.");
                        Console.WriteLine("You can always re-run this tool via the RTC launcher.");
                        Thread.Sleep(3000);
                        return;
                    }
                }

                if (!IsAdministrator())
                {
                    Console.WriteLine("Requesting admin permissions to install...");
                    Thread.Sleep(500);
                    // Restart program and run as admin
                    var exeName = Process.GetCurrentProcess().MainModule.FileName;
                    var startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    try
                    {
                        var p = Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Admin permissions are required to continue.\nThe RTC may not run properly until you re-run this from the launcher.");
                        Thread.Sleep(5000);
                    }
                    return;
                }

                foreach (var d in depsToInstall)
                {
                    Console.WriteLine($"Downloading {d.Name}");
                    var name = d.DownloadLink.Split('/').Last();
                    var path = Path.Combine(redistDir, name);
                    var f = GetFileViaHttp(d.DownloadLink);
                    File.WriteAllBytes(path, f);

                    Console.WriteLine($"Installing {d.Name}");
                    var p = new ProcessStartInfo();
                    p.FileName = path;
                    p.Arguments = d.InstallString;
                    var _p = Process.Start(p);
                    _p.WaitForExit();

                    var _d = d.RunAfter;
                    while (_d != null)
                    {
                        p = new ProcessStartInfo();
                        p.FileName = Path.Combine(redistDir, _d.File);
                        p.Arguments = _d.InstallString;
                        _p = Process.Start(p);
                        _p.WaitForExit();
                        _d = _d.RunAfter;
                    }
                    Console.WriteLine("Installation of " + d.Name + " " + (_p.ExitCode == 0 ? "successful" : "failed (" + TranslateExitCode(_p.ExitCode) + ")"));
                }
            }
            

            //Daisy chain x86 to x64
            if (!Environment.Is64BitProcess)
            {
                var name = Process.GetCurrentProcess().ProcessName + "64.exe";
                if (processModule != null)
                {
                    var fileName = Path.Combine(dir, name);
                    if (File.Exists(fileName))
                    {
                        var p = new ProcessStartInfo{FileName = fileName};
                        Process.Start(p)?.WaitForExit();
                    }
                }
            }

            Console.WriteLine("Done");
            Thread.Sleep(1000);
        }


        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static byte[] GetFileViaHttp(string url)
        {
            //Windows does the big dumb: part 11
            WebRequest.DefaultWebProxy = null;

            using (var client = new HttpClient())
            {
                byte[] b = null;
                try
                {
                    b = client.GetByteArrayAsync(url).Result;
                }
                catch (AggregateException e)
                {
                    Console.WriteLine($"{url} timed out.");
                }

                return b;
            }
        }

        private static string TranslateExitCode(int code)
        {
            switch (code)
            {
                case 0x666:
                {
                    return "Cannot install a product when a newer version is installed.";
                }
            }

            return "Unknown Exit Code";
        }

        private static int GetDotNetVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null) return CheckFor45PlusVersion((int) ndpKey.GetValue("Release"));
            }

            // Checking the version using >= enables forward compatibility.
            int CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 528040)
                    return 480;
                if (releaseKey >= 461808)
                    return 472;
                if (releaseKey >= 461308)
                    return 471;
                if (releaseKey >= 460798)
                    return 470;
                if (releaseKey >= 394802)
                    return 462;
                if (releaseKey >= 394254)
                    return 461;
                if (releaseKey >= 393295)
                    return 460;
                if (releaseKey >= 379893)
                    return 452;
                if (releaseKey >= 378675)
                    return 451;
                if (releaseKey >= 378389)
                    return 450;
                return int.MaxValue;
            }

            return int.MaxValue;
        }

        private static class Win32
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);
        }

        private class Dependency
        {
            public Dependency(string name, string file, string downloadLink, string installString, Dependency runAfter = null)
            {
                Name = name;
                File = file;
                DownloadLink = downloadLink;
                InstallString = installString;
                RunAfter = runAfter;
            }

            public string Name { get; }
            public string File { get; }
            public string DownloadLink { get; }
            public string InstallString { get; }
            public Dependency RunAfter { get; }
        }
    }
}