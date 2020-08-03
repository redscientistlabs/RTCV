namespace RTCV.UI.Modular
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class ComponentForm : Form
    {
        private protected static NLog.Logger logger;
        private Panel defaultPanel = null;
        private Panel previousPanel = null;

        public Panel blockPanel { get; set; } = null;

        public bool undockedSizable = true;
        public bool popoutAllowed = true;
        public UI_ComponentFormTile ParentComponentFormTitle = null;

        protected ComponentForm() : base()
        {
            logger = NLog.LogManager.GetLogger(this.GetType().ToString());
        }

        public void AnchorToPanel(Panel pn)
        {
            if (defaultPanel == null)
            {
                defaultPanel = pn;
            }

            previousPanel = pn;

            this.Hide();
            this.Parent?.Controls.Remove(this);

            this.FormBorderStyle = FormBorderStyle.None;

            //Remove ComponentForm from target panel if required
            var componentFormInTargetPanel = (pn?.Controls.Cast<Control>().FirstOrDefault(it => it is ComponentForm) as ComponentForm);
            if (componentFormInTargetPanel != null && componentFormInTargetPanel != this)
            {
                pn.Controls.Remove(componentFormInTargetPanel);
            }

            this.TopLevel = false;
            this.TopMost = false;
            pn.Controls.Add(this);
            Control p = this;
            while (p.Parent != null)
            {
                p = p.Parent;
            }
            if (p is Form _p)
            {
                _p.WindowState = FormWindowState.Normal;
            }

            this.Size = this.Parent.Size;
            this.Location = new Point(0, 0);

            this.Show();
            this.BringToFront();
        }

        public void SwitchToWindow()
        {
            this.Hide();
            this.Parent?.Controls.Remove(this);

            this.TopLevel = true;
            this.TopMost = true;

            if (undockedSizable)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MaximizeBox = true;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
            }

            this.Show();
        }

        public void RestoreToPreviousPanel()
        {
            //We don't care about redocking on quit and depending on what's open, exceptions may occur otherwise
            if (UICore.isClosing)
            {
                return;
            }

            if (defaultPanel == null)
            {
                throw new Exception("Default panel unset");
            }

            Panel targetPanel;

            //We select which panel we want to restore the ComponentForm to
            if (previousPanel?.Parent?.Visible ?? false)
            {
                targetPanel = previousPanel;
            }
            else                            //If the ComponentForm was moved to another panel than the default one
            {
                targetPanel = defaultPanel; //and that panel was hidden, then we move it back to the original panel.
            }

            //Restore the state since we don't want it maximized or minimized
            this.WindowState = FormWindowState.Normal;

            //This searches for a ComponentForm in the target Panel
            var componentFormInTargetPanel = (targetPanel?.Controls.Cast<Control>().FirstOrDefault(it => it is ComponentForm) as ComponentForm);
            if (componentFormInTargetPanel != null && componentFormInTargetPanel != this)
            {
                this.Hide();                //If the target panel hosts another ComponentForm, we won't override it
            }
            else                            //This is most likely going to happen if a VMD ComponentForm was changed to a window,
            {
                AnchorToPanel(targetPanel); //then another VMD tool was selected and that window was closed
            }
        }

        /* Note: Visual studio is so dumb, the designer won't allow to bind an event to a method in the base class
            Just paste the following code at the beginning of the ComponentForm class to fix this stupid shit

        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        ALSO, GroupBox does have an event handler for MouseDown but michaelsoft are too high on crack to let
        us bind something to it in the properties panel. Gotta add it manually in the designer.cs ffs.
        */

        public void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (sender is NumericUpDown || sender is TextBox)
            {
                return;
            }

            while (!(sender is ComponentForm))
            {
                var c = (Control)sender;
                sender = c.Parent;
                e = new MouseEventArgs(e.Button, e.Clicks, e.X + c.Location.X, e.Y + c.Location.Y, e.Delta);
            }

            if (popoutAllowed && e.Button == MouseButtons.Right && (sender as ComponentForm).FormBorderStyle == FormBorderStyle.None)
            {
                var locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
                var columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Detach to window", null, new EventHandler((ob, ev) =>
                {
                    (sender as ComponentForm).SwitchToWindow();
                }));
                columnsMenu.Show(this, locate);
            }
        }

        public void HandleFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.RestoreToPreviousPanel();
                return;
            }
        }

        public sealed override string ToString()
        {
            return Text;
        }
    }
}
