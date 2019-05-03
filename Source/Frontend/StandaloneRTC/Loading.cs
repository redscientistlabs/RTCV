using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
			//Create the RTC log next to the executable
			string rtcLogPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RTC", "RTC_LOG.txt");
            RTCV.NetCore.Extensions.ConsoleHelper.CreateConsole(rtcLogPath);
			if (args.Contains("-CONSOLE"))
			{
				RTCV.NetCore.Extensions.ConsoleHelper.ShowConsole();
			}
			else
			{
				RTCV.NetCore.Extensions.ConsoleHelper.HideConsole();
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
