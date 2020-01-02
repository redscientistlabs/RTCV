using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RTCV.Prereqs
{
    public partial class MainForm : Form
    {
        WebClient webClient = new WebClient();
        Queue<Dependency> downloadQueue = new Queue<Dependency>();
        ProcessModule processModule;
        private string dir;
        private string redistDir;
        public bool skipPrompt = Environment.GetCommandLineArgs().Contains("-SKIPPROMPT");
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            this.btnQuit.MouseEnter += btnQuit_MouseEnter;
            this.btnQuit.MouseLeave += btnQuit_MouseLeave;
            pnTopPanel.MouseMove += PnTopPanel_MouseMove;

            processModule = Process.GetCurrentProcess().MainModule;
            dir = Path.GetDirectoryName(processModule.FileName);
            redistDir = Path.Combine(dir, "REDISTS");

            webClient.DownloadProgressChanged += OnWebClientOnDownloadProgressChanged;
            webClient.DownloadFileCompleted += OnWebClientOnDownloadFileCompleted;
        }

        private void OnWebClientOnDownloadProgressChanged(object ov, DownloadProgressChangedEventArgs ev)
        {
            this.progressBar.Value = ev.ProgressPercentage;
            lbDownloadProgress.Text = $"{String.Format("{0:0.##}", (Convert.ToDouble(ev.BytesReceived) / (1024d * 1024d)))}/{String.Format("{0:0.##}", (Convert.ToDouble(ev.TotalBytesToReceive) / (1024d * 1024d)))}MB";
        }

        private async void OnWebClientOnDownloadFileCompleted(object ov, AsyncCompletedEventArgs ev)
        {
            var d = (Dependency)ev.UserState;
            lbStatus.Text = $"Installing {d.Name}";
            this.Refresh(); //Force this
            await Task.Run(() =>
            {
                while (d != null)
                {
                    ProcessStartInfo p = new ProcessStartInfo(d.ExecutableName, d.InstallString);
                    try
                    {
                        Process _p = Process.Start(p);
                        _p.WaitForExit();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Something went wrong when launching {p}.\n" +
                                        $"If the RTC fails to run, try running the pre-requisite installer from the launcher manually.\n" +
                                        $"If this error persists, poke the devs.");
                        Console.WriteLine(e);
                        Console.WriteLine(e.StackTrace);

                        return;
                    }

                    d = d.RunAfter;
                }
            });

            if (this.InvokeRequired)
                this.Invoke((MethodInvoker) (() => DownloadNext()));
            else
                DownloadNext();
        }

        private void PnTopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32.ReleaseCapture();
                Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HT_CAPTION, 0);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.RunCheck();
        }


        private void RunCheck()
        {
            Directory.CreateDirectory(redistDir);

            var d3dx9Setup = new Dependency("DirectX9 End-User Runtime", "", "", $"/silent", Path.Combine(redistDir, "DXSETUP.exe"));
            var d3dx9 = new Dependency("DirectX9 End-User Runtime", "d3dx9_43.dll", "https://download.microsoft.com/download/8/4/A/84A35BF1-DAFE-4AE8-82AF-AD2AE20B6B14/directx_Jun2010_redist.exe", $"/Q /T:{redistDir}", "", d3dx9Setup);

            var vc2019 = new Dependency("Visual C++ 2019 x64", "vcruntime140_1.dll", "https://aka.ms/vs/16/release/vc_redist.x64.exe",
                "/install /passive /norestart");
           
            var vc2015x86 = new Dependency("Visual C++ 2015-2019 x86", "msvcp140.dll", "https://aka.ms/vs/16/release/vc_redist.x86.exe",
                "/install /passive /norestart");

            var vc2013 = new Dependency("Visual C++ 2013 x64", "msvcr120.dll", "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe", "/install /passive /norestart");
            var vc2012 = new Dependency("Visual C++ 2012 x64", "msvcr110.dll", "https://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe", "/install /passive /norestart");
            var vc2010 = new Dependency("Visual C++ 2010 x64", "msvcr100.dll", "https://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe", "/passive /norestart");
            var dotNet471 = new Dependency(".Net Framework 4.7.1", "471", "https://download.visualstudio.microsoft.com/download/pr/014120d7-d689-4305-befd-3cb711108212/0fd66638cde16859462a6243a4629a50/ndp48-x86-x64-allos-enu.exe", "/install /x86 /x64 /passive /norestart");

            if (Environment.Is64BitProcess)
            {
                if (Win32.LoadLibrary(d3dx9.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(d3dx9); //Bizhawk

                if (Win32.LoadLibrary(vc2019.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(vc2019); //PCSX2, MelonDS, Dolphin

                if (Win32.LoadLibrary(vc2013.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(vc2013); //Bizhawk

                if (Win32.LoadLibrary(vc2012.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(vc2012); //Bizhawk

                if (Win32.LoadLibrary(vc2010.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(vc2010); //Bizhawk and RTC (SlimDX)

                if (GetDotNetVersion() < 471)
                    downloadQueue.Enqueue(dotNet471);
            }
            else
            {
                if (Win32.LoadLibrary(vc2015x86.CheckFile) == IntPtr.Zero)
                    downloadQueue.Enqueue(vc2015x86); //VC++2015 x86 for PCSX2
            }


            if (downloadQueue.Count > 0 && !skipPrompt )
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("You are missing the following dependencies: ");
                foreach (var d in downloadQueue)
                    sb.AppendLine(d.Name);
                sb.AppendLine("");
                sb.AppendLine("Would you like to download and install them?");
                if (MessageBox.Show(sb.ToString(), "Missing dependencies", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    if (MessageBox.Show("The RTC may not work properly without these dependencies.\nYou can always run this tool again via the RTC Launcher", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != null) ;
                    {
                        Environment.Exit(0);
                    };
                }
                if (!IsAdministrator())
                {

                    // Restart program and run as admin
                    var exeName = Process.GetCurrentProcess().MainModule.FileName;
                    var startInfo = new ProcessStartInfo(exeName, "-SKIPPROMPT");
                    startInfo.Verb = "runas";
                    try
                    {
                        Process.Start(startInfo).WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Admin permissions are required to continue.\nThe RTC may not run properly until you re-run this from the launcher.");
                    }

                    Application.Exit();
                }
                this.Show();
            }
            DownloadNext();
        }


        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static int GetDotNetVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null) return CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
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

        private void DownloadNext()
        {
            if (downloadQueue.Count > 0)
            {
                var d = downloadQueue.Dequeue();
                d.ExecutableName = Path.Combine(redistDir, d.Name + ".exe");
                lbStatus.Text = $"Downloading {d.Name}...";
                if (File.Exists(d.ExecutableName))
                    File.Delete(d.ExecutableName);
                webClient.DownloadFileAsync(new Uri(d.DownloadLink), d.ExecutableName, d);
            }
            else
            {
                //Daisy chain x86 to x64
                if (!Environment.Is64BitProcess)
                {
                    var name = Process.GetCurrentProcess().ProcessName + "64.exe";
                    //var args = String.Join(" ", Environment.GetCommandLineArgs()); 
                    if (processModule != null)
                    {
                        var fileName = Path.Combine(dir, name);
                        if (File.Exists(fileName))
                        {
                            this.Hide();
                            try
                            {
                                var p = new ProcessStartInfo(fileName);
                                Process.Start(p)?.WaitForExit();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Something went wrong when launching the 64-bit installer. Send this error to the devs.\n" + ex);
                            }
                        }
                    }
                }

                if (Environment.Is64BitProcess)
                {
                    try
                    {
                        Thread.Sleep(300); //Hope the processes have actually released their resources
                        Directory.Delete(redistDir, true);
                    }
                    catch { } //eat it
                }
                lbStatus.Text = "Done";
                this.Refresh(); //Force this
                //Daisy chain x86 to x64
                if (Environment.Is64BitProcess && Environment.GetCommandLineArgs().Contains("-NOTIFY"))
                    MessageBox.Show("All prerequisites satisfied.");
                Application.Exit();

            }
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnQuit_MouseEnter(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(230, 46, 76);
        }

        private void btnQuit_MouseLeave(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(64, 64, 64);
        }

        private static class Win32
        {
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HT_CAPTION = 0x2;
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImportAttribute("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [DllImportAttribute("user32.dll")]
            public static extern bool ReleaseCapture();
        }

        private class Dependency
        {
            public Dependency(string name, string checkFile, string downloadLink, string installString, string executableName = null, Dependency runAfter = null)
            {
                Name = name;
                CheckFile = checkFile;
                DownloadLink = downloadLink;
                InstallString = installString;
                RunAfter = runAfter;
                ExecutableName = executableName;
            }

            public string Name { get; }
            public string CheckFile { get; }
            public string DownloadLink { get; }
            public string InstallString { get; }
            public Dependency RunAfter { get; }
            public String ExecutableName { get; set; }
        }

    }
}
