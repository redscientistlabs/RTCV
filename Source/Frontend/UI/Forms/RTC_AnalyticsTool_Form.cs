using System;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.Common;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RTCV.UI
{
    public partial class RTC_AnalyticsTool_Form : Form, IAutoColorize
    {
        public BlastLayer originalBlastLayer = null;
        private MemoryInterface MemoryInterface;
        private List<string> ActiveTableDumps;

        public RTC_AnalyticsTool_Form()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                string additionalInfo = "An error occurred while opening the SanitizeTool Form\n\n";

                var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

                if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
                {
                    throw new RTCV.NetCore.AbortEverythingException();
                }
            }
        }

        List<dynamic> DumpSource = new List<dynamic>();
        private int WordSize;

        public static void OpenAnalyticsTool(MemoryInterface mi, List<string> memoryDumpPaths)
        {
            S.GET<RTC_AnalyticsTool_Form>().Close();
            var stf = new RTC_AnalyticsTool_Form();
            S.SET(stf);

            stf.MemoryInterface = mi;
            stf.ActiveTableDumps = memoryDumpPaths;

            stf.DumpSource.Clear();

            foreach (var dump in memoryDumpPaths)
            {
                var fi = new FileInfo(dump);
                stf.DumpSource.Add(new { 
                    key = fi.Name, 
                    value = fi.FullName 
                });
            }

            stf.lbDumps.DataSource = null;
            stf.lbDumps.DataSource = stf.DumpSource;

            stf.btnSelectAllDumps_Click(null, null);

            stf.lbDomainSize.Text = $"Domain size: {mi.Size}";

            stf.WordSize = mi.WordSize;
            switch (mi.WordSize)
            {
                default:
                case 1:
                    stf.cbWordSize.SelectedIndex = 0;
                    break;
                case 2:
                    stf.cbWordSize.SelectedIndex = 1;
                    break;
                case 4:
                    stf.cbWordSize.SelectedIndex = 2;
                    break;
                case 8:
                    stf.cbWordSize.SelectedIndex = 3;
                    break;
            }

            stf.Show();
        }

        private void RTC_AnalyticsToolForm_Load(object sender, EventArgs e)
        {
            UICore.SetRTCColor(UICore.GeneralColor, this);
        }

        private void RTC_SanitizeTool_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            Form frm = (sender as Form);
            Button check = (frm?.ActiveControl as Button);

            if (check == null && lbDumps.Items.Count > 1)
            {
            }
        }

        private void btnSelectAllDumps_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbDumps.Items.Count; i++)
                lbDumps.SetSelected(i, true);
        }

        private void btnSelectNoDumps_Click(object sender, EventArgs e)
        {
            lbDumps.ClearSelected();
        }

        private void lbDumps_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbDumpsSelected.Text = $"Dumps selected: {lbDumps.SelectedItems.Count}";
        }

        private void cbWordSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbWordSize.SelectedIndex)
            {
                default:
                case 0:
                    WordSize = 1;
                    break;
                case 1:
                    WordSize = 2;
                    break;
                case 2:
                    WordSize = 4;
                    break;
                case 3:
                    WordSize = 8;
                    break;
            }
        }

        private void btnComputeActivity_Click(object sender, EventArgs e)
        {
            AnalyticsCube.Init();

            int nbDumps = lbDumps.SelectedItems.Count;
            int dumpSize = -1;
            int nbWords = -1;

            foreach (dynamic item in lbDumps.SelectedItems)
            {
                string filePath = item.value;

                byte[] dump = File.ReadAllBytes(filePath);

                if (dumpSize == -1)
                {
                    dumpSize = dump.Length;
                    nbWords = (dumpSize / WordSize);

                }

                AnalyticsCube.Push(dump, WordSize);

            }

            var cpus = Environment.ProcessorCount;
            int remainder = nbWords % cpus;

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < cpus; i++)
            {
                var task = new Task<int[]>(function: (cpu_i) =>
                {

                    int real_i = (int)cpu_i;

                    int nbWordsInASlice = ((nbWords - remainder) / cpus);
                    int nbWordsInThisOne = ((nbWords - remainder) / cpus) + (real_i == (cpus - 1) ? remainder : 0);

                    List<int> activity = new List<int>();

                    int maxActivity = 0;

                    for (int y = (nbWordsInASlice * real_i); y < (nbWordsInASlice * real_i) + nbWordsInThisOne; y++)
                    {
                        List<byte[]> stripe = new List<byte[]>();


                        for (int x = 0; x < AnalyticsCube.Cube.Count; x++)
                        {
                            var word = AnalyticsCube.GetWord(x, y);
                            stripe.Add(word);

                        }

                        int crunchedActivity = AnalyticsCube.CrunchChain(stripe);

                        if (maxActivity < crunchedActivity)
                            maxActivity = crunchedActivity;

                        activity.Add(crunchedActivity);

                    }

                    return activity.ToArray();

                }, state: i);

                tasks.Add(task);
                task.Start();

            }

            Task.WaitAll(tasks.ToArray());

            new object();

        }
    }

    public static class AnalyticsCube
    {
        public static List<List<byte[]>> Cube;

        internal static int CrunchChain(List<byte[]> stripe)
        {
            int activity = 0;

            for(int i=0;i<stripe.Count;i++)
            {
                if (i == 0)
                    continue;

                bool hasActivity = false;

                byte[] prevWord = stripe[i - 1];
                byte[] currentWord = stripe[i];

                for (int j=0;j< currentWord.Length; j++)
                {
                    if (prevWord[j] != currentWord[j])
                    {
                        hasActivity = true;
                        continue;
                    }
                }

                if (hasActivity)
                    activity++;
            }

            return activity;
        }

        internal static byte[] GetWord(int x, int y)
        {
            try
            {
                return Cube[x][y];
            }
            catch(Exception ex)
            {
                new object();
                return null;
            }
        }

        internal static void Init()
        {
            Cube = new List<List<byte[]>>();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        internal static void Push(byte[] dump, int wordSize)
        {
            if (Cube == null)
                Init();

            int nbWords = (dump.Length / wordSize);

            var Square = new List<byte[]>();

            for (int i = 0; i < nbWords; i++)
                Square.Add(getWord(dump, wordSize, i));

            Cube.Add(Square);
        }

        private static byte[] getWord(byte[] dump, int wordSize, int index)
        {
            byte[] output = new byte[wordSize];

            for(int i=0;i<wordSize;i++)
            {
                output[i] = dump[index + i];
            }

            return output;
        }
    }

}
