using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public partial class RTC_SelectBox_Form : ComponentForm
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		ComponentForm[] childForms;

		public RTC_SelectBox_Form(ComponentForm[] _childForms)
		{
			InitializeComponent();

			childForms = _childForms;

			cbSelectBox.DisplayMember = "text";
			cbSelectBox.ValueMember = "value";

			foreach (var item in childForms)
				cbSelectBox.Items.Add(new { text = item.Text, value = item });

		}

		private void cbSelectBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			((cbSelectBox.SelectedItem as dynamic).value as ComponentForm)?.AnchorToPanel(pnComponentForm);
		}

		private void RTC_SelectBox_Form_Load(object sender, EventArgs e)
		{
			cbSelectBox.SelectedIndex = 0;
		}
	}
}
