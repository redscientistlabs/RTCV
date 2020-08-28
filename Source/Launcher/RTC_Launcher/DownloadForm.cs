namespace RTCV.Launcher
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;

    public partial class DownloadForm : Form
    {
        WebClient webClient;

        public DownloadForm(string downloadURL, string downloadedFile, string extractDirectory)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            webClient = new WebClient();

            webClient.DownloadProgressChanged += (ov, ev) =>
            {
                MainForm.dForm.progressBar.Value = ev.ProgressPercentage;
                if (ev.BytesReceived == ev.TotalBytesToReceive)
                    lbDownloadProgress.Text = "Extracting files...";
                else
                    lbDownloadProgress.Text = $"{string.Format("{0:0.##}", (Convert.ToDouble(ev.BytesReceived) / (1024d * 1024d)))}/{string.Format("{0:0.##}", (Convert.ToDouble(ev.TotalBytesToReceive) / (1024d * 1024d)))}MB";
            };

            webClient.DownloadFileCompleted += (ov, ev) =>
            {
                MainForm.mf.InvokeUI(() => {
                    lbDownloadProgress.Text = $"Uncompressing files...";
                    MainForm.mf.DownloadComplete(downloadedFile, extractDirectory); });
                MainForm.mf.btnVersionDownloader.Enabled = true;
            };

            if (File.Exists(downloadedFile))
                File.Delete(downloadedFile);

            webClient.DownloadFileAsync(new Uri(downloadURL), downloadedFile);
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
            var loc = new Point((this.Width - pnDownloadBar.Width) / 2, (this.Height - pnDownloadBar.Height) / 2);
            pnDownloadBar.Location = loc;
            pnDownloadBar.Visible = true;
        }
    }
}
