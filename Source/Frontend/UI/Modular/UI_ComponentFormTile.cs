namespace RTCV.UI
{
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class UI_ComponentFormTile : Form, ITileForm
    {
        public Form childForm = null;
        public int SizeX = 2;
        public int SizeY = 2;

        public UI_ComponentFormTile()
        {
            InitializeComponent();
            UICore.SetRTCColor(UICore.GeneralColor, this);
        }

        public void SetCompoentForm(Form _childForm, int _sizeX, int _sizeY, bool DisplayHeader)
        {
            childForm = _childForm;
            SizeX = _sizeX;
            SizeY = _sizeY;

            this.Size = new Size(
                (SizeX * UI_CanvasForm.tileSize) + ((SizeX - 1) * UI_CanvasForm.spacerSize),
                (SizeY * UI_CanvasForm.tileSize) + ((SizeY - 1) * UI_CanvasForm.spacerSize)
                );

            if (childForm is ComponentForm cf)
            {
                cf.AnchorToPanel(pnComponentFormHost);
                cf.ParentComponentFormTitle = this;
            }

            if (DisplayHeader)
            {
                lbComponentFormName.Text = childForm.Text; // replace that with the childform's text property
            }
            else
            {
                lbComponentFormName.Visible = false;
                pnComponentFormHost.Location = new Point(0, 0);
                pnComponentFormHost.Size = this.Size;
                childForm.Size = this.Size;
            }
        }

        public bool CanPopout { get; set; } = false;
        public int TilesX { get => SizeX; set => SizeX = value; }
        public int TilesY { get => SizeY; set => SizeY = value; }

        private void UI_ComponentFormTile_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.Location.X, e.Location.Y - pnComponentFormHost.Location.Y);
            var ea = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
            (childForm as ComponentForm)?.HandleMouseDown(childForm, ea);
        }

        public void ReAnchorToPanel()
        {
            var cf = (childForm as ComponentForm);

            if (cf != null)
            {
                cf.AnchorToPanel(pnComponentFormHost);
                cf.Size = pnComponentFormHost.Size;
                this.Anchor = cf.Anchor;
            }
        }
    }

    public class ComponentPanel : Panel
    {
    }
}
