namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    public partial class VmdSimpleGenForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private long currentDomainSize = 0;

        public VmdSimpleGenForm()
        {
            InitializeComponent();
        }

        private void SelectAll(object sender, EventArgs e)
        {
            S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected();

            cbSelectedMemoryDomain.Items.Clear();
            var domains = MemoryDomains.MemoryInterfaces?.Keys.Where(it => !it.Contains("[V]")).ToArray();
            if (domains?.Length > 0)
            {
                cbSelectedMemoryDomain.Items.AddRange(domains);
            }

            if (cbSelectedMemoryDomain.Items.Count > 0)
            {
                cbSelectedMemoryDomain.SelectedIndex = 0;
            }
        }

        private void HandleSelectedMemoryDomainChange(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return;
            }

            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            lbDomainSizeValue.Text = "0x" + mi.Size.ToString("X");
            lbWordSizeValue.Text = $"{mi.WordSize * 8} bits";
            lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

            currentDomainSize = Convert.ToInt64(mi.Size);

            updateInterface();
        }

        private void updateInterface()
        {
            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            long fullRange = mi.Size;

            if (mtbStartAddress.Value > fullRange)
            {
                mtbStartAddress.Value = fullRange;
            }

            if (mtbStartAddress.Maximum != fullRange)
            {
                mtbStartAddress.Maximum = fullRange;
            }

            if (mtbRange.Maximum != fullRange)
            {
                mtbRange.Maximum = fullRange;
            }

            mtbStartAddress.Enabled = true;
            mtbRange.Enabled = true;
            btnGenerateVMD.Enabled = true;
        }

        private void HandleGenerateVMDClick(object sender, EventArgs e)
        {
            GenerateVMD();
        }

        public bool GenerateVMD(string textBoxContent)
        {
            if (textBoxContent == null)
            {
                throw new Exception("Can't generate a vmd without text");
            }

            tbRangeExpression.Text = textBoxContent;

            return GenerateVMD();
        }

        private bool GenerateVMD()
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(tbVmdName.Text) && MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return false;
            }

            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();
            VmdPrototype proto = new VmdPrototype
            {
                GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString(),
                UsingRPC = mi.UsingRPC,
            };

            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
            {
                proto.VmdName = RtcCore.GetRandomKey();
            }
            else
            {
                proto.VmdName = tbVmdName.Text;
            }

            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;
            proto.Padding = 0;

            foreach (string line in tbRangeExpression.Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string trimmedLine = line.Trim();

                bool remove = false;

                if (trimmedLine[0] == '-')
                {
                    remove = true;
                    trimmedLine = trimmedLine.Substring(1);
                }

                proto.AddFromTrimmedLine(trimmedLine, currentDomainSize, remove);
            }

            if (proto.AddRanges.Count == 0 && proto.AddSingles.Count == 0)
            {
                //No add range was specified, use entire domain
                proto.AddRanges.Add(new long[] { 0, (currentDomainSize > long.MaxValue ? long.MaxValue : Convert.ToInt64(currentDomainSize)) });
            }

            //Precalc the size of the vmd
            //Ignore the fact that addranges and subtractranges can overlap. Only account for add
            long size = 0;
            foreach (var v in proto.AddSingles)
            {
                size++;
            }

            foreach (var v in proto.AddRanges)
            {
                long x = v[1] - v[0];
                size += x;
            }
            //If the size is still 0 and we have removals, we're gonna use the entire range then sub from it so size is now the size of the domain
            if (size == 0 &&
                (proto.RemoveSingles.Count > 0 || proto.RemoveRanges.Count > 0) ||
                (proto.RemoveSingles.Count == 0 && proto.RemoveRanges.Count == 0 && size == 0))
            {
                size = currentDomainSize;
            }

            foreach (var v in proto.RemoveSingles)
            {
                size--;
            }

            foreach (var v in proto.RemoveRanges)
            {
                long x = v[1] - v[0];
                size -= x;
            }

            //Verify they want to continue if the domain is larger than 32MB and they didn't manually set ranges
            if (size > 0x2000000)
            {
                DialogResult result = MessageBox.Show("The VMD you're trying to generate is larger than 32MB\n The VMD size is " + ((size / 1024 / 1024) + 1) + " MB (" + (size / 1024f / 1024f / 1024f) + " GB).\n Are you sure you want to continue?", "VMD Detected", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }

            VMD = proto.Generate();

            if (VMD.Size == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                return false;
            }

            MemoryDomains.AddVMD(VMD);

            tbVmdName.Text = "";
            cbSelectedMemoryDomain.SelectedIndex = -1;
            cbSelectedMemoryDomain.Items.Clear();

            currentDomainSize = 0;

            tbRangeExpression.Text = "";

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";

            //send to vmd pool menu
            S.GET<VmdPoolForm>().RefreshVMDs();

            //Selects back the VMD Pool menu
            S.GET<VmdPoolForm>().GetFocus();

            return true;
        }

        private void ComputeRangeExpression(object sender, EventArgs e)
        {
            long maxAddress = mtbStartAddress.Maximum;
            long startAddress = mtbStartAddress.Value;
            long range = mtbRange.Value;
            long addressPlusRange = (startAddress + range > mtbStartAddress.Maximum) ? mtbStartAddress.Maximum : startAddress + range;
            bool rangeUsed = mtbRange.Checked;

            string generatedExpression = $"{startAddress.ToHexString()}-{(rangeUsed ? addressPlusRange.ToHexString() : maxAddress.ToHexString())}";
            tbRangeExpression.Text = generatedExpression;
        }
    }
}
