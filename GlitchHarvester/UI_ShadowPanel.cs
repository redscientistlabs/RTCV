using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI
{
    public partial class UI_ShadowPanel : Form
    {
        public static UI_CanvasForm parentForm;
        public static Form subForm = null;

        public UI_ShadowPanel(UI_CanvasForm _parentForm, string _type)
        {
            InitializeComponent();

            parentForm = _parentForm;
            UpdateBackground();

            if (_type != null)
            {
                var subFormType = Type.GetType("RTCV.UI." + _type); //remove that and replace with componentform loader
                subForm = (Form)Activator.CreateInstance(subFormType);
            }
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
                return;

            if(!pnContainer.Contains(subForm))
            {
                subForm.TopLevel = false;
                pnContainer.Controls.Add(subForm);
                subForm.Show();
            }

            if(subForm is ISubForm)
                    btnCancel.Visible = (subForm as ISubForm).HasCancelButton;

        }

        public void UpdateBackground()
        {
            //Creates darkened blured background for the subform
            //If updates from resizing, will replace background for a black square (maybe fix later)

            //Then, repositions flating box in the center of the window.

            if(parentForm.Width == 0 || parentForm.Height == 0)
                return;

            var bitmap = new Bitmap(parentForm.Width, parentForm.Height);
            var rectSize = new Rectangle(0, 0, parentForm.Width, parentForm.Height);
            parentForm.DrawToBitmap(bitmap, rectSize);

            Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width / 4, bitmap.Height / 4));

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(resized, rectSize);

                if (subForm == null)
                {
                    SolidBrush darkBrush = new SolidBrush(Color.FromArgb(96, Color.Black));
                    g.FillRectangle(darkBrush, rectSize);

                    Pen pn = new Pen(Color.Black, 1);
                    for (int y = 0; y < rectSize.Height / 2; y++)
                        g.DrawLine(pn, new Point(0, y * 2), new Point(rectSize.Width, y * 2));
                }
                else
                {
                    SolidBrush darkBrush = new SolidBrush(Color.Black);
                    g.FillRectangle(darkBrush, rectSize);
                }

            }

            this.Size = parentForm.Size;
            this.BackgroundImage = bitmap;
            pnFloater.Location = new Point((parentForm.Width - pnFloater.Width) / 2, (parentForm.Height - pnFloater.Height) / 2);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Ok() from Interface ISubForm then Exits SubForm Mode

            if (subForm is ISubForm)
                (subForm as ISubForm).SubForm_Ok();

            parentForm.CloseSubForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Cancel() from Interface ISubForm then Exits SubForm Mode

            if (subForm is ISubForm)
                (subForm as ISubForm).SubForm_Cancel();

            parentForm.CloseSubForm();
        }
    }
}
