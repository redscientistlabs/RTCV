namespace Package_Downloader
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Windows.Forms;

    public partial class PackageDownloader : Form
    {
        public string RtcVer = "TEST";
        public DirectoryInfo RtcDir = null;
        public PackageDownloader()
        {
            InitializeComponent();

            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());

            if (currentDir.Name == "Launcher")
            {
                RtcVer = currentDir.Parent.Name;
                RtcDir = currentDir.Parent;
            }
            else
            {
                RtcDir = currentDir;
            }

            this.Text += $" : {RtcVer}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private bool bCancel = false;

        private void webBrowser_DocumentCompleted(object sender,
                                         WebBrowserDocumentCompletedEventArgs e)
        {
            int i;
            for (i = 0; i < webBrowser.Document.Links.Count; i++)
            {
                webBrowser.Document.Links[i].Click += new
                                       HtmlElementEventHandler(this.LinkClick);
            }
        }
        private void LinkClick(object sender, HtmlElementEventArgs e)
        {
            var link = (sender as System.Windows.Forms.HtmlElement);

            var url = (link.DomElement as mshtml.IHTMLAnchorElement)?.href;

            if (!string.IsNullOrWhiteSpace(url) && url.EndsWith(".pkg"))
            {
                bCancel = true;

                string urlFilename = url.Substring(url.LastIndexOf('/') + 1);
                var result = MessageBox.Show($"Do you want to install package {urlFilename} to your {RtcVer} install?", "Install Package?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string versionFolder = RtcDir.FullName;
                    string pkgPath = Path.Combine(versionFolder, urlFilename);

                    if (File.Exists(pkgPath))
                        File.Delete(pkgPath);

                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(url, pkgPath);
                    }


                    try
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(pkgPath))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                var entryPath = Path.Combine(versionFolder, entry.FullName).Replace("/", "\\");

                                if (entryPath.EndsWith("\\"))
                                {
                                    if (!Directory.Exists(entryPath))
                                        Directory.CreateDirectory(entryPath);
                                }
                                else
                                {
                                    entry.ExtractToFile(entryPath, true);
                                }
                            }
                        }

                        MessageBox.Show($"Package {urlFilename} successfully installed");

                        //System.IO.Compression.ZipFile.ExtractToDirectory(file, versionFolder,);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred during extraction and your RTC installation is possibly corrupted. \n\nYou may need to delete your RTC installation and reinstall it from the launcher. To do so, you can right click the version on the left side panel and select Delete from the menu.\n\nIf you need to backup any downloaded emulator to keep configurations or particular setups, you will find the content to backup by right clicking the card and selecting Open Folder.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    finally
                    {
                        if (File.Exists(pkgPath))
                            File.Delete(pkgPath);
                    }
                }
            }
        }
        private void webBrowser_Navingating(object sender,
                                        WebBrowserNavigatingEventArgs e)
        {
            if (bCancel == true)
            {
                e.Cancel = true;
                bCancel = false;
            }
        }
    }
}
