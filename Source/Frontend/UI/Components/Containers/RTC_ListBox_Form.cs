using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public partial class RTC_ListBox_Form : ComponentForm
	{
		ComponentForm[] childForms;

		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_ListBox_Form(ComponentForm[] _childForms)
		{
			InitializeComponent();

			this.undockedSizable = false;

			childForms = _childForms;

			//Populate the filter ComboBox
			lbComponentForms.DisplayMember = "Name";
			lbComponentForms.ValueMember = "Value";

			foreach (var item in childForms)
				lbComponentForms.Items.Add(new ComboBoxItem<Form>(item.Text, item));
		}

		private void lbComponentForms_SelectedIndexChanged(object sender, EventArgs e)
		{
			((lbComponentForms.SelectedItem as ComboBoxItem<Form>)?.Value as ComponentForm)?.AnchorToPanel(pnTargetComponentForm);
		}

		private void RTC_ListBox_Form_Load(object sender, EventArgs e)
		{
			lbComponentForms.SelectedIndex = 0;
		}

		public void SetFocusedForm(ComponentForm form)
		{
			lbComponentForms.SelectedItem = lbComponentForms.Items.Cast<ComboBoxItem<Form>>().FirstOrDefault(x => x.Value == form);
		}
	}
}
