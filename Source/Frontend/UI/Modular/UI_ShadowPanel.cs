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

            UICore.SetRTCColor(UICore.GeneralColor, this);

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

        public Bitmap FormScreenShot(Form f)
        {
            //recursively creates a screenshot of the form and subforms

            var bitmap = new Bitmap(f.Width, f.Height);
            var rectSize = new Rectangle(0, 0, f.Width, f.Height);
            f.DrawToBitmap(bitmap, rectSize);

            foreach(Control c in f.Controls)
            {
                if(c is Form)
                {
                    var cf = (Form)c;
                    var subBitmap = FormScreenShot(cf);
                    var subrect = new Rectangle(0, 0, cf.Width, cf.Height);
                    //cf.DrawToBitmap(subBitmap, subrect);
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(subBitmap, new Point(cf.Location.X, cf.Location.Y));
                    }
                }

                if (c is Panel)
                {
                    var p = (Panel)c;
                    foreach (Control cp in p.Controls)
                    {
                        if(cp is Form)
                        {
                            var subBitmap = FormScreenShot((Form)cp);
                            var subrect = new Rectangle(0, 0, cp.Width, cp.Height);
                            //cp.DrawToBitmap(subBitmap, subrect);
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.DrawImage(subBitmap, new Point(p.Location.X + cp.Location.X, p.Location.Y + cp.Location.Y));
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        public void UpdateBackground()
        {
            //Creates darkened blured background for the subform
            //If updates from resizing, will replace background for a black square (maybe fix later)

            //Then, repositions flating box in the center of the window.

            if(parentForm.Width == 0 || parentForm.Height == 0)
                return;

            var bitmap = FormScreenShot(parentForm);
            var rectSize = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

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

        private void UI_ShadowPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
