using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PrereqChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            var d3dx9 = new Dependency("DirectX9 End-User Runtime", "d3dx9_43.dll", "https://download.microsoft.com/download/8/4/A/84A35BF1-DAFE-4AE8-82AF-AD2AE20B6B14/directx_Jun2010_redist.exe", "/silent");
            var vc2015 = new Dependency("Visual C++ 2015-2019 x64", "msvcp140.dll", "https://download.visualstudio.microsoft.com/download/pr/9e04d214-5a9d-4515-9960-3d71398d98c3/1e1e62ab57bbb4bf5199e8ce88f040be/vc_redist.x64.exe",
                "/install /passive /norestart");
            var vc2015x86 = new Dependency("Visual C++ 2015-2019 x86", "msvcp140.dll", "https://download.visualstudio.microsoft.com/download/pr/c8edbb87-c7ec-4500-a461-71e8912d25e9/99ba493d660597490cbb8b3211d2cae4/vc_redist.x86.exe",
                "/install /passive /norestart");
            var vc2012 = new Dependency("Visual C++ 2012 x64", "msvcr120.dll", "https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe", "/install /passive /norestart");
            var vc2010 = new Dependency("Visual C++ 2010 x64", "msvcr100.dll", "https://download.microsoft.com/download/3/2/2/3224B87F-CFA0-4E70-BDA3-3DE650EFEBA5/vcredist_x64.exe", "/passive /norestart");
            List<Dependency> deps = new List<Dependency>();

            if (Environment.Is64BitProcess)
            {
                if (Win32.LoadLibrary(d3dx9.File) != IntPtr.Zero)
                    deps.Add(d3dx9);

                if (Win32.LoadLibrary(vc2015.File) != IntPtr.Zero)
                    deps.Add(vc2015);

                if (Win32.LoadLibrary(vc2012.File) != IntPtr.Zero)
                    deps.Add(vc2012);

                if (Win32.LoadLibrary(vc2010.File) != IntPtr.Zero)
                    deps.Add(vc2010);
            }
            else
            {
                if (Win32.LoadLibrary(vc2015x86.File) != IntPtr.Zero)
                    deps.Add(vc2015x86); //VC++2015 x86 for PCSX2
            }

            Console.WriteLine("You are missing the following dependencies: ");
            foreach (var d in deps)
                Console.WriteLine(d.Name);

            if (!IsAdministrator())
            {
                Console.WriteLine("Requesting admin permissions to install...");

                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                var p = System.Diagnostics.Process.Start(startInfo);
                p.WaitForExit();
                return;
            }

            foreach (var d in deps)
            {
                Console.WriteLine($"Downloading {d.Name}");
                var f = GetFileViaHttp(d.DownloadLink);
                var name = d.DownloadLink.Split('/').Last();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "REDISTS", name);
                File.WriteAllBytes(path, f);

                Console.WriteLine($"Installing {d.Name}");
                var p = new ProcessStartInfo();
                p.FileName = path;
                var _p = Process.Start(p);
                _p.WaitForExit();
                Console.WriteLine($"Installation of {d.Name}: {(_p.ExitCode == 0 ? "successful" : "failed")}");
            }

            Console.WriteLine("Done");
        }

        private static class Win32
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);
        }

        private class Dependency
        {
            public string Name { get; }
            public string File { get; }
            public string DownloadLink { get; }
            public string InstallString { get; }

            public Dependency(string name, string file, string downloadLink, string installString)
            {
                Name = name;
                File = file;
                DownloadLink = downloadLink;
                InstallString = installString;
            }
        }


        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);

        }
        public static byte[] GetFileViaHttp(string url)
        {
            //Windows does the big dumb: part 11
            WebRequest.DefaultWebProxy = null;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(12000);
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
    }
}
