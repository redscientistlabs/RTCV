using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RTCV.UI.Modular
{
#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class ComponentFormTile : ColorizedForm, ITileForm
    {
        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, UIntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const uint WM_SYSCOMMAND = 0x112;
        private const uint MOUSE_MOVE = 0xF012;
        
        private const int DragPopoutThreshold = 15;
        
        public Form ChildForm { get; private set; } = null;
        private int _sizeX = 2;
        private int _sizeY = 2;

        public ComponentFormTile()
        {
            this.InitializeComponent();
        }

        public void SetComponentForm(Form childForm, int sizeX, int sizeY, bool displayHeader)
        {
            this.ChildForm = childForm;
            this._sizeX = sizeX;
            this._sizeY = sizeY;

            this.Size = new Size(
                (this._sizeX * CanvasForm.tileSize) + ((this._sizeX - 1) * CanvasForm.spacerSize),
                (this._sizeY * CanvasForm.tileSize) + ((this._sizeY - 1) * CanvasForm.spacerSize));

            if (this.ChildForm is ComponentForm cf)
            {
                cf.AnchorToPanel(this.pnComponentFormHost);
                cf.ParentComponentFormTitle = this;
            }

            if (displayHeader)
            {
                this.lbComponentFormName.Text = this.ChildForm.Text; // replace that with the childform's text property
            }
            else
            {
                this.lbComponentFormName.Visible = false;
                this.pnComponentFormHost.Location = new Point(0, 0);
                this.pnComponentFormHost.Size = this.Size;
                this.ChildForm.Size = this.Size;
            }
        }

        public bool CanPopout { get; set; } = false;

        public int TilesX
        {
            get => this._sizeX; set => this._sizeX = value;
        }
        public int TilesY
        {
            get => this._sizeY; set => this._sizeY = value;
        }
        private Point _mouseDownAt = new Point(int.MinValue, int.MinValue);
        
        private void OnFormTileMouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y - this.pnComponentFormHost.Location.Y);
            var ea = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
            (this.ChildForm as ComponentForm)?.HandleMouseDown(this.ChildForm, ea);

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            this._mouseDownAt = new Point(e.X, e.Y);
            
            if (!((ComponentForm)this.ChildForm).PopoutAllowed)
            {
                ((Control)sender).Cursor = Cursors.No;
            }
        }

        private void OnFormTileMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !((ComponentForm)this.ChildForm).PopoutAllowed)
            {
                ((Control)sender).Cursor = Cursors.Default;
            }
        }

        private void OnFormTileMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || this._mouseDownAt.X == int.MinValue || this._mouseDownAt.Y == int.MinValue)
            {
                return;
            }

            var cf = (ComponentForm)this.ChildForm;
            if (!cf.PopoutAllowed)
            {
                return;
            }

            Point p = new Point(Math.Abs(e.X - this._mouseDownAt.X), Math.Abs(e.Y - this._mouseDownAt.Y));
            bool stillDocked = this.pnComponentFormHost.Controls.Contains(this.ChildForm);
            if (p.X < DragPopoutThreshold && p.Y < DragPopoutThreshold && stillDocked)
            {
                return;
            }

            if (stillDocked)
            {
                cf.SwitchToWindow();
                this.ChildForm.Location = this.PointToScreen(e.Location - new Size(this._mouseDownAt));
                ReleaseCapture();
                DefWindowProc(this.ChildForm.Handle, WM_SYSCOMMAND, (UIntPtr)MOUSE_MOVE, IntPtr.Zero);
            }
        }

        public void ReAnchorToPanel()
        {
            if (this.ChildForm is ComponentForm cf)
            {
                cf.AnchorToPanel(this.pnComponentFormHost);
                cf.Size = this.pnComponentFormHost.Size;
                this.Anchor = cf.Anchor;
            }
        }

        private void lbComponentFormName_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnFormTileMouseMove(sender, e);
        }
    }

    public class ComponentPanel : Panel
    {
    }
}
