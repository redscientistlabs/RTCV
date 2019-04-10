using System;
using System.Linq;
using System.Windows.Forms;
using RTCV.UI;
using RTCV;

namespace StandaloneRTC
{
public partial class Loader : UI_Extensions.RTC_Standalone_Form
	{ 

		public Loader(string[] args)
		{
			if (RTCV.NetCore.Extensions.IsGDIEnhancedScalingAvailable())
			{
				RTCV.NetCore.Extensions.SetThreadDpiAwarenessContext(RTCV.NetCore.Extensions.DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED);
				Application.EnableVisualStyles();
			}

			InitializeComponent();
			RTCV.NetCore.Extensions.ConsoleHelper.CreateConsole("RTC\\RTC_LOG.txt");
			if (args.Contains("-CONSOLE"))
			{
				RTCV.NetCore.Extensions.ConsoleHelper.ShowConsole();
			}
			else
			{
				RTCV.NetCore.Extensions.ConsoleHelper.HideConsole();
			}

			if (args.Contains("-HIDESTARTBUTTON"))
			{
				UICore.HideStartButton = true;
			}
			UICore.Start(this);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}


		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			Visible = false;
		}
	}
}
