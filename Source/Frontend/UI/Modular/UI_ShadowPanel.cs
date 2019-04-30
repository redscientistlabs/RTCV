using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI
{
    public partial class UI_ShadowPanel : Form
    {
        public UI_CanvasForm parentForm;
        public Form subForm = null;

        public UI_ShadowPanel(UI_CanvasForm _parentForm, Form reqForm)
        {
            InitializeComponent();

            UICore.SetRTCColor(UICore.GeneralColor, this);

            parentForm = _parentForm;
            UpdateBackground();

            subForm = reqForm;

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

            if (subForm is ISubForm sf)
            {
                btnLeft.Visible = sf.SubForm_HasLeftButton;
                btnRight.Visible = sf.SubForm_HasRightButton;

                if(!sf.SubForm_HasLeftButton && !sf.SubForm_HasRightButton)
                {
                    int newYSize = pnFloater.Size.Height - (pnContainer.Location.Y*2);
                    pnContainer.Size = new Size(pnContainer.Size.Width, newYSize);
                }

                if (sf.SubForm_LeftButtonText != null)
                    btnLeft.Text = sf.SubForm_LeftButtonText;

                if (sf.SubForm_RightButtonText != null)
                    btnRight.Text = sf.SubForm_RightButtonText;
            }

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

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(bitmap, rectSize);

                SolidBrush darkBrush = new SolidBrush(Color.FromArgb(0xCC, UICore.Dark4Color));
                g.FillRectangle(darkBrush, rectSize);

            }

            this.Size = parentForm.Size;
            this.BackgroundImage = bitmap;
            //this.BackgroundImage = bgFlat;
            pnFloater.Location = new Point((parentForm.Width - pnFloater.Width) / 2, (parentForm.Height - pnFloater.Height) / 2);

        }


        public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Ok() from Interface ISubForm then Exits SubForm Mode

            if (subForm is ISubForm)
                (subForm as ISubForm).SubForm_RightButton_Click();

            parentForm.CloseSubForm();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            //Fires SubForm_Cancel() from Interface ISubForm then Exits SubForm Mode

            if (subForm is ISubForm)
                (subForm as ISubForm).SubForm_LeftButton_Click();

            parentForm.CloseSubForm();
        }

        private void UI_ShadowPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
