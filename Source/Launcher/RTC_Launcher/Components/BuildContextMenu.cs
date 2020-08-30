namespace RTCV.Launcher.Components
{
    using System.Drawing;
    using System.Windows.Forms;

    internal class BuildContextMenuColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected {
            get { return Color.DodgerBlue; }
        }
    }

    internal class BuildContextMenuRenderer : ToolStripProfessionalRenderer
    {
        public BuildContextMenuRenderer() : base(new BuildContextMenuColors())
        {
        }
    }

    public class BuildContextMenu : ContextMenuStrip
    {
        public BuildContextMenu() : base()
        {
            ShowImageMargin = false;
            ForeColor = Color.FromArgb(235, 235, 235);
            BackColor = Color.FromArgb(30, 30, 30);
            Renderer = new BuildContextMenuRenderer();
        }
    }
}
