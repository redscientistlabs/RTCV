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
    using System.Text;
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

    /// <summary>
    /// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
    /// Custom column type dedicated to the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor for the DataGridViewNumericUpDownColumn class.
        /// </summary>
        public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell())
        {
        }

        /// <summary>
        /// Represents the implicit cell that gets cloned when adding rows to the grid.
        /// </summary>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                var dataGridViewNumericUpDownCell = value as DataGridViewNumericUpDownCell;
                if (value != null && dataGridViewNumericUpDownCell == null)
                {
                    throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewNumericUpDownElements.DataGridViewNumericUpDownCell or derive from it.");
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [
            Category("Data"),
            Description("Indicates the amount to increment or decrement on each button click.")
        ]
        public decimal Increment
        {
            get
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumericUpDownCellTemplate.Increment;
            }
            set
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumericUpDownCellTemplate.Increment = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    var rowCount = dataGridViewRows.Count;
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetIncrement(rowIndex, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [
            Category("Data"),
            Description("Indicates the maximum value for the numeric up-down cells."),
            RefreshProperties(RefreshProperties.All)
        ]
        public decimal Maximum
        {
            get
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumericUpDownCellTemplate.Maximum;
            }
            set
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumericUpDownCellTemplate.Maximum = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    var rowCount = dataGridViewRows.Count;
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetMaximum(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows,
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [
            Category("Data"),
            Description("Indicates the minimum value for the numeric up-down cells."),
            RefreshProperties(RefreshProperties.All)
        ]
        public decimal Minimum
        {
            get
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumericUpDownCellTemplate.Minimum;
            }
            set
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumericUpDownCellTemplate.Minimum = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    var rowCount = dataGridViewRows.Count;
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetMinimum(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows,
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Replicates the ThousandsSeparator property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [
            Category("Data"),
            DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator),
            Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")
        ]
        public bool ThousandsSeparator
        {
            get
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumericUpDownCellTemplate.ThousandsSeparator;
            }
            set
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumericUpDownCellTemplate.ThousandsSeparator = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    var rowCount = dataGridViewRows.Count;
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetThousandsSeparator(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows,
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
        /// </summary>
        [
            Category("Data"),
            Description("Indicates if it should display as hexadecimal"),
            DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal),
            RefreshProperties(RefreshProperties.All)
        ]
        public bool Hexadecimal
        {
            get
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumericUpDownCellTemplate.Hexadecimal;
            }
            set
            {
                if (this.NumericUpDownCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumericUpDownCellTemplate.Hexadecimal = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    var rowCount = dataGridViewRows.Count;
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
                        {
                            dataGridViewCell.SetHexadecimal(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);

                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows,
                    //       column headers / row headers as needed.
                }
            }
        }

        /// <summary>
        /// Small utility function that returns the template cell as a DataGridViewNumericUpDownCell
        /// </summary>
        private DataGridViewNumericUpDownCell NumericUpDownCellTemplate => (DataGridViewNumericUpDownCell)this.CellTemplate;

        /// <summary>
        /// Returns a standard compact string representation of the column.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder(100);
            sb.Append("DataGridViewNumericUpDownColumn { Name=");
            sb.Append(this.Name);
            sb.Append(", Index=");
            sb.Append(this.Index.ToString(CultureInfo.CurrentCulture));
            sb.Append(" }");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
    /// Defines a NumericUpDown cell type for the System.Windows.Forms.DataGridView control
    /// </summary>
    public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        // Used in KeyEntersEditMode function
        [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern short VkKeyScan(char key);

        // Used in TranslateAlignment function
        private const DataGridViewContentAlignment anyRight = DataGridViewContentAlignment.TopRight |
                                                              DataGridViewContentAlignment.MiddleRight |
                                                              DataGridViewContentAlignment.BottomRight;

        private const DataGridViewContentAlignment anyCenter = DataGridViewContentAlignment.TopCenter |
                                                               DataGridViewContentAlignment.MiddleCenter |
                                                               DataGridViewContentAlignment.BottomCenter;

        // Default dimensions of the static rendering bitmap used for the painting of the non-edited cells
        private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth = 100;

        private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight = 22;

        // Default value of the DecimalPlaces property
        internal const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces = 0;

        // Default value of the Increment property
        internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement = decimal.One;

        // Default value of the Maximum property
        internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum = (decimal)100.0;

        // Default value of the Minimum property
        internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum = decimal.Zero;

        // Default value of the ThousandsSeparator property
        internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator = false;

        internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal = false;

        // Type of this cell's editing control
        private static Type defaultEditType = typeof(DataGridViewNumericUpDownEditingControl);

        // Type of this cell's value. The formatted value type is string, the same as the base class DataGridViewTextBoxCell
        private static Type defaultValueType = typeof(string);

        // The bitmap used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
        [ThreadStatic]
        private static Bitmap renderingBitmap;

        // The NumericUpDown control used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
        [ThreadStatic]
        private NumericUpDownHexFix paintingNumericUpDown;

        private int decimalPlaces;       // Caches the value of the DecimalPlaces property
        private decimal increment;       // Caches the value of the Increment property
        private decimal minimum;         // Caches the value of the Minimum property
        private decimal maximum;         // Caches the value of the Maximum property
        private bool thousandsSeparator; // Caches the value of the ThousandsSeparator property
        private bool hexadecimal;

        /// <summary>
        /// Constructor for the DataGridViewNumericUpDownCell cell type
        /// </summary>
        public DataGridViewNumericUpDownCell()
        {
            // Create a thread specific bitmap used for the painting of the non-edited cells
            if (renderingBitmap == null)
            {
                renderingBitmap = new Bitmap(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth, DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight);
            }

            // Create a thread specific NumericUpDown control used for the painting of the non-edited cells
            if (paintingNumericUpDown == null)
            {
                paintingNumericUpDown = new NumericUpDownHexFix
                {
                    // Some properties only need to be set once for the lifetime of the control:
                    BorderStyle = BorderStyle.None,
                    Maximum = decimal.MaxValue / 10,
                    Minimum = decimal.MinValue / 10,
                    Hexadecimal = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal
                };
                //paintingNumericUpDown.DoubleBuffered(true);
            }

            // Set the default values of the properties:
            this.decimalPlaces = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces;
            this.increment = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement;
            this.minimum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum;
            this.maximum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum;
            this.thousandsSeparator = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator;
            this.hexadecimal = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal;
        }

        /// <summary>
        /// The DecimalPlaces property replicates the one from the NumericUpDown control
        /// </summary>
        [
            DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces)
        ]
        public int DecimalPlaces
        {
            get => this.decimalPlaces;

            set
            {
                if (value < 0 || value > 99)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The DecimalPlaces property cannot be smaller than 0 or larger than 99.");
                }
                if (this.decimalPlaces != value)
                {
                    SetDecimalPlaces(this.RowIndex, value);
                    OnCommonChange();  // Assure that the cell or column gets repainted and autosized if needed
                }
            }
        }

        /// <summary>
        /// Returns the current DataGridView EditingControl as a DataGridViewNumericUpDownEditingControl control
        /// </summary>
        private DataGridViewNumericUpDownEditingControl EditingNumericUpDown => this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;

        /// <summary>
        /// Define the type of the cell's editing control
        /// </summary>
        public override Type EditType => defaultEditType; // the type is DataGridViewNumericUpDownEditingControl

        /// <summary>
        /// The Increment property replicates the one from the NumericUpDown control
        /// </summary>
        public decimal Increment
        {
            get => this.increment;

            set
            {
                if (value < (decimal)0.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The Increment property cannot be smaller than 0.");
                }
                SetIncrement(this.RowIndex, value);
                // No call to OnCommonChange is needed since the increment value does not affect the rendering of the cell.
            }
        }

        /// <summary>
        /// The Maximum property replicates the one from the NumericUpDown control
        /// </summary>
        public decimal Maximum
        {
            get => this.maximum;

            set
            {
                if (this.maximum != value)
                {
                    SetMaximum(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }

        /// <summary>
        /// The Minimum property replicates the one from the NumericUpDown control
        /// </summary>
        public decimal Minimum
        {
            get => this.minimum;

            set
            {
                if (this.minimum != value)
                {
                    SetMinimum(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }

        /// <summary>
        /// The ThousandsSeparator property replicates the one from the NumericUpDown control
        /// </summary>
        [
            DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator)
        ]
        public bool ThousandsSeparator
        {
            get => this.thousandsSeparator;

            set
            {
                if (this.thousandsSeparator != value)
                {
                    SetThousandsSeparator(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }

        [
          DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal),
        ]
        public bool Hexadecimal
        {
            get => this.hexadecimal;

            set
            {
                if (this.hexadecimal != value)
                {
                    SetHexadecimal(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }

        /// <summary>
        /// Returns the type of the cell's Value property
        /// </summary>
        public override Type ValueType
        {
            get
            {
                Type valueType = base.ValueType;
                if (valueType != null)
                {
                    return valueType;
                }
                return defaultValueType;
            }
        }

        /// <summary>
        /// Clones a DataGridViewNumericUpDownCell cell, copies all the custom properties.
        /// </summary>
        public override object Clone()
        {
            var dataGridViewCell = base.Clone() as DataGridViewNumericUpDownCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
                dataGridViewCell.Increment = this.Increment;
                dataGridViewCell.Maximum = this.Maximum;
                dataGridViewCell.Minimum = this.Minimum;
                dataGridViewCell.ThousandsSeparator = this.ThousandsSeparator;
                dataGridViewCell.Hexadecimal = this.Hexadecimal;
            }
            return dataGridViewCell;
        }

        /// <summary>
        /// Returns the provided value constrained to be within the min and max.
        /// </summary>
        private decimal Constrain(decimal value)
        {
            Debug.Assert(this.minimum <= this.maximum);
            if (value < this.minimum)
            {
                value = this.minimum;
            }
            if (value > this.maximum)
            {
                value = this.maximum;
            }
            return value;
        }

        /// <summary>
        /// DetachEditingControl gets called by the DataGridView control when the editing session is ending
        /// </summary>
        [
            EditorBrowsable(EditorBrowsableState.Advanced)
        ]
        public override void DetachEditingControl()
        {
            DataGridView dataGridView = this.DataGridView;
            if (dataGridView == null || dataGridView.EditingControl == null)
            {
                throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
            }

            if (dataGridView.EditingControl is NumericUpDownHexFix numericUpDown)
            {
                // Editing controls get recycled. Indeed, when a DataGridViewNumericUpDownCell cell gets edited
                // after another DataGridViewNumericUpDownCell cell, the same editing control gets reused for
                // performance reasons (to avoid an unnecessary control destruction and creation).
                // Here the undo buffer of the TextBox inside the NumericUpDown control gets cleared to avoid
                // interferences between the editing sessions.
                if (numericUpDown.Controls[1] is TextBox textBox)
                {
                    textBox.ClearUndo();
                }
            }

            base.DetachEditingControl();
        }

        /// <summary>
        /// Adjusts the location and size of the editing control given the alignment characteristics of the cell
        /// </summary>
        private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
        {
            // Add a 1 pixel padding on the left and right of the editing control
            editingControlBounds.X += 1;
            editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);

            // Adjust the vertical location of the editing control:
            var preferredHeight = cellStyle.Font.Height + 3;
            if (preferredHeight < editingControlBounds.Height)
            {
                switch (cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.MiddleLeft:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.MiddleRight:
                        editingControlBounds.Y += (editingControlBounds.Height - preferredHeight) / 2;
                        break;
                    case DataGridViewContentAlignment.BottomLeft:
                    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.BottomRight:
                        editingControlBounds.Y += editingControlBounds.Height - preferredHeight;
                        break;
                }
            }

            return editingControlBounds;
        }

        /// <summary>
        /// Customized implementation of the GetErrorIconBounds function in order to draw the potential
        /// error icon next to the up/down buttons and not on top of them.
        /// </summary>
        protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            const int ButtonsWidth = 16;

            Rectangle errorIconBounds = base.GetErrorIconBounds(graphics, cellStyle, rowIndex);
            if (this.DataGridView.RightToLeft == RightToLeft.Yes)
            {
                errorIconBounds.X = errorIconBounds.Left + ButtonsWidth;
            }
            else
            {
                errorIconBounds.X = errorIconBounds.Left - ButtonsWidth;
            }
            return errorIconBounds;
        }

        /// <summary>
        /// Customized implementation of the GetFormattedValue function in order to include the decimal and thousand separator
        /// characters in the formatted representation of the cell value.
        /// </summary>
        protected override object GetFormattedValue(object value,
                                                    int rowIndex,
                                                    ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            if (this.Hexadecimal)
            {
                var valueulong = System.Convert.ToUInt64(value);
                return valueulong.ToString("X");
            }
            else
            {
                // By default, the base implementation converts the Decimal 1234.5 into the string "1234.5"
                var formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
                var formattedNumber = formattedValue as string;
                if (!string.IsNullOrEmpty(formattedNumber) && value != null)
                {
                    var unformattedDecimal = System.Convert.ToDecimal(value);
                    var formattedDecimal = System.Convert.ToDecimal(formattedNumber);
                    if (unformattedDecimal == formattedDecimal)
                    {
                        // The base implementation of GetFormattedValue (which triggers the CellFormatting event) did nothing else than
                        // the typical 1234.5 to "1234.5" conversion. But depending on the values of ThousandsSeparator and DecimalPlaces,
                        // this may not be the actual string displayed. The real formatted value may be "1,234.500"
                        return formattedDecimal.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
                    }
                }
                return formattedValue;
            }
        }

        /// <summary>
        /// Custom implementation of the GetPreferredSize function. This implementation uses the preferred size of the base
        /// DataGridViewTextBoxCell cell and adds room for the up/down buttons.
        /// </summary>
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (this.DataGridView == null)
            {
                return new Size(-1, -1);
            }

            Size preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
            if (constraintSize.Width == 0)
            {
                const int ButtonsWidth = 16; // Account for the width of the up/down buttons.
                const int ButtonMargin = 8;  // Account for some blank pixels between the text and buttons.
                preferredSize.Width += ButtonsWidth + ButtonMargin;
            }
            return preferredSize;
        }

        /// <summary>
        /// Custom implementation of the InitializeEditingControl function. This function is called by the DataGridView control
        /// at the beginning of an editing session. It makes sure that the properties of the NumericUpDown editing control are
        /// set according to the cell properties.
        /// </summary>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            if (this.DataGridView.EditingControl is NumericUpDownHexFix numericUpDown)
            {
                numericUpDown.BorderStyle = BorderStyle.None;
                numericUpDown.DecimalPlaces = this.DecimalPlaces;
                numericUpDown.Increment = this.Increment;
                numericUpDown.Maximum = this.Maximum;
                numericUpDown.Minimum = this.Minimum;
                numericUpDown.ThousandsSeparator = this.ThousandsSeparator;
                numericUpDown.Hexadecimal = this.Hexadecimal;
                if (!(initialFormattedValue is string initialFormattedValueStr))
                {
                    numericUpDown.Text = string.Empty;
                }
                else
                {
                    numericUpDown.Text = initialFormattedValueStr;
                }
            }
        }

        /// <summary>
        /// Custom implementation of the KeyEntersEditMode function. This function is called by the DataGridView control
        /// to decide whether a keystroke must start an editing session or not. In this case, a new session is started when
        /// a digit or negative sign key is hit.
        /// </summary>
        public override bool KeyEntersEditMode(KeyEventArgs e)
        {
            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            Keys negativeSignKey = Keys.None;
            var negativeSignStr = numberFormatInfo.NegativeSign;
            if (!string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
            {
                negativeSignKey = (Keys)(VkKeyScan(negativeSignStr[0]));
            }
            if (Hexadecimal && ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)))
            {
                return true;
            }
            if ((char.IsDigit((char)e.KeyCode) ||
                 (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
                 negativeSignKey == e.KeyCode ||
                 Keys.Subtract == e.KeyCode) &&
                !e.Shift && !e.Alt && !e.Control)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when a cell characteristic that affects its rendering and/or preferred size has changed.
        /// This implementation only takes care of repainting the cells. The DataGridView's autosizing methods
        /// also need to be called in cases where some grid elements autosize.
        /// </summary>
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    // Invalidate and autosize column
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);

                    // TODO: Add code to autosize the cell's column, the rows, the column headers
                    // and the row headers depending on their autosize settings.
                    // The DataGridView control does not expose a public method that takes care of this.
                }
                else
                {
                    // The DataGridView control exposes a public method called UpdateCellValue
                    // that invalidates the cell so that it gets repainted and also triggers all
                    // the necessary autosizing: the cell's column and/or row, the column headers
                    // and the row headers are autosized depending on their autosize settings.
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }

        /// <summary>
        /// Determines whether this cell, at the given row index, shows the grid's editing control or not.
        /// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
        /// </summary>
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            return this.DataGridView.EditingControl is DataGridViewNumericUpDownEditingControl numericUpDownEditingControl && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
        }

        /// <summary>
        /// Custom paints the cell. The base implementation of the DataGridViewTextBoxCell type is called first,
        /// dropping the icon error and content foreground parts. Those two parts are painted by this custom implementation.
        /// In this sample, the non-edited NumericUpDown control is painted by using a call to Control.DrawToBitmap. This is
        /// an easy solution for painting controls but it's not necessarily the most performant. An alternative would be to paint
        /// the NumericUpDown control piece by piece (text and up/down buttons).
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
                                      object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (this.DataGridView == null)
            {
                return;
            }

            // First paint the borders and background of the cell.
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
                       paintParts & ~(DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.ContentForeground));

            Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
            var cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
            var cellEdited = cellCurrent && this.DataGridView.EditingControl != null;

            // If the cell is in editing mode, there is nothing else to paint
            if (!cellEdited)
            {
                if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
                {
                    // Paint a NumericUpDown control
                    // Take the borders into account
                    Rectangle borderWidths = BorderWidths(advancedBorderStyle);
                    Rectangle valBounds = cellBounds;
                    valBounds.Offset(borderWidths.X, borderWidths.Y);
                    valBounds.Width -= borderWidths.Right;
                    valBounds.Height -= borderWidths.Bottom;
                    // Also take the padding into account
                    if (cellStyle.Padding != Padding.Empty)
                    {
                        if (this.DataGridView.RightToLeft == RightToLeft.Yes)
                        {
                            valBounds.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
                        }
                        else
                        {
                            valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                        }
                        valBounds.Width -= cellStyle.Padding.Horizontal;
                        valBounds.Height -= cellStyle.Padding.Vertical;
                    }
                    // Determine the NumericUpDown control location
                    valBounds = GetAdjustedEditingControlBounds(valBounds, cellStyle);

                    // Set all the relevant properties
                    paintingNumericUpDown.Value = Convert.ToDecimal(value);
                    paintingNumericUpDown.Hexadecimal = this.Hexadecimal;
                    paintingNumericUpDown.Font = cellStyle.Font;
                    paintingNumericUpDown.Width = valBounds.Width;
                    paintingNumericUpDown.Height = valBounds.Height;
                    paintingNumericUpDown.Location = new Point(0, -paintingNumericUpDown.Height - 100);

                    Color foreColor;
                    var cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;
                    if (PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
                    {
                        foreColor = cellStyle.SelectionForeColor;
                    }
                    else
                    {
                        foreColor = cellStyle.ForeColor;
                    }
                    if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
                    {
                        if (foreColor.A < 255)
                        {
                            // The NumericUpDown control does not support transparent fore colors
                            foreColor = Color.FromArgb(255, foreColor);
                        }
                        paintingNumericUpDown.ForeColor = foreColor;

                        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                                   cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground);
                    }

                    Color backColor;
                    if (PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
                    {
                        backColor = cellStyle.SelectionBackColor;
                    }
                    else
                    {
                        backColor = cellStyle.BackColor;
                    }
                    if (PartPainted(paintParts, DataGridViewPaintParts.Background))
                    {
                        if (backColor.A < 255)
                        {
                            // The NumericUpDown control does not support transparent back colors
                            backColor = Color.FromArgb(255, backColor);
                        }
                        paintingNumericUpDown.BackColor = backColor;
                    }
                    // Finally paint the NumericUpDown control
                    /*
                    Rectangle srcRect = new Rectangle(0, 0, valBounds.Width, valBounds.Height);
                    if (srcRect.Width > 0 && srcRect.Height > 0)
                    {

                        paintingNumericUpDown.DrawToBitmap(renderingBitmap, srcRect);
                        graphics.DrawImage(renderingBitmap, new Rectangle(valBounds.Location, valBounds.Size),
                                           srcRect, GraphicsUnit.Pixel);
                    }*/
                }
                if (PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon))
                {
                    // Paint the potential error icon on top of the NumericUpDown control
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                               cellStyle, advancedBorderStyle, DataGridViewPaintParts.ErrorIcon);
                }
            }
        }

        /// <summary>
        /// Little utility function called by the Paint function to see if a particular part needs to be painted.
        /// </summary>
        private static bool PartPainted(DataGridViewPaintParts paintParts, DataGridViewPaintParts paintPart)
        {
            return (paintParts & paintPart) != 0;
        }

        /// <summary>
        /// Custom implementation of the PositionEditingControl method called by the DataGridView control when it
        /// needs to relocate and/or resize the editing control.
        /// </summary>
        public override void PositionEditingControl(bool setLocation,
                                            bool setSize,
                                            Rectangle cellBounds,
                                            Rectangle cellClip,
                                            DataGridViewCellStyle cellStyle,
                                            bool singleVerticalBorderAdded,
                                            bool singleHorizontalBorderAdded,
                                            bool isFirstDisplayedColumn,
                                            bool isFirstDisplayedRow)
        {
            Rectangle editingControlBounds = PositionEditingPanel(cellBounds,
                                                        cellClip,
                                                        cellStyle,
                                                        singleVerticalBorderAdded,
                                                        singleHorizontalBorderAdded,
                                                        isFirstDisplayedColumn,
                                                        isFirstDisplayedRow);
            editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
            this.DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
            this.DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
        }

        /// <summary>
        /// Utility function that sets a new value for the DecimalPlaces property of the cell. This function is used by
        /// the cell and column DecimalPlaces property. The column uses this method instead of the DecimalPlaces
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetDecimalPlaces(int rowIndex, int value)
        {
            Debug.Assert(value >= 0 && value <= 99);
            this.decimalPlaces = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.DecimalPlaces = value;
            }
        }

        /// Utility function that sets a new value for the Increment property of the cell. This function is used by
        /// the cell and column Increment property. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        internal void SetIncrement(int rowIndex, decimal value)
        {
            Debug.Assert(value >= (decimal)0.0);
            this.increment = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Increment = value;
            }
        }

        /// Utility function that sets a new value for the Maximum property of the cell. This function is used by
        /// the cell and column Maximum property. The column uses this method instead of the Maximum
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        internal void SetMaximum(int rowIndex, decimal value)
        {
            this.maximum = value;
            if (this.minimum > this.maximum)
            {
                this.minimum = this.maximum;
            }
            var cellValue = GetValue(rowIndex);
            if (cellValue != null)
            {
                var currentValue = System.Convert.ToDecimal(cellValue);
                var constrainedValue = Constrain(currentValue);
                if (constrainedValue != currentValue)
                {
                    SetValue(rowIndex, constrainedValue);
                }
            }
            Debug.Assert(this.maximum == value);
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Maximum = value;
            }
        }

        /// Utility function that sets a new value for the Minimum property of the cell. This function is used by
        /// the cell and column Minimum property. The column uses this method instead of the Minimum
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        internal void SetMinimum(int rowIndex, decimal value)
        {
            this.minimum = value;
            if (this.minimum > this.maximum)
            {
                this.maximum = value;
            }
            var cellValue = GetValue(rowIndex);
            if (cellValue != null)
            {
                if (Hexadecimal)
                {
                    var currentValue = System.Convert.ToDecimal(cellValue);
                    var constrainedValue = Constrain(currentValue);
                    if (constrainedValue != currentValue)
                    {
                        SetValue(rowIndex, constrainedValue);
                    }
                }
                else
                {
                    var currentValue = System.Convert.ToDecimal(cellValue);
                    var constrainedValue = Constrain(currentValue);
                    if (constrainedValue != currentValue)
                    {
                        SetValue(rowIndex, constrainedValue);
                    }
                }
            }
            Debug.Assert(this.minimum == value);
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Minimum = value;
            }
        }

        /// Utility function that sets a new value for the ThousandsSeparator property of the cell. This function is used by
        /// the cell and column ThousandsSeparator property. The column uses this method instead of the ThousandsSeparator
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        internal void SetThousandsSeparator(int rowIndex, bool value)
        {
            this.thousandsSeparator = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.ThousandsSeparator = value;
            }
        }

        internal void SetHexadecimal(int rowIndex, bool value)
        {
            this.hexadecimal = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericUpDown.Hexadecimal = value;
            }
        }

        /// <summary>
        /// Returns a standard textual representation of the cell.
        /// </summary>
        public override string ToString()
        {
            return "DataGridViewNumericUpDownCell { ColumnIndex=" + ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", RowIndex=" + RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
        }

        /// <summary>
        /// Little utility function used by both the cell and column types to translate a DataGridViewContentAlignment value into
        /// a HorizontalAlignment value.
        /// </summary>
        internal static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
        {
            if ((align & anyRight) != 0)
            {
                return HorizontalAlignment.Right;
            }
            else if ((align & anyCenter) != 0)
            {
                return HorizontalAlignment.Center;
            }
            else
            {
                return HorizontalAlignment.Left;
            }
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
}
