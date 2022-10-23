namespace StandaloneRTC
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using RTCV.UI;
    using RTCV.NetCore.NetCoreExtensions;

    public partial class Loader : RTCV.UI.Forms.StandaloneForm
    {
        private static bool IsGDIEnhancedScalingAvailable() => (Environment.OSVersion.Version.Major == 10 &
                    Environment.OSVersion.Version.Build >= 17763);

        [DllImport("User32.dll")]
        public static extern DPIAwarenessContext SetThreadDpiAwarenessContext(
            DPIAwarenessContext dpiContext);

        public Loader(string[] args)
        {
            if (IsGDIEnhancedScalingAvailable())
            {
                SetThreadDpiAwarenessContext(DPIAwarenessContext.UnawareGDIScaled);
                Application.EnableVisualStyles();
            }

            InitializeComponent();
            //Create the RTC log next to the executable
            string rtcLogPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RTC", "RTC_LOG.txt");

            RTCV.Common.ConsoleHelper.CreateConsole();
            RTCV.Common.Logging.StartLogging(rtcLogPath);
            if (args.Contains("-CONSOLE"))
            {
                RTCV.Common.ConsoleHelper.ShowConsole();
            }
            else
            {
                RTCV.Common.ConsoleHelper.HideConsole();
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
