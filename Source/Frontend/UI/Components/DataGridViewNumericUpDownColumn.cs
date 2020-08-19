namespace RTCV.UI.Components
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

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
}
