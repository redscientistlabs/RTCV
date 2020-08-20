#pragma warning disable RCS1138 // Add summary element to documentation comment.
#pragma warning disable SA1615
#pragma warning disable SA1514
#pragma warning disable SA1626
#pragma warning disable SA1623

namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class UI_Extensions
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static DialogResult GetInputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            textBox.GotFocus += (o, e) => UICore.UpdateFormFocusStatus(false);
            textBox.LostFocus += (o, e) => UICore.UpdateFormFocusStatus(false);

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.Shown += (f, g) =>
            {
                form.TopMost = true;
                form.Focus();
                form.BringToFront();
            };
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static void Tint(this Bitmap bmp, Color col)
        {
            var rectSize = new Rectangle(0, 0, bmp.Width, bmp.Height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(bmp, rectSize);

                var darkBrush = new SolidBrush(col);
                g.FillRectangle(darkBrush, rectSize);
            }
        }

        private const int SRCCOPY = 0xCC0020;

        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, int rop);

        public static Bitmap getFormScreenShot(this Control con)
        {
            logger.Trace($"getFormScreenShot ClientRectangle | Width: {con.ClientRectangle.Width} | Height: {con.ClientRectangle.Height} | X: {con.ClientRectangle.X} | Y: {con.ClientRectangle.Y}");
            try
            {
                var bmp = new Bitmap(con.ClientRectangle.Width, con.ClientRectangle.Height);
                using (var bmpGraphics = Graphics.FromImage(bmp))
                {
                    var bmpDC = bmpGraphics.GetHdc();
                    using (var formGraphics = Graphics.FromHwnd(con.Handle))
                    {
                        var formDC = formGraphics.GetHdc();
                        BitBlt(bmpDC, 0, 0, con.ClientRectangle.Width, con.ClientRectangle.Height, formDC, 0, 0, SRCCOPY);
                        formGraphics.ReleaseHdc(formDC);
                    }

                    bmpGraphics.ReleaseHdc(bmpDC);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to get form screenshot.");
                return new Bitmap(1, 1);
            }
        }

        #region CONTROL EXTENSIONS

        public static List<Control> getControlsWithTag(this Control.ControlCollection controls)
        {
            var allControls = new List<Control>();

            foreach (Control c in controls)
            {
                if (c.Tag != null)
                {
                    allControls.Add(c);
                }

                if (c.HasChildren)
                {
                    allControls.AddRange(c.Controls.getControlsWithTag()); //Recursively check all children controls as well; ie groupboxes or tabpages
                }
            }

            return allControls;
        }

        #endregion CONTROL EXTENSIONS

        public class RTC_Standalone_Form : Form { }
    }
}
