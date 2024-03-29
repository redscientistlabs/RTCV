namespace RTCV.UI.Components
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

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
        private static Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
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
                var valueulong = Convert.ToUInt64(value);
                return valueulong.ToString("X");
            }
            else
            {
                // By default, the base implementation converts the Decimal 1234.5 into the string "1234.5"
                var formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
                var formattedNumber = formattedValue as string;
                if (!string.IsNullOrEmpty(formattedNumber) && value != null)
                {
                    var unformattedDecimal = Convert.ToDecimal(value);
                    var formattedDecimal = Convert.ToDecimal(formattedNumber);
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

            if (cellStyle == null)
            {
                throw new ArgumentNullException(nameof(cellStyle));
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
                var currentValue = Convert.ToDecimal(cellValue);
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
                    var currentValue = Convert.ToDecimal(cellValue);
                    var constrainedValue = Constrain(currentValue);
                    if (constrainedValue != currentValue)
                    {
                        SetValue(rowIndex, constrainedValue);
                    }
                }
                else
                {
                    var currentValue = Convert.ToDecimal(cellValue);
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
}
