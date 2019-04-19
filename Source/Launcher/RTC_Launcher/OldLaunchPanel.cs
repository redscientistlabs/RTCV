using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC_Launcher
{
    public partial class OldLaunchPanel : Form
    {
        public Button[] buttons;

        public OldLaunchPanel()
        {
            InitializeComponent();

            buttons = new Button[]
            {
            btnBatchfile01,
            btnBatchfile02,
            btnBatchfile03,
            btnBatchfile04,
            btnBatchfile05,
            btnBatchfile06,
            btnBatchfile07,
            btnBatchfile08,
            btnBatchfile09,
            btnBatchfile10,
            };

            lbSelectedVersion.Visible = false;
            pnVersionBatchFiles.Visible = false;

            DisplayVersion();

        }

        public void DisplayVersion()
        {
            string version = MainForm.SelectedVersion;

            List<string> batchFiles = new List<string>(Directory.GetFiles(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version));
            List<string> batchFileNames = new List<string>(batchFiles.Select(it => MainForm.mf.removeExtension(MainForm.mf.getFilenameFromFullFilename(it))));

            bool isDefaultStartPresent = false;

            if (batchFileNames.Contains("START"))
            {
                batchFileNames.Remove("START");
                isDefaultStartPresent = true;
            }

            string startfilename = null;

            foreach (string file in batchFiles)
                if (file.ToUpper().Contains(Path.DirectorySeparatorChar + "START.BAT"))
                {
                    startfilename = file;
                    break;
                }

            if (startfilename != null)
                batchFiles.Remove(startfilename);

            btnStart.Visible = isDefaultStartPresent;

            foreach (Button btn in buttons)
                btn.Visible = false;

            for (int i = 0; i < batchFileNames.Count; i++)
            {
                buttons[i].Visible = true;
                buttons[i].Text = batchFileNames[i];
                buttons[i].Tag = (buttons[i].Tag as string) + ";" + batchFiles[i];
            }

            lbSelectedVersion.Text = version;
            lbSelectedVersion.Visible = true;
            pnVersionBatchFiles.Visible = true;

            new object();
        }

        private void OldLaunchPanel_Load(object sender, EventArgs e)
        {

        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            //string version = MainForm.mf.lbVersions.SelectedItem.ToString();

            string version = MainForm.SelectedVersion;
            string fullPath;

            if (currentButton.Text == "START")
                fullPath = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "START.bat";
            else
                fullPath = (currentButton.Tag as string).Split(';')[1];

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.GetFileName(fullPath);
            psi.WorkingDirectory = Path.GetDirectoryName(fullPath);
            Process.Start(psi);
        }
    }
}
