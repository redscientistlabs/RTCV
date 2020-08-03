namespace StandaloneRTC
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using RTCV.UI;
    using RTCV.NetCore.NetCore_Extensions;

    public partial class Loader : UI_Extensions.RTC_Standalone_Form
    {
        private static bool IsGDIEnhancedScalingAvailable() => (Environment.OSVersion.Version.Major == 10 &
                    Environment.OSVersion.Version.Build >= 17763);

        [DllImport("User32.dll")]
        public static extern DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(
            DPI_AWARENESS_CONTEXT dpiContext);

        public Loader(string[] args)
        {
            if (IsGDIEnhancedScalingAvailable())
            {
                SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED);
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
