#pragma warning disable RCS1138 // Add summary element to documentation comment.
#pragma warning disable SA1615
#pragma warning disable SA1514
#pragma warning disable SA1626
#pragma warning disable SA1623

namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using static RTCV.UI.UI_Extensions;

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

        // http://msdn.microsoft.com/en-us/library/ms229644%28v=vs.80%29.aspx
        public class NumericTextBox : TextBox
        {
            // Restricts the entry of characters to digits (including hex), the negative sign,
            // the decimal point, and editing keystrokes (backspace).
            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                base.OnKeyPress(e);

                var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
                var decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
                var groupSeparator = numberFormatInfo.NumberGroupSeparator;
                var negativeSign = numberFormatInfo.NegativeSign;

                var keyInput = e.KeyChar.ToString();

                if (char.IsDigit(e.KeyChar))
                {
                    // Digits are OK
                }
                else if (keyInput.Equals(decimalSeparator) && AllowDecimal)
                {
                    // Decimal separator is OK
                }
                else if (keyInput.Equals(negativeSign) && AllowNegative)
                {
                    // Negative is OK
                }
                else if (keyInput.Equals(groupSeparator))
                {
                    // group seperator is ok
                }
                else if (e.KeyChar == '\b')
                {
                    // Backspace key is OK
                }
                //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
                //    {
                //     // Let the edit control handle control and alt key combinations
                //    }
                else if (AllowSpace && e.KeyChar == ' ')
                {
                }
                else
                {
                    // Swallow this invalid key and beep
                    e.Handled = true;
                    //    MessageBeep();
                }
            }

            public int IntValue => int.Parse(Text);

            public decimal DecimalValue => decimal.Parse(Text);

            public bool AllowSpace { get; set; }

            public bool AllowDecimal { get; set; }

            public bool AllowNegative { get; set; }
        }
    }

    //From Bizhawk
    public static class NumberExtensions
    {
        public static Point GetMouseLocation(this MouseEventArgs e, object sender)
        {
            if (!(sender is Control ctr))
            {
                return new Point(e.Location.X, e.Location.Y);
            }

            var x = e.Location.X;
            var y = e.Location.Y;

            do
            {
                if (ctr.Parent != null
                    && !(ctr is UI_ComponentFormTile)
                    && !(ctr is UI_CanvasForm)
                    && !(ctr is ComponentPanel)
                    && !(ctr is ComponentForm)
                    )
                {
                    x += ctr.Location.X;
                    y += ctr.Location.Y;
                }

                ctr = ctr.Parent;
            }
            while (ctr != null);

            return new Point(x, y);
        }

        public static string ToHexString(this long n)
        {
            return $"{n:X}";
        }

        /// <summary>
        /// Force the value to be strictly between min and max (both exclued)
        /// </summary>
        /// <typeparam name="T">Anything that implements <see cref="IComparable{T}"/></typeparam>
        /// <param name="val">Value that will be clamped</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The value if strictly between min and max; otherwise min (or max depending of what is passed)</returns>
        public static T Clamp<T>(this T val, T min, T max)
            where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }

            if (val.CompareTo(max) > 0)
            {
                return max;
            }

            return val;
        }
    }

    //https://stackoverflow.com/a/2372976
    /// <summary>
    /// Data Grid view with a custom drag drop implementation
    /// </summary>
    public partial class DataGridViewDraggable : DataGridView
    {
        private bool _delayedMouseDown = false;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

        public DataGridViewDraggable()
        {
            base.DragDrop += DragDrop;
            base.DragOver += DragOver;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right && this.CurrentRow != null)
            {
                var currentRow = this.CurrentRow.Index;
                var selectedRows = this.SelectedRows.OfType<DataGridViewRow>().ToList();
                var clickedRowSelected = this.Rows[e.RowIndex].Selected;

                this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Select previously selected rows, if control is down or the clicked row was already selected
                if ((Control.ModifierKeys & Keys.Control) != 0 || clickedRowSelected)
                {
                    selectedRows.ForEach(row => row.Selected = true);
                }

                // Select a range of new rows, if shift key is down
                if ((Control.ModifierKeys & Keys.Shift) != 0)
                {
                    for (var i = currentRow; i != e.RowIndex; i += Math.Sign(e.RowIndex - currentRow))
                    {
                        this.Rows[i].Selected = true;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var rowIndex = base.HitTest(e.X, e.Y).RowIndex;
            _delayedMouseDown = (rowIndex >= 0 &&
                (ModifierKeys & Keys.Alt) > 0);

            if (!_delayedMouseDown)
            {
                base.OnMouseDown(e);

                if (rowIndex >= 0)
                {
                    // Remember the point where the mouse down occurred.
                    // The DragSize indicates the size that the mouse can move
                    // before a drag event should be started.
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    _dragBoxFromMouseDown = new Rectangle(
                        new Point(e.X - (dragSize.Width / 4), e.Y - (dragSize.Height / 4)), dragSize);
                }
                else
                {
                    // Reset the rectangle if the mouse is not over an item in the datagridview.
                    _dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Perform the delayed mouse down before the mouse up
            if (_delayedMouseDown)
            {
                _delayedMouseDown = false;
                base.OnMouseDown(e);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // If the mouse moves outside the rectangle, start the drag.
            if (this.SelectedRows != null && (e.Button & MouseButtons.Left) > 0 &&
                _dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                if (_delayedMouseDown)
                {
                    _delayedMouseDown = false;
                    if ((ModifierKeys & Keys.Control) > 0)
                    {
                        base.OnMouseDown(e);
                    }
                }

                // Proceed with the drag and drop, passing in the drag data
                var dragData = this.SelectedRows;
                this.DoDragDrop(dragData, DragDropEffects.Move);
            }
        }

        private int rowIndexOfItemUnderMouseToDrop;

        private new void DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                var rows = e.Data.GetData(typeof(DataGridViewSelectedRowCollection)) as DataGridViewSelectedRowCollection;

                if (rows != null)
                {
                    //We want to keep things in their original order rather than the order in which they were selected. Therefore sort by their rowindex as a key
                    DataGridViewRow[] _rows = rows.Cast<DataGridViewRow>().ToArray();
                    _rows = _rows.OrderBy(it => it.Index).ToArray();

                    //Go in reverse since the collection seems to be backwards?
                    foreach (DataGridViewRow row in _rows)
                    {
                        this.Rows.Remove(row);
                    }

                    // Get the row index of the item the mouse is below.
                    var hitTest = this.HitTest(clientPoint.X, clientPoint.Y);
                    rowIndexOfItemUnderMouseToDrop = hitTest.RowIndex;
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                    {
                        //Do a global hittest to figure out if we're on the header since you get -1 on both the header and the area below
                        if (clientPoint.Y <= this.ColumnHeadersHeight)
                        {
                            rowIndexOfItemUnderMouseToDrop = 0;
                        }
                        else
                        {
                            rowIndexOfItemUnderMouseToDrop = Math.Max(this.Rows.Count, 0);
                        }
                    }

                    //We InsertRange rather than inserting in the iterator so we don't have to deal with the edge case of moving two items up by one position goofing the indexes
                    this.Rows.InsertRange(rowIndexOfItemUnderMouseToDrop, _rows);

                    //Re-select the new rows
                    this.ClearSelection();
                    for (var i = rowIndexOfItemUnderMouseToDrop; i < (rowIndexOfItemUnderMouseToDrop + _rows.Length); i++)
                    {
                        this.Rows[i].Selected = true;
                    }
                }
            }
        }

        private new void DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            var headeroffset = this.Top + this.ColumnHeadersHeight;

            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

            if (clientPoint.Y < headeroffset && FirstDisplayedScrollingRowIndex > 0)
            {
                this.FirstDisplayedScrollingRowIndex -= 1;
            }
            else if (clientPoint.Y > this.Bottom - 60)
            {
                this.FirstDisplayedScrollingRowIndex += 1;
            }
        }
    }
}
