namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.UI.Extensions;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class ShadowPanel : Form
    {
        private CanvasForm parentForm;
        public Form subForm { get; set; } = null;

        public ShadowPanel(CanvasForm _parentForm, ISubForm reqForm)
        {
            InitializeComponent();
            var blockerForm = new Form
            {
                ControlBox = false,
                MinimizeBox = false,
                FormBorderStyle = FormBorderStyle.None,
                Text = "",
                Size = Size,
                BackColor = Color.DarkSlateBlue,
                Opacity = 0.2f
            };

            Colors.SetRTCColor(Colors.GeneralColor, this);

            parentForm = _parentForm;
            UpdateBackground();

            subForm = (Form)reqForm;

            UpdateSubForm();
        }

        public void OnParentResizeBegin()
        {
            this.BackgroundImage = null;
            pnFloater.Visible = false;
        }

        public void OnParentResizeEnd()
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
                btnLeft.Visible = sf.HasLeftButton;
                btnRight.Visible = sf.HasRightButton;

                if (!sf.HasLeftButton && !sf.HasRightButton)
                {
                    int newYSize = pnFloater.Size.Height - (pnContainer.Location.Y * 2);
                    pnContainer.Size = new Size(pnContainer.Size.Width, newYSize);
                }

                if (sf.LeftButtonText != null)
                {
                    btnLeft.Text = sf.LeftButtonText;
                }

                if (sf.RightButtonText != null)
                {
                    btnRight.Text = sf.RightButtonText;
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
            bmp.Tint(Color.FromArgb(0x7F, Colors.Dark4Color));

            this.Size = parentForm.Size;
            this.BackgroundImage = bmp;

            pnFloater.Location = new Point((parentForm.Width - pnFloater.Width) / 2, (parentForm.Height - pnFloater.Height) / 2);
        }

        private void OnRightButtonClick(object sender, EventArgs e)
        {
            //Fires SubForm_Ok() from Interface SubForm then Exits SubForm Mode

            if (subForm is ISubForm)
            {
                (subForm as ISubForm).RightButtonClick();
            }

            parentForm.CloseSubForm();
        }

        private void OnLeftButtonClick(object sender, EventArgs e)
        {
            //Fires SubForm_Cancel() from Interface SubForm then Exits SubForm Mode

            if (subForm is ISubForm)
            {
                (subForm as ISubForm).LeftButtonClick();
            }

            parentForm.CloseSubForm();
        }
    }
}
