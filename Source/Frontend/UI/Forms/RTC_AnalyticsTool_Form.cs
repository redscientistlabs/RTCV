using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using System.Threading.Tasks;
using RTCV.Common;

namespace RTCV.UI
{
	public partial class RTC_AnalyticsTool_Form : Form, IAutoColorize
	{

        public BlastLayer originalBlastLayer = null;
        public volatile List<MemoryDump> MemoryDumps = new List<MemoryDump>();


		public RTC_AnalyticsTool_Form()
		{
			try
			{
				InitializeComponent();

			}
			catch(Exception ex)
			{
				string additionalInfo = "An error occurred while opening the AnalyticsTool Form\n\n";

				var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

				if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

			}
		}


        public static void OpenAnalyticsTool(List<string> memoryDumps)
        {
            //S.GET<RTC_AnalyticsTool_Form>().Close();
            var atf = new RTC_AnalyticsTool_Form();
            S.SET(atf);

            atf.AddMemoryDumps(memoryDumps);

            atf.Show();
        }

        private void RTC_AnalyticsToolForm_Load(object sender, EventArgs e)
		{
			UICore.SetRTCColor(UICore.GeneralColor, this);

		}

        private void RTC_SanitizeTool_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            Form frm = (sender as Form);
            Button check = (frm?.ActiveControl as Button);

            if(check == null && lbDumps.Items.Count > 1)
            {

            }

        }



        public void AddMemoryDumps(List<string> memoryDumps)
        {
            MemoryDumps.AddRange(memoryDumps.Select(it => new MemoryDump(it)));

            lbDumps.DataSource = null;
            lbDumps.SelectedIndex = -1;
            lbDumps.DataSource = MemoryDumps;

        }

        private void btnCalculateActivity_Click(object sender, EventArgs e)
        {
            CalculateActivity();
        }

        public class CrunchRequest
        {
            public List<MemoryDump> data;
            public int order;
            public long startAddress;
            public long nmAdressesToCrunch;
        }

        private (int order, double[] output) CrunchActivity(CrunchRequest cr)
        {
            try
            {

                double[] output = new double[cr.nmAdressesToCrunch];
                int i = 0;
                for (long addr = cr.startAddress; addr < (cr.startAddress + cr.nmAdressesToCrunch); addr++)
                {
                    int resolution = cr.data.Count() - 1;
                    int changes = 0;

                    for (int layer = 1; layer < cr.data.Count(); layer++)
                    {
                        byte prevByte = cr.data[layer - 1].Dump.PeekByte(addr);
                        byte currentByte = cr.data[layer].Dump.PeekByte(addr);

                        if (currentByte != prevByte)
                            changes++;
                    }

                    output[i] = (Convert.ToDouble(changes) / Convert.ToDouble(resolution));
                    i++;
                }

                return (cr.order, output);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<double> CalculateActivity()
        {
            //preload dumps
            foreach (var item in MemoryDumps)
                item.CacheDump();

            long dumpSize = MemoryDumps[0].Dump.GetSize();
            var cpus = Environment.ProcessorCount;

            if (cpus == 1)
            {
                CrunchRequest cr = new CrunchRequest()
                {
                    data = MemoryDumps,
                    order = 0,
                    startAddress = 0,
                    nmAdressesToCrunch = dumpSize,
                };

                return CrunchActivity(cr).output;
            }
            else
            {

                long startAddress = 0;
                long reminder = dumpSize % (cpus - 1);
                long splitSize = (dumpSize - reminder) / (cpus - 1);

                Task<(int order, double[] output)>[] tasks = new Task<(int order, double[] output)>[cpus];
                for (int i = 0; i < cpus; i++)
                {
                    long requestedSize = splitSize;

                    if (i == 0 && reminder != 0)
                        requestedSize = reminder;

                    CrunchRequest cr = new CrunchRequest
                    {
                        data = MemoryDumps,
                        order = i,
                        startAddress = startAddress,
                        nmAdressesToCrunch = requestedSize,
                    };

                    tasks[i] = new TaskFactory().StartNew((arg) =>
                        CrunchActivity((arg as CrunchRequest)),cr);

                    startAddress += requestedSize;
                }

                Task.WaitAll(tasks);

                var orderedOutputs = tasks.Select(it => it.Result).OrderBy(it => it.order).Select(it => it.output);

                IEnumerable<double> masterArray = new double[] { };

                foreach (var output in orderedOutputs)
                    masterArray = masterArray.Concat(output);

                return masterArray;

            }
        }
    }

    public class MemoryDump
    {
        string key;
        FileInfo file;
        public DateTime Timestamp => file.CreationTime;

        volatile byte[][] dump = null;
        public byte[][] Dump
        {
            get
            {
                if (dump == null)
                    dump = MemoryBanks.ReadFile(file.FullName);

                return dump;
            }
        }

        public MemoryDump(string _key)
        {
            key = _key;
            file = new FileInfo(Path.Combine(CorruptCore.RtcCore.workingDir, "MEMORYDUMPS", key + ".dmp"));
        }

        public override string ToString()
        {
            return Timestamp.ToString();
        }

        internal void CacheDump()
        {
            var item = Dump;
        }
    }
}
