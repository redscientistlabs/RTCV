namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;

    public partial class AnalyticsToolForm : Modular.ColorizedForm
    {
        private MemoryInterface MemoryInterface;
        private List<string> ActiveTableDumps;

        public AnalyticsToolForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }
            }
        }

        List<dynamic> DumpSource = new List<dynamic>();
        private int WordSize;

        internal static void OpenAnalyticsTool(MemoryInterface mi, List<string> memoryDumpPaths)
        {
            S.GET<AnalyticsToolForm>().Close();
            var stf = new AnalyticsToolForm();
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

            stf.SelectAllDumps(null, null);

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

        private void OnFormClosing(object sender, FormClosingEventArgs e)
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

        private void SelectAllDumps(object sender, EventArgs e)
        {
            for (int i = 0; i < lbDumps.Items.Count; i++)
            {
                lbDumps.SetSelected(i, true);
            }
        }

        private void SelectNoDumps(object sender, EventArgs e)
        {
            lbDumps.ClearSelected();
        }

        private void UpdateSelectedDumps(object sender, EventArgs e)
        {
            lbDumpsSelected.Text = $"Dumps selected: {lbDumps.SelectedItems.Count}";
        }

        private void UpdateSelectedWordSize(object sender, EventArgs e)
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

        private void ComputeActivity(object sender, EventArgs e)
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
            int remainderWords = nbWords % cpus;

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < cpus; i++)
            {
                //tuple params: cpuid, activity array, max activity
                var task = new Task<(int, int[], int)>(function: (cpuI) =>
                {
                    int realI = (int)cpuI;

                    int nbWordsInASlice = ((nbWords - remainderWords) / cpus);
                    int nbWordsInThisOne = nbWordsInASlice + (realI == (cpus - 1) ? remainderWords : 0);

                    List<int> activity = new List<int>();
                    int maxActivity = 0;
                    int wordStartIndex = (nbWordsInASlice * realI);

                    for (int y = wordStartIndex; y < wordStartIndex + nbWordsInThisOne; y++)
                    {
                        List<byte[]> stripe = new List<byte[]>();

                        for (int x = 0; x < AnalyticsCube.Cube.Count; x++)
                        {
                            stripe.Add(AnalyticsCube.GetWord(x, y));
                        }

                        int crunchedActivity = AnalyticsCube.CrunchAlgoChain(stripe);

                        if (maxActivity < crunchedActivity)
                        {
                            maxActivity = crunchedActivity;
                        }

                        activity.Add(crunchedActivity);
                    }

                    return (realI, activity.ToArray(), maxActivity);
                }, state: i);

                tasks.Add(task);
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());
            var returns = tasks.Select(it => (it as Task<(int cpuI, int[] activity, int maxActivity)>).Result).OrderBy(it => it.cpuI).ToArray();

            int maxActivity = 0;
            List<int> fullActivity = new List<int>();

            foreach (var ret in returns)
            {
                if (maxActivity < ret.maxActivity)
                {
                    maxActivity = ret.maxActivity;
                }

                fullActivity.AddRange(ret.activity);
            }

            List<double> dumpsActivity = AnalyticsCube.CrunchFloatActivity(fullActivity, maxActivity);

            var more50 = dumpsActivity.Where(it => it > 0.5d).ToList();
            new object();
        }
    }

    public static class AnalyticsCube
    {
        public static List<List<byte[]>> Cube { get; private set; }

        internal static List<double> CrunchFloatActivity(List<int> fullActivity, int maxActivity)
        {
            List<double> output = new List<double>();

            foreach (var activity in fullActivity)
            {
                output.Add(Convert.ToDouble(activity) / Convert.ToDouble(maxActivity));
            }

            return output;
        }
        internal static int CrunchAlgoList(/*List<byte[]> stripe, string list*/)
        {
            return 0;
        }

        internal static int CrunchAlgoChain(List<byte[]> stripe)
        {
            int activity = 0;

            for (int i = 0; i < stripe.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                bool hasActivity = false;

                byte[] prevWord = stripe[i - 1];
                byte[] currentWord = stripe[i];

                for (int j = 0; j < currentWord.Length; j++)
                {
                    if (prevWord[j] != currentWord[j])
                    {
                        hasActivity = true;
                        continue;
                    }
                }

                if (hasActivity)
                {
                    activity++;
                }
            }

            return activity;
        }

        internal static byte[] GetWord(int x, int y)
        {
            try
            {
                return Cube[x][y];
            }
            #pragma warning disable CS0168
            catch (Exception ex)
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
            {
                Init();
            }

            int nbWords = (dump.Length / wordSize);
            var Square = new List<byte[]>();

            for (int i = 0; i < nbWords; i++)
            {
                Square.Add(getWord(dump, wordSize, i));
            }

            Cube.Add(Square);
        }

        private static byte[] getWord(byte[] dump, int wordSize, int index)
        {
            byte[] output = new byte[wordSize];

            for (int i = 0; i < wordSize; i++)
            {
                output[i] = dump[index + i];
            }

            return output;
        }
    }
}
