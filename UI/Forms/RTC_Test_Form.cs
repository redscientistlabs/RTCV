using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_Test_Form : Form, IAutoColorize
	{
		public RTC_Test_Form()
		{
			InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
		}
	}

	public class TestClass
	{
		public List<long[]> ListLongArr { get; set; } = new List<long[]>();
		public TestClass()
		{

		}
	}
}
