namespace RTCV.NetCore
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.Common;

    public partial class CloudDebug : Form
    {
        private Exception ex;

        public CloudDebug(Exception _ex, bool canContinue = false)
        {
            InitializeComponent();
            ex = _ex;
            if (ex is AbortEverythingException)
            {
                return;
            }

            if (!(ex is OperationAbortedException))
            {
                lbException.Text = ex.Message;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"{ex.Message}\n{ex.StackTrace}");
                var e = _ex;
                while (e.InnerException != null)
                {
                    sb.AppendLine();
                    sb.AppendLine($"Inner Exception: {ex.Message}\n{ex.StackTrace}");
                    e = e.InnerException;
                }
                tbStackTrace.Text = sb.ToString();

                btnContinue.Visible = canContinue;
                btnContinue.Visible = true;
            }
            this.Shown += CloudDebug_Shown;
        }

        private void CloudDebug_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        public DialogResult Start()
        {
            if (ex is OperationAbortedException)
            {
                return DialogResult.Abort;
            }
            else
            {
                return this.ShowDialog();
            }
        }

        public static string getRTCInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Spec Dump from UICore");
            sb.AppendLine();
            sb.AppendLine("UISpec");
            RTCV.NetCore.AllSpec.UISpec?.GetDump().ForEach(x => sb.AppendLine(x));
            sb.AppendLine("CorruptCoreSpec");
            RTCV.NetCore.AllSpec.CorruptCoreSpec?.GetDump().ForEach(x => sb.AppendLine(x));
            sb.AppendLine("VanguardSpec");
            RTCV.NetCore.AllSpec.VanguardSpec?.GetDump().ForEach(x => sb.AppendLine(x));

            return sb.ToString();
        }

        public static string getEmuInfo()
        {
            string str = LocalNetCoreRouter.QueryRoute<string>(NetcoreCommands.CORRUPTCORE, "GETSPECDUMPS");
            if (str != null)
            {
                return str;
            }
            else
            {
                return "GETSPECDUMPS returned null!";
            }
        }

        private void btnSendDebug_Click(object sender, EventArgs e)
        {
            string szdll = "";
            if (Environment.Is64BitProcess)
                szdll = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7z.dll");
            else
                szdll = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7z32.dll");


            if (btnSendDebug.Text != "Fetch data") //If not in receive mode
            {
                string password = Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "").ToUpper();

                string relativedir = Directory.GetCurrentDirectory();
                string tempdebugdir = Path.Combine(relativedir, "debug");
                string tempzipfile = tempdebugdir + ".7z";

                if (!Directory.Exists(tempdebugdir))
                {
                    Directory.CreateDirectory(tempdebugdir);
                }

                string[] debugFiles = Directory.GetFiles(tempdebugdir);
                foreach (string file in debugFiles)
                {
                    File.Delete(file);
                }

                if (File.Exists(tempzipfile))
                {
                    File.Delete(tempzipfile);
                }

                //Exporting side
                string sideFile = Path.Combine(tempdebugdir, "SIDE.txt");
                File.WriteAllText(sideFile, System.Diagnostics.Process.GetCurrentProcess().ProcessName);

                //Exporting Stacktrace
                var _ex = ex;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Exception: {ex.Message}\n{ex.StackTrace}");
                while (_ex.InnerException != null)
                {
                    sb.AppendLine();
                    sb.AppendLine($"Inner Exception: {ex.Message}\n{ex.StackTrace}");
                    _ex = _ex.InnerException;
                }
                tbStackTrace.Text = sb.ToString();
                string stacktracefile = Path.Combine(tempdebugdir, "STACKTRACE.TXT");
                File.WriteAllText(stacktracefile, sb.ToString());

                //Exporting data
                string data = Path.Combine(tempdebugdir, "DATA.TXT");
                sb = new StringBuilder();
                foreach (var key in ex.Data.Keys)
                {
                    sb.AppendLine(key + " : " + ex.Data[key]);
                }
                File.WriteAllText(data, sb.ToString());

                //Exporting Specs from RTC's perspective
                string rtcfile = Path.Combine(tempdebugdir, "RTC_PERSPECTIVE.TXT");
                File.WriteAllText(rtcfile, getRTCInfo());

                //Exporting Specs from the Emu's perspective
                string emufile = Path.Combine(tempdebugdir, "EMU_PERSPECTIVE.txt");
                File.WriteAllText(emufile, getEmuInfo());

                //Copying the log files
                if (AllSpec.CorruptCoreSpec?["RTCDIR"] is string rtcdir)
                {
                    string rtcLog = Path.Combine(rtcdir, "RTC_LOG.txt");
                    string rtcLogOutput = Path.Combine(tempdebugdir, "RTC_LOG.txt");
                    //lock (NetCore_Extensions.ConsoleHelper.con.FileWriter)
                    //{
                    if (File.Exists(rtcLog))
                    {
                        File.Copy(rtcLog, rtcLogOutput, true);
                    }
                    //}
                }

                if (AllSpec.VanguardSpec?["EMUDIR"] is string emudir)
                {
                    string emuLog = Path.Combine(emudir, "EMU_LOG.txt");
                    string emuLogOutput = Path.Combine(tempdebugdir, "EMU_LOG.txt");
                    //lock (NetCore_Extensions.ConsoleHelper.con.FileWriter)
                    //{
                    if (File.Exists(emuLog))
                    {
                        File.Copy(emuLog, emuLogOutput, true);
                    }
                    //}
                }

                SevenZip.SevenZipBase.SetLibraryPath(szdll);
                var comp = new SevenZip.SevenZipCompressor
                {
                    CompressionMode = SevenZip.CompressionMode.Create,
                    TempFolderPath = Path.GetTempPath(),
                    ArchiveFormat = SevenZip.OutArchiveFormat.SevenZip
                };
                comp.CompressDirectory(tempdebugdir, tempzipfile, false, password);

                string filename = CloudTransfer.CloudSave(tempzipfile);
                tbKey.Text = filename + "-" + password;

                btnSendDebug.Enabled = false;

                if (File.Exists(tempzipfile))
                {
                    File.Delete(tempzipfile);
                }
            }
            else
            {
                if (tbKey.Text.Length != 65)
                {
                    MessageBox.Show("Invalid key");
                    return;
                }

                string[] keyparts = tbKey.Text.Split('-');
                string filename = keyparts[0];
                string password = keyparts[1];

                string downloadfilepath = CloudTransfer.CloudLoad(filename, password);

                if (downloadfilepath == null)
                {
                    return;
                }

                string extractpath = downloadfilepath.Replace(".7z", "");

                if (!Directory.Exists(extractpath))
                {
                    Directory.CreateDirectory(extractpath);
                }

                string[] debugFiles = Directory.GetFiles(extractpath);
                foreach (string file in debugFiles)
                {
                    File.Delete(file);
                }


                SevenZip.SevenZipCompressor.SetLibraryPath(szdll);
                var decomp = new SevenZip.SevenZipExtractor(downloadfilepath, password, SevenZip.InArchiveFormat.SevenZip);
                decomp.ExtractArchive(extractpath);

                File.Delete(downloadfilepath);
                Process.Start(extractpath);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            Close();
        }

        public static DialogResult ShowErrorDialog(Exception ex, bool canContinue = false) => new RTCV.NetCore.CloudDebug(ex, canContinue).Start();

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSendDebug_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Control c = (Control)sender;
                Point locate = new Point(c.Location.X + e.Location.X + c.Parent.Location.X, ((Control)sender).Location.Y + e.Location.Y + c.Parent.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                if (lbException.Text.Contains("SECRET"))
                {
                    if (btnSendDebug.Text == "Send debug info to devs")
                    {
                        columnsMenu.Items.Add("Switch to retreive mode", null, new EventHandler((ob, ev) =>
                        {
                            tbKey.ReadOnly = false;
                            btnSendDebug.Text = "Fetch data";
                            RTCV.NetCore.Params.SetParam("DEBUG_FETCHMODE");
                        }));
                    }
                    else
                    {
                        columnsMenu.Items.Add("Switch to send mode", null, new EventHandler((ob, ev) =>
                        {
                            tbKey.ReadOnly = false;
                            btnSendDebug.Text = "Send debug info to devs";
                            RTCV.NetCore.Params.RemoveParam("DEBUG_FETCHMODE");
                        }));
                    }
                }
                columnsMenu.Show(this, locate);
            }
        }

        private void CloudDebug_Load(object sender, EventArgs e)
        {
            if (RTCV.NetCore.Params.IsParamSet("DEBUG_FETCHMODE"))
            {
                btnSendDebug.Text = "Fetch data";
                tbKey.ReadOnly = false;
            }
        }

        private void btnDebugInfo_Click(object sender, EventArgs e) => S.GET<DebugInfo_Form>().ShowDialog();
    }

    [Serializable]
    public class AbortEverythingException : Exception
    {
        public AbortEverythingException() : base()
        {
        }

        public AbortEverythingException(string message) : base(message)
        {
        }

        public AbortEverythingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbortEverythingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    internal class WebClientTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 5000;
            return w;
        }
    }

    internal static class CloudTransfer
    {
        private static string CorruptCloudServer = "http://cc.r5x.cc/rtc/debug";

        public static string CloudLoad(string filename, string password)
        {
            string remoteUri = CorruptCloudServer + "/FILES/";

            WebClientTimeout myWebClient = new WebClientTimeout();

            string downloadfilepath;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "7z",
                Title = "Browse path for 7z File",
                Filter = "7z files|*.7z",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                downloadfilepath = saveFileDialog1.FileName;
            }
            else
            {
                return null;
            }

            if (File.Exists(downloadfilepath))
            {
                File.Delete(downloadfilepath);
            }

            try
            {
                myWebClient.DownloadFile(remoteUri + filename, downloadfilepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Couldn't download requested Debug file\n\n\n" + ex.ToString());
                return null;
            }

            return downloadfilepath;
        }

        public static string CloudSave(string filepath)
        {
            WebRequest.DefaultWebProxy = null;

            string remoteUri = CorruptCloudServer + "/post.php?submit=true&action=upload";
            byte[] responseBinary;
            try
            {
                WebClientTimeout client = new WebClientTimeout();
                responseBinary = client.UploadFile(remoteUri, "POST", filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong with the upload. Try again. \n\n\n" + ex.ToString());
                return "";
            }

            string response = Encoding.UTF8.GetString(responseBinary);

            if (response == "ERROR")
            {
                return "";
            }
            else
            {
                return response;
            }
        }

        // String serializers
        public static string SerializeObject(object o)
        {
            if (!o.GetType().IsSerializable)
            {
                return "";
            }

            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static object DeserializeObject(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}
