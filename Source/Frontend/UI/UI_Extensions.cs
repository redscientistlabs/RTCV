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

    /// <summary>
    /// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
    /// Defines the editing control for the DataGridViewNumericUpDownCell custom cell type.
    /// </summary>/// <summary>
    internal class DataGridViewNumericUpDownEditingControl : NumericUpDownHexFix, IDataGridViewEditingControl
    {
        // Needed to forward keyboard messages to the child TextBox control.
        [DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        // The grid that owns this editing control
        private DataGridView dataGridView;

        // Stores whether the editing control's value has changed or not
        private bool valueChanged;

        // Stores the row index in which the editing control resides
        private int rowIndex;

        /// <summary>
        /// Constructor of the editing control class
        /// </summary>
        public DataGridViewNumericUpDownEditingControl()
        {
            // The editing control must not be part of the tabbing loop
            this.TabStop = false;
        }

        // Beginning of the IDataGridViewEditingControl interface implementation

        /// <summary>
        /// Property which caches the grid that uses this editing control
        /// </summary>
        public virtual DataGridView EditingControlDataGridView
        {
            get => this.dataGridView;
            set => this.dataGridView = value;
        }

        /// <summary>
        /// Property which represents the current formatted value of the editing control
        /// </summary>
        public virtual object EditingControlFormattedValue
        {
            get => GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
            set => this.Text = (string)value;
        }

        /// <summary>
        /// Property which represents the row in which the editing control resides
        /// </summary>
        public virtual int EditingControlRowIndex
        {
            get => this.rowIndex;
            set => this.rowIndex = value;
        }

        /// <summary>
        /// Property which indicates whether the value of the editing control has changed or not
        /// </summary>
        public virtual bool EditingControlValueChanged
        {
            get => this.valueChanged;
            set => this.valueChanged = value;
        }

        /// <summary>
        /// Property which determines which cursor must be used for the editing panel,
        /// i.e. the parent of the editing control.
        /// </summary>
        public virtual Cursor EditingPanelCursor => Cursors.Default;

        /// <summary>
        /// Property which indicates whether the editing control needs to be repositioned
        /// when its value changes.
        /// </summary>
        public virtual bool RepositionEditingControlOnValueChange => false;

        /// <summary>
        /// Method called by the grid before the editing control is shown so it can adapt to the
        /// provided cell style.
        /// </summary>
        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 255)
            {
                // The NumericUpDown control does not support transparent back colors
                var opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
                this.BackColor = opaqueBackColor;
                this.dataGridView.EditingPanel.BackColor = opaqueBackColor;
            }
            else
            {
                this.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(dataGridViewCellStyle.Alignment);
        }

        /// <summary>
        /// Method called by the grid on keystrokes to determine if the editing control is
        /// interested in the key or not.
        /// </summary>
        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                    {
                        if (this.Controls[1] is TextBox textBox)
                        {
                            // If the end of the selection is at the end of the string,
                            // let the DataGridView treat the key message
                            if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)) ||
                                (this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)))
                            {
                                return true;
                            }
                        }
                        break;
                    }

                case Keys.Left:
                    {
                        if (this.Controls[1] is TextBox textBox)
                        {
                            // If the end of the selection is at the begining of the string
                            // or if the entire text is selected and we did not start editing,
                            // send this character to the dataGridView, else process the key message
                            if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)) ||
                                (this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)))
                            {
                                return true;
                            }
                        }
                        break;
                    }

                case Keys.Down:
                    // If the current value hasn't reached its minimum yet, handle the key. Otherwise let
                    // the grid handle it.
                    if (this.Value > this.Minimum)
                    {
                        return true;
                    }
                    break;

                case Keys.Up:
                    // If the current value hasn't reached its maximum yet, handle the key. Otherwise let
                    // the grid handle it.
                    if (this.Value < this.Maximum)
                    {
                        return true;
                    }
                    break;

                case Keys.Home:
                case Keys.End:
                    {
                        // Let the grid handle the key if the entire text is selected.
                        if (this.Controls[1] is TextBox textBox)
                        {
                            if (textBox.SelectionLength != textBox.Text.Length)
                            {
                                return true;
                            }
                        }
                        break;
                    }

                case Keys.Delete:
                    {
                        // Let the grid handle the key if the carret is at the end of the text.
                        if (this.Controls[1] is TextBox textBox)
                        {
                            if (textBox.SelectionLength > 0 ||
                                textBox.SelectionStart < textBox.Text.Length)
                            {
                                return true;
                            }
                        }
                        break;
                    }
            }
            return !dataGridViewWantsInputKey;
        }

        /// <summary>
        /// Returns the current value of the editing control.
        /// </summary>
        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            bool userEdit = this.UserEdit;
            try
            {
                // Prevent the Value from being set to Maximum or Minimum when the cell is being painted.
                this.UserEdit = (context & DataGridViewDataErrorContexts.Display) == 0;
                return this.Value.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
            }
            finally
            {
                this.UserEdit = userEdit;
            }
        }

        /// <summary>
        /// Called by the grid to give the editing control a chance to prepare itself for
        /// the editing session.
        /// </summary>
        public virtual void PrepareEditingControlForEdit(bool selectAll)
        {
            if (this.Controls[1] is TextBox textBox)
            {
                if (selectAll)
                {
                    textBox.SelectAll();
                }
                else
                {
                    // Do not select all the text, but
                    // position the caret at the end of the text
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        // End of the IDataGridViewEditingControl interface implementation

        /// <summary>
        /// Small utility function that updates the local dirty state and
        /// notifies the grid of the value change.
        /// </summary>
        private void NotifyDataGridViewOfValueChange()
        {
            if (!this.valueChanged)
            {
                this.valueChanged = true;
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }

        //Let's just assume it was always changed
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            NotifyDataGridViewOfValueChange();
        }

        //Handle OnLostFocus to update if you paste.
        //Intercepting paste doesn't work and OnValueChanged also doesn't
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            NotifyDataGridViewOfValueChange();
        }

        /// <summary>
        /// Listen to the ValueChanged notification to forward the change to the grid.
        /// </summary>
        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            if (this.Focused)
            {
                // Let the DataGridView know about the value change
                NotifyDataGridViewOfValueChange();
            }
        }

        /// <summary>
        /// A few keyboard messages need to be forwarded to the inner textbox of the
        /// NumericUpDown control so that the first character pressed appears in it.
        /// </summary>
        protected override bool ProcessKeyEventArgs(ref Message m)
        {
            if (this.Controls[1] is TextBox textBox)
            {
                SendMessage(textBox.Handle, m.Msg, m.WParam, m.LParam);
                return true;
            }
            else
            {
                return base.ProcessKeyEventArgs(ref m);
            }
        }
    }

    //Fixes microsoft's numericupdown hex issues. Thanks microsoft
    public class NumericUpDownHexFix : NumericUpDown
    {
        private bool currentValueChanged = false;

        public NumericUpDownHexFix()
        {
            base.Minimum = 0;
            base.Maximum = ulong.MaxValue;
            this.ValueChanged += OnValueChanged;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new decimal Maximum
        {    // Doesn't serialize properly
            get => base.Maximum;
            set => base.Maximum = value;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            currentValueChanged = true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0 && this.Value < this.Maximum)
            {
                this.Value += this.Increment;
            }
            else if (e.Delta < 0 && this.Value > this.Minimum)
            {
                this.Value -= this.Increment;
            }
        }

        protected override void UpdateEditText()
        {
            if (UserEdit)
            {
                if (base.Hexadecimal)
                {
                    HexParseEditText();
                }
                else
                {
                    ParseEditText();
                }
            }

            if (currentValueChanged || (!string.IsNullOrEmpty(Text) && !(Text.Length == 1 && Text == "-")))
            {
                currentValueChanged = false;
                ChangingText = true;
                Text = GetNumberText(Value);
                ChangingText = false;
            }
        }

        private string GetNumberText(decimal num)
        {
            string text;

            if (Hexadecimal)
            {
                text = ((ulong)num).ToString("X", CultureInfo.InvariantCulture);
                Debug.Assert(text == text.ToUpper(CultureInfo.InvariantCulture), "GetPreferredSize assumes hex digits to be uppercase.");
            }
            else
            {
                text = num.ToString((ThousandsSeparator ? "N" : "F") + DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            }
            return text;
        }

        private void HexParseEditText()
        {
            try
            {
                if (!string.IsNullOrEmpty(Text) && !(Text.Length == 1 && Text == "-"))
                {
                    var val = Convert.ToDecimal(Convert.ToUInt64(base.Text, 16));

                    if (val > Maximum)
                    {
                        base.Text = string.Format("{0:X}", (uint)Maximum);
                        //    val = (uint)Maximum;
                    }

                    if (!string.IsNullOrEmpty(base.Text))
                    {
                        this.Value = val;
                    }
                }
            }
            catch { }
            finally
            {
                base.UserEdit = false;
            }
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

    /// <summary>
    /// Provides a generic collection that supports data binding and additionally supports sorting.
    /// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
    /// If the elements are IComparable it uses that; otherwise compares the ToString()
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SortableBindingList<T> : BindingList<T>
        where T : class
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        public SortableBindingList()
        {
        }

        #pragma warning disable CA1200 //Avoid using prefix with cref tag
        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        /// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="T:System.ComponentModel.BindingList`1" />.</param>
        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override bool SupportsSortingCore => true;

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override bool IsSortedCore => _isSorted;

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore => _sortDirection;

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore => _sortProperty;

        /// <summary>
        /// Removes any sort applied with ApplySortCore if sorting is implemented
        /// </summary>
        protected override void RemoveSortCore()
        {
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
            _isSorted = false; //thanks Luca
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;
            _sortDirection = direction;

            List<T> list = Items as List<T>;
            if (list == null)
            {
                return;
            }

            list.Sort(Compare);

            _isSorted = true;
            //fire an event that the list has been changed.
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private int Compare(T lhs, T rhs)
        {
            var result = OnComparison(lhs, rhs);
            //invert if descending
            if (_sortDirection == ListSortDirection.Descending)
            {
                result = -result;
            }

            return result;
        }

        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
            object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1; //nulls are equal
            }
            if (rhsValue == null)
            {
                return 1; //first has value, second doesn't
            }
            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }
            if (lhsValue.Equals(rhsValue))
            {
                return 0; //both are the same
            }
            //not comparable, compare ToString
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}
