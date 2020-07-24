namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class UI_ShadowPanel : Form
    {
        public UI_CanvasForm parentForm;
        public Form subForm = null;
        public Form blockerForm = null;

        public UI_ShadowPanel(UI_CanvasForm _parentForm, ISubForm reqForm)
        {
            InitializeComponent();
            blockerForm = new Form
            {
                ControlBox = false,
                MinimizeBox = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                Text = "",
                Size = Size,
                BackColor = Color.DarkSlateBlue,
                Opacity = 0.2f
            };

            UICore.SetRTCColor(UICore.GeneralColor, this);

            parentForm = _parentForm;
            UpdateBackground();

            subForm = (Form)reqForm;

            UpdateSubForm();
        }

        public void Parent_ResizeBegin()
        {
            this.BackgroundImage = null;
            pnFloater.Visible = false;
        }

        public void Parent_ResizeEnd()
        {
            UpdateBackground();
            pnFloater.Visible = true;
        }

        public void UpdateSubForm()
        {
            //Adds SubForm item to the floating panel
            //Makes Cancel Button appear if needed

            if (subForm == null)
            {
                return;
            }

            if (!pnContainer.Contains(subForm))
            {
                subForm.TopLevel = false;
                pnContainer.Controls.Add(subForm);
                subForm.Show();
                ((ISubForm)subForm).OnShown();
            }

            if (subForm is ISubForm sf)
            {
                btnLeft.Visible = sf.SubForm_HasLeftButton;
                btnRight.Visible = sf.SubForm_HasRightButton;

                if (!sf.SubForm_HasLeftButton && !sf.SubForm_HasRightButton)
                {
                    int newYSize = pnFloater.Size.Height - (pnContainer.Location.Y * 2);
                    pnContainer.Size = new Size(pnContainer.Size.Width, newYSize);
                }

                if (sf.SubForm_LeftButtonText != null)
                {
                    btnLeft.Text = sf.SubForm_LeftButtonText;
                }

                if (sf.SubForm_RightButtonText != null)
                {
                    btnRight.Text = sf.SubForm_RightButtonText;
                }
            }
        }

        public void UpdateBackground()
        {
            //Creates darkened blured background for the subform
            //If updates from resizing, will replace background for a black square (maybe fix later)

            //Then, repositions flating box in the center of the window.

            if (parentForm.Width == 0 || parentForm.Height == 0)
            {
                return;
            }

            Bitmap bmp = parentForm.getFormScreenShot();
            bmp.Tint(Color.FromArgb(0x7F, UICore.Dark4Color));

            this.Size = parentForm.Size;
            this.BackgroundImage = bmp;

            pnFloater.Location = new Point((parentForm.Width - pnFloater.Width) / 2, (parentForm.Height - pnFloater.Height) / 2);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Ok() from Interface SubForm then Exits SubForm Mode

            if (subForm is ISubForm)
            {
                (subForm as ISubForm).SubForm_RightButton_Click();
            }

            parentForm.CloseSubForm();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Cancel() from Interface SubForm then Exits SubForm Mode

            if (subForm is ISubForm)
            {
                (subForm as ISubForm).SubForm_LeftButton_Click();
            }

            parentForm.CloseSubForm();
        }

        private void UI_ShadowPanel_Load(object sender, EventArgs e)
        {
        }
    }
}
