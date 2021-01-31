namespace RTCV.UI
{
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    public partial class NoToolShortcuts : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public NoToolShortcuts()
        {
            InitializeComponent();

            popoutAllowed = false;
        }

        private void btnNavigateToMyLists_Click(object sender, System.EventArgs e)
        {
            SettingsForm settingsform = S.GET<SettingsForm>();
            DefaultGrids.settings.LoadToMain();
            settingsform.SwitchToComponentForm(S.GET<MyListsForm>());
        }

        private void btnNavigateToMyVMDs_Click(object sender, System.EventArgs e)
        {
            SettingsForm settingsform = S.GET<SettingsForm>();
            DefaultGrids.settings.LoadToMain();
            settingsform.SwitchToComponentForm(S.GET<MyVMDsForm>());
        }

        private void btnNavigateToMyPlugins_Click(object sender, System.EventArgs e)
        {
            SettingsForm settingsform = S.GET<SettingsForm>();
            DefaultGrids.settings.LoadToMain();
            settingsform.SwitchToComponentForm(S.GET<MyPluginsForm>());
        }

        private void btnOpenPackageDownloader_Click(object sender, System.EventArgs e)
        {
            string exeFile = Path.Combine(RtcCore.LauncherDir, "PackageDownloader.exe");
            var psi = new ProcessStartInfo();
            psi.FileName = exeFile;
            psi.WorkingDirectory = RtcCore.LauncherDir;
            Process.Start(psi);
        }

        private void btnPrepareGlitchHarvester_MouseDown(object sender, MouseEventArgs e)
        {
            S.GET<CoreForm>().OpenGlitchHarvester(null, null);
            S.GET<SavestateManagerForm>().savestateList.NewSavestateNow();
        }

        private void lbDragAndDropGH_DragDrop(object sender, DragEventArgs e)
        {


            var formats = e.Data.GetFormats();
            e.Effect = DragDropEffects.Move;

            string[] fd = (string[])e.Data.GetData(DataFormats.FileDrop); //file drop

            foreach (var file in fd)
            {
                var fi = new FileInfo(file);
                switch (fi.Extension.ToUpper())
                {
                    case ".SKS":
                        {
                            S.GET<CoreForm>().OpenGlitchHarvester(null, null);
                            var sm = S.GET<StockpileManagerForm>();
                            sm.LoadStockpile(file);
                        }
                        break;
                    case ".SSK":
                        {
                            S.GET<CoreForm>().OpenGlitchHarvester(null, null);
                            var sm = S.GET<SavestateManagerForm>();
                            sm.loadSavestateList(false, file);
                        }
                        break;
                }
            }

        }

        private void lbDragAndDropGH_DragEnter(object sender, DragEventArgs e)
        {

                e.Effect = DragDropEffects.Move;

        }
    }
}
