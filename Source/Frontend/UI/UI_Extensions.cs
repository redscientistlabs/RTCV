using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RTCV.UI
{
	public static class UI_Extensions
	{
		public static DialogResult GetInputBox(string title, string promptText, ref string value)
		{
			
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

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

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}

		public interface ISF<T>
		{
			//Interface for Singleton Form
			T Me();
			T NewMe();
		}

		public interface IColorable
		{

		}

		public class RTC_Standalone_Form : Form { }

		public class ComponentForm : Form
		{

			Panel defaultPanel = null;
			Panel previousPanel = null;

			public bool undockedSizable = true;
			public bool popoutAllowed = true;
			
			public void AnchorToPanel(Panel pn)
			{

				if (defaultPanel == null)
					defaultPanel = pn;

				previousPanel = pn;

				this.Hide();
				this.Parent?.Controls.Remove(this);

				this.FormBorderStyle = FormBorderStyle.None;

				//Remove ComponentForm from target panel if required
				ComponentForm componentFormInTargetPanel = (pn?.Controls.Cast<Control>().FirstOrDefault(it => it is ComponentForm) as ComponentForm);
				if (componentFormInTargetPanel != null && componentFormInTargetPanel != this)
					pn.Controls.Remove(componentFormInTargetPanel);

				this.TopLevel = false;
				this.TopMost = false;
				pn.Controls.Add(this);

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
				if (defaultPanel == null)
					throw new Exception("Default panel unset");

				Panel targetPanel;

				//We select which panel we want to restore the ComponentForm to
				if (previousPanel?.Parent?.Visible ?? false)
					targetPanel = previousPanel;
				else                            //If the ComponentForm was moved to another panel than the default one
					targetPanel = defaultPanel; //and that panel was hidden, then we move it back to the original panel.

				//Restore the state since we don't want it maximized or minimized
				this.WindowState = FormWindowState.Normal;

				//This searches for a ComponentForm in the target Panel
				ComponentForm componentFormInTargetPanel = (targetPanel?.Controls.Cast<Control>().FirstOrDefault(it => it is ComponentForm) as ComponentForm);
				if (componentFormInTargetPanel != null && componentFormInTargetPanel != this)
					this.Hide();                //If the target panel hosts another ComponentForm, we won't override it
				else                            //This is most likely going to happen if a VMD ComponentForm was changed to a window, 
					AnchorToPanel(targetPanel); //then another VMD tool was selected and that window was closed
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
					return;

				while (!(sender is ComponentForm))
				{
					Control c = (Control)sender;
					sender = c.Parent;
					e = new MouseEventArgs(e.Button, e.Clicks, e.X + c.Location.X, e.Y + c.Location.Y, e.Delta);
				}

				if (popoutAllowed && e.Button == MouseButtons.Right && (sender as ComponentForm).FormBorderStyle == FormBorderStyle.None)
				{
					Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
					ContextMenuStrip columnsMenu = new ContextMenuStrip();
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
				string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
				string groupSeparator = numberFormatInfo.NumberGroupSeparator;
				string negativeSign = numberFormatInfo.NegativeSign;

				string keyInput = e.KeyChar.ToString();

				if (Char.IsDigit(e.KeyChar))
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

		public interface INumberBox
		{
			bool Nullable { get; }
			int? ToRawInt();
			void SetFromRawInt(int? rawint);
		}

		public class HexTextBox : TextBox, INumberBox
		{
			private string _addressFormatStr = "";
			private long? _maxSize;
			private bool _nullable = true;

			public HexTextBox()
			{
				CharacterCasing = CharacterCasing.Upper;
			}

			public bool Nullable { get { return _nullable; } set { _nullable = value; } }

			public void SetHexProperties(long domainSize)
			{
				bool wasMaxSizeSet = _maxSize.HasValue;
				int currMaxLength = MaxLength;

				_maxSize = domainSize - 1;

				MaxLength = _maxSize.Value.NumHexDigits();
				_addressFormatStr = "{0:X" + MaxLength + "}";

				//try to preserve the old value, as best we can
				if (!wasMaxSizeSet)
					ResetText();
				else if (_nullable)
					Text = "";
				else if (MaxLength != currMaxLength)
				{
					long? value = ToLong();
					if (value.HasValue)
						value = value.Value & ((1L << (MaxLength * 4)) - 1);
					else value = 0;
					Text = string.Format(_addressFormatStr, value.Value);
				}
			}

			public long GetMax()
			{
				if (_maxSize.HasValue)
				{
					return _maxSize.Value;
				}

				return ((long)1 << (4 * MaxLength)) - 1;
			}

			public override void ResetText()
			{
				Text = _nullable ? "" : string.Format(_addressFormatStr, 0);
			}

			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				if (e.KeyChar == '\b' || e.KeyChar == 22 || e.KeyChar == 1 || e.KeyChar == 3)
				{
					return;
				}

				if (!e.KeyChar.IsHex())
				{
					e.Handled = true;
				}
			}

			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Up)
				{
					if (Text.IsHex() && !string.IsNullOrEmpty(_addressFormatStr))
					{
						var val = (uint)ToRawInt();

						if (val == GetMax())
						{
							val = 0;
						}
						else
						{
							val++;
						}

						Text = string.Format(_addressFormatStr, val);
					}
				}
				else if (e.KeyCode == Keys.Down)
				{
					if (Text.IsHex() && !string.IsNullOrEmpty(_addressFormatStr))
					{
						var val = (uint)ToRawInt();
						if (val == 0)
						{
							val = (uint)GetMax(); // int to long todo
						}
						else
						{
							val--;
						}

						Text = string.Format(_addressFormatStr, val);
					}
				}
				else
				{
					base.OnKeyDown(e);
				}
			}

			protected override void OnTextChanged(EventArgs e)
			{
				if (string.IsNullOrWhiteSpace(Text))
				{
					ResetText();
					SelectAll();
					return;
				}

				base.OnTextChanged(e);
			}

			public int? ToRawInt()
			{
				if (string.IsNullOrWhiteSpace(Text))
				{
					if (Nullable)
					{
						return null;
					}

					return 0;
				}

				return int.Parse(Text, NumberStyles.HexNumber);
			}

			public void SetFromRawInt(int? val)
			{
				Text = val.HasValue ? string.Format(_addressFormatStr, val) : "";
			}

			public void SetFromLong(long val)
			{
				Text = string.Format(_addressFormatStr, val);
			}

			public long? ToLong()
			{
				if (string.IsNullOrWhiteSpace(Text))
				{
					if (Nullable)
					{
						return null;
					}

					return 0;
				}

				return long.Parse(Text, NumberStyles.HexNumber);
			}
		}

		public class UnsignedIntegerBox : TextBox, INumberBox
		{
			private bool _nullable = true;

			public UnsignedIntegerBox()
			{
				CharacterCasing = CharacterCasing.Upper;
			}

			public bool Nullable { get { return _nullable; } set { _nullable = value; } }

			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				if (e.KeyChar == '\b' || e.KeyChar == 22 || e.KeyChar == 1 || e.KeyChar == 3)
				{
					return;
				}

				if (!e.KeyChar.IsUnsigned())
				{
					e.Handled = true;
				}
			}

			public override void ResetText()
			{
				Text = _nullable ? "" : "0";
			}

			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Up)
				{
					if (Text.IsHex())
					{
						var val = (uint)ToRawInt();
						if (val == uint.MaxValue)
						{
							val = 0;
						}
						else
						{
							val++;
						}

						Text = val.ToString();
					}
				}
				else if (e.KeyCode == Keys.Down)
				{
					if (Text.IsHex())
					{
						var val = (uint)ToRawInt();

						if (val == 0)
						{
							val = uint.MaxValue;
						}
						else
						{
							val--;
						}

						Text = val.ToString();
					}
				}
				else
				{
					base.OnKeyDown(e);
				}
			}

			protected override void OnTextChanged(EventArgs e)
			{
				if (string.IsNullOrWhiteSpace(Text) || !Text.IsHex())
				{
					ResetText();
					SelectAll();
					return;
				}

				base.OnTextChanged(e);
			}

			public int? ToRawInt()
			{
				if (string.IsNullOrWhiteSpace(Text) || !Text.IsHex())
				{
					if (Nullable)
					{
						return null;
					}

					return 0;
				}

				return (int)uint.Parse(Text);
			}

			public void SetFromRawInt(int? val)
			{
				Text = val.HasValue ? val.ToString() : "";
			}
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
			get
			{
				return base.CellTemplate;
			}
			set
			{
				DataGridViewNumericUpDownCell dataGridViewNumericUpDownCell = value as DataGridViewNumericUpDownCell;
				if (value != null && dataGridViewNumericUpDownCell == null)
				{
					throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewNumericUpDownElements.DataGridViewNumericUpDownCell or derive from it.");
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>
		/// Replicates the DecimalPlaces property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Appearance"),
			DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces),
			Description("Indicates the number of decimal places to display.")
		]
		public int DecimalPlaces
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.DecimalPlaces;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				// Update the template cell so that subsequent cloned cells use the new value.
				this.NumericUpDownCellTemplate.DecimalPlaces = value;
				if (this.DataGridView != null)
				{
					// Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						// Be careful not to unshare rows unnecessarily.
						// This could have severe performance repercussions.
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						if (dataGridViewRow.Cells[this.Index] is DataGridViewNumericUpDownCell dataGridViewCell)
						{
							// Call the internal SetDecimalPlaces method instead of the property to avoid invalidation
							// of each cell. The whole column is invalidated later in a single operation for better performance.
							dataGridViewCell.SetDecimalPlaces(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
				}
			}
		}

		/// <summary>
		/// Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the amount to increment or decrement on each button click.")
		]
		public Decimal Increment
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
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
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

		/// Indicates whether the Increment property should be persisted.
		private bool ShouldSerializeIncrement()
		{
			return !this.Increment.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement);
		}

		/// <summary>
		/// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the maximum value for the numeric up-down cells."),
			RefreshProperties(RefreshProperties.All)
		]
		public Decimal Maximum
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
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
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

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeMaximum()
		{
			return !this.Maximum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum);
		}

		/// <summary>
		/// Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the minimum value for the numeric up-down cells."),
			RefreshProperties(RefreshProperties.All)
		]
		public Decimal Minimum
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
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
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

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeMinimum()
		{
			return !this.Minimum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum);
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
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
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
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
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

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeHexadecimal()
		{
			return !this.Hexadecimal.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal);
		}

		/// <summary>
		/// Small utility function that returns the template cell as a DataGridViewNumericUpDownCell
		/// </summary>
		private DataGridViewNumericUpDownCell NumericUpDownCellTemplate
		{
			get
			{
				return (DataGridViewNumericUpDownCell)this.CellTemplate;
			}
		}

		/// <summary>
		/// Returns a standard compact string representation of the column.
		/// </summary>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(100);
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
		private static readonly DataGridViewContentAlignment anyRight = DataGridViewContentAlignment.TopRight |
																		DataGridViewContentAlignment.MiddleRight |
																		DataGridViewContentAlignment.BottomRight;

		private static readonly DataGridViewContentAlignment anyCenter = DataGridViewContentAlignment.TopCenter |
																		 DataGridViewContentAlignment.MiddleCenter |
																		 DataGridViewContentAlignment.BottomCenter;

		// Default dimensions of the static rendering bitmap used for the painting of the non-edited cells
		private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth = 100;

		private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight = 22;

		// Default value of the DecimalPlaces property
		internal const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces = 0;

		// Default value of the Increment property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement = Decimal.One;

		// Default value of the Maximum property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum = (Decimal)100.0;

		// Default value of the Minimum property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum = Decimal.Zero;

		// Default value of the ThousandsSeparator property
		internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator = false;

		internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal = false;

		// Type of this cell's editing control
		private static Type defaultEditType = typeof(DataGridViewNumericUpDownEditingControl);

		// Type of this cell's value. The formatted value type is string, the same as the base class DataGridViewTextBoxCell
		private static Type defaultValueType = typeof(System.String);

		// The bitmap used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
		[ThreadStatic]
		private static Bitmap renderingBitmap;

		// The NumericUpDown control used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
		[ThreadStatic]
		private NumericUpDownHexFix paintingNumericUpDown;

		private int decimalPlaces;       // Caches the value of the DecimalPlaces property
		private Decimal increment;       // Caches the value of the Increment property
		private Decimal minimum;         // Caches the value of the Minimum property
		private Decimal maximum;         // Caches the value of the Maximum property
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
					Maximum = Decimal.MaxValue / 10,
					Minimum = Decimal.MinValue / 10,
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
			get
			{
				return this.decimalPlaces;
			}

			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("The DecimalPlaces property cannot be smaller than 0 or larger than 99.");
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
		private DataGridViewNumericUpDownEditingControl EditingNumericUpDown
		{
			get
			{
				return this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;
			}
		}

		/// <summary>
		/// Define the type of the cell's editing control
		/// </summary>
		public override Type EditType
		{
			get
			{
				return defaultEditType; // the type is DataGridViewNumericUpDownEditingControl
			}
		}

		/// <summary>
		/// The Increment property replicates the one from the NumericUpDown control
		/// </summary>
		public Decimal Increment
		{
			get
			{
				return this.increment;
			}

			set
			{
				if (value < (Decimal)0.0)
				{
					throw new ArgumentOutOfRangeException("The Increment property cannot be smaller than 0.");
				}
				SetIncrement(this.RowIndex, value);
				// No call to OnCommonChange is needed since the increment value does not affect the rendering of the cell.
			}
		}

		/// <summary>
		/// The Maximum property replicates the one from the NumericUpDown control
		/// </summary>
		public Decimal Maximum
		{
			get
			{
				return this.maximum;
			}

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
		public Decimal Minimum
		{
			get
			{
				return this.minimum;
			}

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
			get
			{
				return this.thousandsSeparator;
			}

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
			get
			{
				return this.hexadecimal;
			}

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
			DataGridViewNumericUpDownCell dataGridViewCell = base.Clone() as DataGridViewNumericUpDownCell;
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
		private Decimal Constrain(Decimal value)
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
			int preferredHeight = cellStyle.Font.Height + 3;
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
				UInt64 valueuint64 = System.Convert.ToUInt64(value);
				return valueuint64.ToString("X");
			}
			else
			{
				// By default, the base implementation converts the Decimal 1234.5 into the string "1234.5"
				object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
				string formattedNumber = formattedValue as string;
				if (!string.IsNullOrEmpty(formattedNumber) && value != null)
				{
					Decimal unformattedDecimal = System.Convert.ToDecimal(value);
					Decimal formattedDecimal = System.Convert.ToDecimal(formattedNumber);
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
			string negativeSignStr = numberFormatInfo.NegativeSign;
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
			bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
			bool cellEdited = cellCurrent && this.DataGridView.EditingControl != null;

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

					bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;

					/*if (renderingBitmap.Width < valBounds.Width ||
						renderingBitmap.Height < valBounds.Height)
					{
						// The static bitmap is too small, a bigger one needs to be allocated.
						renderingBitmap.Dispose();
						renderingBitmap = new Bitmap(valBounds.Width, valBounds.Height);
					}*/

					//7/1/2018
					//OPTIMIZE PAINTING BY REMOVING UNUSED FUNCTIONALITY
					//IF ANY OF THESE FUNCTIONS ARE USED, THEY NEED TO BE RE-ENABLED

					// Make sure the NumericUpDown control is parented to a visible control
					/*
					if (paintingNumericUpDown.Parent == null || !paintingNumericUpDown.Parent.Visible)
					{
						paintingNumericUpDown.Parent = this.DataGridView;
					}
					paintingNumericUpDown.RightToLeft = this.DataGridView.RightToLeft;
					paintingNumericUpDown.ThousandsSeparator = this.ThousandsSeparator;
					paintingNumericUpDown.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(cellStyle.Alignment);
					paintingNumericUpDown.DecimalPlaces = this.DecimalPlaces;*/

					// Set all the relevant properties
					paintingNumericUpDown.Value = Convert.ToDecimal(value);
					paintingNumericUpDown.Hexadecimal = this.Hexadecimal;
					paintingNumericUpDown.Font = cellStyle.Font;
					paintingNumericUpDown.Width = valBounds.Width;
					paintingNumericUpDown.Height = valBounds.Height;
					paintingNumericUpDown.Location = new Point(0, -paintingNumericUpDown.Height - 100);

					Color foreColor;
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
		internal void SetIncrement(int rowIndex, Decimal value)
		{
			Debug.Assert(value >= (Decimal)0.0);
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
		internal void SetMaximum(int rowIndex, Decimal value)
		{
			this.maximum = value;
			if (this.minimum > this.maximum)
			{
				this.minimum = this.maximum;
			}
			object cellValue = GetValue(rowIndex);
			if (cellValue != null)
			{
				Decimal currentValue = System.Convert.ToDecimal(cellValue);
				Decimal constrainedValue = Constrain(currentValue);
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
		internal void SetMinimum(int rowIndex, Decimal value)
		{
			this.minimum = value;
			if (this.minimum > this.maximum)
			{
				this.maximum = value;
			}
			object cellValue = GetValue(rowIndex);
			if (cellValue != null)
			{
				if (Hexadecimal)
				{
					Decimal currentValue = System.Convert.ToDecimal(cellValue);
					Decimal constrainedValue = Constrain(currentValue);
					if (constrainedValue != currentValue)
					{
						SetValue(rowIndex, constrainedValue);
					}
				}
				else
				{
					Decimal currentValue = System.Convert.ToDecimal(cellValue);
					Decimal constrainedValue = Constrain(currentValue);
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
			get
			{
				return this.dataGridView;
			}
			set
			{
				this.dataGridView = value;
			}
		}

		/// <summary>
		/// Property which represents the current formatted value of the editing control
		/// </summary>
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				this.Text = (string)value;
			}
		}

		/// <summary>
		/// Property which represents the row in which the editing control resides
		/// </summary>
		public virtual int EditingControlRowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				this.rowIndex = value;
			}
		}

		/// <summary>
		/// Property which indicates whether the value of the editing control has changed or not
		/// </summary>
		public virtual bool EditingControlValueChanged
		{
			get
			{
				return this.valueChanged;
			}
			set
			{
				this.valueChanged = value;
			}
		}

		/// <summary>
		/// Property which determines which cursor must be used for the editing panel,
		/// i.e. the parent of the editing control.
		/// </summary>
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		/// <summary>
		/// Property which indicates whether the editing control needs to be repositioned
		/// when its value changes.
		/// </summary>
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

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
				Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
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


		/*
		/// <summary>
		/// Listen to the KeyPress notification to know when the value changed, and
		/// notify the grid of the change.
		/// </summary>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			// The value changes when a digit, the decimal separator, the group separator or
			// the negative sign is pressed.
			bool notifyValueChange = false;
			if (char.IsDigit(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar == '\b' || (e.KeyChar == (char)Keys.ControlKey && e.KeyChar == (char)Keys.V)))
			{
				notifyValueChange = true;
			}
			else
			{
				System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
				string decimalSeparatorStr = numberFormatInfo.NumberDecimalSeparator;
				string groupSeparatorStr = numberFormatInfo.NumberGroupSeparator;
				string negativeSignStr = numberFormatInfo.NegativeSign;
				if (!string.IsNullOrEmpty(decimalSeparatorStr) && decimalSeparatorStr.Length == 1)
				{
					notifyValueChange = decimalSeparatorStr[0] == e.KeyChar;
				}
				if (!notifyValueChange && !string.IsNullOrEmpty(groupSeparatorStr) && groupSeparatorStr.Length == 1)
				{
					notifyValueChange = groupSeparatorStr[0] == e.KeyChar;
				}
				if (!notifyValueChange && !string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
				{
					notifyValueChange = negativeSignStr[0] == e.KeyChar;
				}
			}

			if (notifyValueChange)
			{
				// Let the DataGridView know about the value change
				NotifyDataGridViewOfValueChange();
			}
		}
		*/

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

	//Enables the doublebuffered flag for DGVs
	public static class ExtensionMethods
	{
		public static void DoubleBuffered(this DataGridView dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
			pi.SetValue(dgv, setting, null);
		}

		/*public static void DoubleBuffered(this NumericUpDownHexFix updown, bool setting)
		{
			Type updownType = updown.GetType();
			PropertyInfo pi = updownType.GetProperty("DoubleBuffered",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
			pi.SetValue(updown, setting, null);
		}*/
	}

	//Fixes microsoft's numericupdown hex issues. Thanks microsoft
	public class NumericUpDownHexFix : NumericUpDown
	{
		bool currentValueChanged = false;
		public NumericUpDownHexFix()
		{
			base.Minimum = 0;
			base.Maximum = UInt64.MaxValue;
			this.ValueChanged += OnValueChanged;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new decimal Maximum
		{    // Doesn't serialize properly
			get { return base.Maximum; }
			set { base.Maximum = value; }
		}

		private void OnValueChanged(object sender, EventArgs e)
		{
			currentValueChanged = true;
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			HandledMouseEventArgs hme = e as HandledMouseEventArgs;
			if (hme != null)
				hme.Handled = true;

			if (e.Delta > 0 && this.Value < this.Maximum)
				this.Value += this.Increment;
			else if (e.Delta < 0 && this.Value > this.Minimum)
				this.Value -= this.Increment;
		}

		protected override void UpdateEditText()
		{
			if (UserEdit)
				if (base.Hexadecimal)
					HexParseEditText();
				else
					ParseEditText();
			if (currentValueChanged || (!string.IsNullOrEmpty(Text) && !(Text.Length == 1 && Text == "-")))
			{
				currentValueChanged = false;
				ChangingText = true;
				Text = GetNumberText(Value);
				ChangingText = false;
			}

			//	if(base.Hexadecimal)
			//	base.Text = GetNumberText(base.Value);

		}

		protected override void ValidateEditText()
		{
			if (base.Hexadecimal)
				HexParseEditText();
			else
				ParseEditText();
			UpdateEditText();
		}

		private string GetNumberText(decimal num)
		{
			string text;

			if (Hexadecimal)
			{
				text = ((Int64)num).ToString("X", CultureInfo.InvariantCulture);
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
					var val = Convert.ToDecimal(Convert.ToInt64(base.Text, 16));

					if (val > Maximum)
					{
						base.Text = string.Format("{0:X}", (uint)Maximum);
						//	val = (uint)Maximum;
					}

					if (!string.IsNullOrEmpty(base.Text))
						this.Value = val;
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
					selectedRows.ForEach(row => row.Selected = true);

				// Select a range of new rows, if shift key is down
				if ((Control.ModifierKeys & Keys.Shift) != 0)
					for (int i = currentRow; i != e.RowIndex; i += Math.Sign(e.RowIndex - currentRow))
						this.Rows[i].Selected = true;
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
					// Reset the rectangle if the mouse is not over an item in the datagridview.
					_dragBoxFromMouseDown = Rectangle.Empty;
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
						base.OnMouseDown(e);
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
							rowIndexOfItemUnderMouseToDrop = 0;
						else
						{
							int temp = this.Rows.Count;
							if (temp >= 0)
								rowIndexOfItemUnderMouseToDrop = temp;
							else
							{
								rowIndexOfItemUnderMouseToDrop = 0;
							}

						}
					}

					//We InsertRange rather than inserting in the iterator so we don't have to deal with the edge case of moving two items up by one position goofing the indexes
					this.Rows.InsertRange(rowIndexOfItemUnderMouseToDrop, _rows);

					//Re-select the new rows
					this.ClearSelection();
					for (int i = rowIndexOfItemUnderMouseToDrop; i < (rowIndexOfItemUnderMouseToDrop + _rows.Length); i++)
					{
						this.Rows[i].Selected = true;
					}
				}

			}
		}
		private new void DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
			int headeroffset = this.Top + this.ColumnHeadersHeight;

			Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

			if (clientPoint.Y < headeroffset && FirstDisplayedScrollingRowIndex > 0)
			{
				this.FirstDisplayedScrollingRowIndex -= 1;
			}
			else if (clientPoint.Y > this.Bottom - 60)
			{
				this.FirstDisplayedScrollingRowIndex += 1;
			}
			//Cursor.Position = this.PointToScreen(new Point(Cursor.Position.X, this.Top + this.ColumnHeadersHeight));
		}
	}

}

public static class JsonHelper
{
	public static void Serialize(object value, Stream s, Formatting f = Formatting.Indented)
	{
		using (StreamWriter writer = new StreamWriter(s))
		using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
		{
			JsonSerializer ser = new JsonSerializer
			{
				Formatting = f
			};
			ser.Serialize(jsonWriter, value);
			jsonWriter.Flush();
		}
	}

	public static T Deserialize<T>(Stream s)
	{
		using (StreamReader reader = new StreamReader(s))
		using (JsonTextReader jsonReader = new JsonTextReader(reader))
		{
			JsonSerializer ser = new JsonSerializer();
			return ser.Deserialize<T>(jsonReader);
		}
	}
}


//From Bizhawk
public static class NumberExtensions
{
	public static string ToHexString(this int n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static string ToHexString(this uint n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static string ToHexString(this byte n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static string ToHexString(this ushort n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static string ToHexString(this long n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static string ToHexString(this ulong n, int numdigits)
	{
		return string.Format("{0:X" + numdigits + "}", n);
	}

	public static bool Bit(this byte b, int index)
	{
		return (b & (1 << index)) != 0;
	}

	public static bool Bit(this int b, int index)
	{
		return (b & (1 << index)) != 0;
	}

	public static bool Bit(this ushort b, int index)
	{
		return (b & (1 << index)) != 0;
	}

	public static bool In(this int i, params int[] options)
	{
		return options.Any(j => i == j);
	}

	public static byte BinToBCD(this byte v)
	{
		return (byte)(((v / 10) * 16) + (v % 10));
	}

	public static byte BCDtoBin(this byte v)
	{
		return (byte)(((v / 16) * 10) + (v % 16));
	}

	/// <summary>
	/// Receives a number and returns the number of hexadecimal digits it is
	/// Note: currently only returns 2, 4, 6, or 8
	/// </summary>
	public static int NumHexDigits(this long i)
	{
		// now this is a bit of a trick. if it was less than 0, it mustve been >= 0x80000000 and so takes all 8 digits
		if (i < 0)
		{
			return 8;
		}

		if (i < 0x100)
		{
			return 2;
		}

		if (i < 0x10000)
		{
			return 4;
		}

		if (i < 0x1000000)
		{
			return 6;
		}

		if (i < 0x100000000)
		{
			return 8;
		}

		return 16;
	}

	/// <summary>
	/// The % operator is a remainder operator. (e.g. -1 mod 4 returns -1, not 3.)
	/// </summary>
	public static int Mod(this int a, int b)
	{
		return a - (b * (int)Math.Floor((float)a / b));
	}

	/// <summary>
	/// Force the value to be strictly between min and max (both exclued)
	/// </summary>
	/// <typeparam name="T">Anything that implements <see cref="IComparable{T}"/></typeparam>
	/// <param name="val">Value that will be clamped</param>
	/// <param name="min">Minimum allowed</param>
	/// <param name="max">Maximum allowed</param>
	/// <returns>The value if strictly between min and max; otherwise min (or max depending of what is passed)</returns>
	public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
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
public static class StringExtensions
{
	public static string GetPrecedingString(this string str, string value)
	{
		var index = str.IndexOf(value);

		if (index < 0)
		{
			return null;
		}

		if (index == 0)
		{
			return "";
		}

		return str.Substring(0, index);
	}

	public static bool IsValidRomExtentsion(this string str, params string[] romExtensions)
	{
		var strUpper = str.ToUpper();
		return romExtensions.Any(ext => strUpper.EndsWith(ext.ToUpper()));
	}

	public static bool In(this string str, params string[] options)
	{
		return options.Any(opt => opt.Equals(str, StringComparison.CurrentCultureIgnoreCase));
	}

	public static bool In(this string str, IEnumerable<string> options)
	{
		return options.Any(opt => opt.Equals(str, StringComparison.CurrentCultureIgnoreCase));
	}

	public static bool In<T>(this string str, IEnumerable<T> options, Func<T, string, bool> eval)
	{
		return options.Any(opt => eval(opt, str));
	}

	public static bool NotIn(this string str, params string[] options)
	{
		return options.All(opt => opt.ToLower() != str.ToLower());
	}

	public static bool NotIn(this string str, IEnumerable<string> options)
	{
		return options.All(opt => opt.ToLower() != str.ToLower());
	}

	public static int HowMany(this string str, char c)
	{
		return !string.IsNullOrEmpty(str) ? str.Count(t => t == c) : 0;
	}

	public static int HowMany(this string str, string s)
	{
		if (str == null)
		{
			return 0;
		}

		var count = 0;
		for (var i = 0; i < (str.Length - s.Length); i++)
		{
			if (str.Substring(i, s.Length) == s)
			{
				count++;
			}
		}

		return count;
	}

	#region String and Char validation extensions

	/// <summary>
	/// Validates all chars are 0-9
	/// </summary>
	public static bool IsUnsigned(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str.All(IsUnsigned);
	}

	/// <summary>
	/// Validates the char is 0-9
	/// </summary>
	public static bool IsUnsigned(this char c)
	{
		return char.IsDigit(c);
	}

	/// <summary>
	/// Validates all chars are 0-9, or a dash as the first value
	/// </summary>
	public static bool IsSigned(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str[0].IsSigned() && str.Substring(1).All(IsUnsigned);
	}

	/// <summary>
	/// Validates the char is 0-9 or a dash
	/// </summary>
	public static bool IsSigned(this char c)
	{
		return char.IsDigit(c) || c == '-';
	}

	/// <summary>
	/// Validates all chars are 0-9, A-F or a-f
	/// </summary>
	public static bool IsHex(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str.All(IsHex);
	}

	/// <summary>
	/// Validates the char is 0-9, A-F or a-f
	/// </summary>
	public static bool IsHex(this char c)
	{
		if (char.IsDigit(c))
		{
			return true;
		}

		return char.ToUpperInvariant(c) >= 'A' && char.ToUpperInvariant(c) <= 'F';
	}

	/// <summary>
	/// Validates all chars are 0 or 1
	/// </summary>
	public static bool IsBinary(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str.All(IsBinary);
	}

	/// <summary>
	/// Validates the char is 0 or 1
	/// </summary>
	public static bool IsBinary(this char c)
	{
		return c == '0' || c == '1';
	}

	/// <summary>
	/// Validates all chars are 0-9, a decimal point, and that there is no more than 1 decimal point, can not be signed
	/// </summary>
	public static bool IsFixedPoint(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str.HowMany('.') <= 1
			&& str.All(IsFixedPoint);
	}

	/// <summary>
	/// Validates the char is 0-9, a dash, or a decimal
	/// </summary>
	public static bool IsFixedPoint(this char c)
	{
		return c.IsUnsigned() || c == '.';
	}

	/// <summary>
	/// Validates all chars are 0-9 or decimal, and that there is no more than 1 decimal point, a dash can be the first character
	/// </summary>
	public static bool IsFloat(this string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}

		return str.HowMany('.') <= 1
			&& str[0].IsFloat()
			&& str.Substring(1).All(IsFixedPoint);
	}

	/// <summary>
	/// Validates that the char is 0-9, a dash, or a decimal point
	/// </summary>
	public static bool IsFloat(this char c)
	{
		return c.IsFixedPoint() || c == '-';
	}

	/// <summary>
	/// Takes any string and removes any value that is not a valid binary value (0 or 1)
	/// </summary>
	public static string OnlyBinary(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder();

		foreach (var chr in raw)
		{
			if (IsBinary(chr))
			{
				output.Append(chr);
			}
		}

		return output.ToString();
	}

	/// <summary>
	/// Takes any string and removes any value that is not a valid unsigned integer value (0-9)
	/// </summary>
	public static string OnlyUnsigned(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder();

		foreach (var chr in raw)
		{
			if (IsUnsigned(chr))
			{
				output.Append(chr);
			}
		}

		return output.ToString();
	}

	/// <summary>
	/// Takes any string and removes any value that is not a valid unsigned integer value (0-9 or -)
	/// Note: a "-" will only be kept if it is the first digit
	/// </summary>
	public static string OnlySigned(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder();

		int count = 0;
		foreach (var chr in raw)
		{
			if (count == 0 && chr == '-')
			{
				output.Append(chr);
			}
			else if (IsUnsigned(chr))
			{
				output.Append(chr);
			}

			count++;
		}

		return output.ToString();
	}

	/// <summary>
	/// Takes any string and removes any value that is not a valid hex value (0-9, a-f, A-F), returns the remaining characters in uppercase
	/// </summary>
	public static string OnlyHex(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder(raw.Length);

		foreach (var chr in raw)
		{
			if (IsHex(chr))
			{
				output.Append(char.ToUpper(chr));
			}
		}

		return output.ToString();
	}

	/// <summary>
	/// Takes any string and removes any value that is not a fixed point value (0-9 or .)
	/// Note: only the first occurrence of a . will be kept
	/// </summary>
	public static string OnlyFixedPoint(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder();

		var usedDot = false;
		foreach (var chr in raw)
		{
			if (chr == '.')
			{
				if (usedDot)
				{
					continue;
				}

				usedDot = true;
			}

			if (IsFixedPoint(chr))
			{
				output.Append(chr);
			}
		}

		return output.ToString();
	}

	/// <summary>
	/// Takes any string and removes any value that is not a float point value (0-9, -, or .)
	/// Note: - is only valid as the first character, and only the first occurrence of a . will be kept
	/// </summary>
	public static string OnlyFloat(this string raw)
	{
		if (string.IsNullOrWhiteSpace(raw))
		{
			return "";
		}

		var output = new StringBuilder();

		var usedDot = false;
		var count = 0;
		foreach (var chr in raw)
		{
			if (count == 0 && chr == '-')
			{
				output.Append(chr);
			}
			else
			{
				if (chr == '.')
				{
					if (usedDot)
					{
						continue;
					}

					usedDot = true;
				}

				if (IsFixedPoint(chr))
				{
					output.Append(chr);
				}
			}

			count++;
		}

		return output.ToString();
	}

	#endregion
}
// Used code from this https://github.com/wasabii/Cogito/blob/master/Cogito.Core/RandomExtensions.cs
// MIT Licensed. thank you very much.
internal static class RandomExtensions
{
	public static long RandomLong(this Random rnd)
	{
		byte[] buffer = new byte[8];
		rnd.NextBytes(buffer);
		return BitConverter.ToInt64(buffer, 0);
	}

	public static long RandomLong(this Random rnd, long min, long max)
	{
		EnsureMinLEQMax(ref min, ref max);
		long numbersInRange = unchecked(max - min + 1);
		if (numbersInRange < 0)
			throw new ArgumentException(
				"Size of range between min and max must be less than or equal to Int64.MaxValue");

		long randomOffset = RandomLong(rnd);
		if (IsModuloBiased(randomOffset, numbersInRange))
			return RandomLong(rnd, min, max); // Try again
		return min + PositiveModuloOrZero(randomOffset, numbersInRange);
	}

	public static long RandomLong(this Random rnd, long max)
	{
		return rnd.RandomLong(0, max);
	}

	private static bool IsModuloBiased(long randomOffset, long numbersInRange)
	{
		long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
		return randomOffset > greatestCompleteRange;
	}

	private static long PositiveModuloOrZero(long dividend, long divisor)
	{
		Math.DivRem(dividend, divisor, out long mod);
		if (mod < 0)
			mod += divisor;
		return mod;
	}

	private static void EnsureMinLEQMax(ref long min, ref long max)
	{
		if (min <= max)
			return;
		long temp = min;
		min = max;
		max = temp;
	}
}

/// <summary>
/// Provides a generic collection that supports data binding and additionally supports sorting.
/// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
/// If the elements are IComparable it uses that; otherwise compares the ToString()
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class SortableBindingList<T> : BindingList<T> where T : class
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
	protected override bool SupportsSortingCore
	{
		get { return true; }
	}

	/// <summary>
	/// Gets a value indicating whether the list is sorted.
	/// </summary>
	protected override bool IsSortedCore
	{
		get { return _isSorted; }
	}

	/// <summary>
	/// Gets the direction the list is sorted.
	/// </summary>
	protected override ListSortDirection SortDirectionCore
	{
		get { return _sortDirection; }
	}

	/// <summary>
	/// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
	/// </summary>
	protected override PropertyDescriptor SortPropertyCore
	{
		get { return _sortProperty; }
	}

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
		if (list == null) return;

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
			result = -result;
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
