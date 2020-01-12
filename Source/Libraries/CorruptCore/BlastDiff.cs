using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{

    public static class BlastDiff
    {

        public static BlastLayer GetBlastLayer(string filename)
        {
            string thisSystem = (AllSpec.VanguardSpec[VSPEC.NAME] as string);
            var rp = MemoryDomains.GetRomParts(thisSystem, filename);

            IMemoryDomain Corrupt = new FileInterface("File|" + filename, false, false);

            (Corrupt as FileInterface).getMemoryDump(); //gotta cache it otherwise it's going to be super slow

            string[] selectedDomains = (string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"];

            if (selectedDomains.Length == 0)
            {
                MessageBox.Show("Error: No domain is selected");
                return null;
            }

            string targetDomain = selectedDomains.FirstOrDefault();

            MemoryDomainProxy[] mdps = (AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES] as MemoryDomainProxy[]);

            List<IMemoryDomain> originalDomains = new List<IMemoryDomain>();

            if (rp.Error != null)
            {
                originalDomains.Add(mdps.FirstOrDefault(it => it.Name == targetDomain).MD);

                if (selectedDomains.Length == 0)
                {
                    MessageBox.Show($"Warning: More than one domain was selected. The first one ({targetDomain}) was chosen.");
                }

            }
            else
            {
                originalDomains.Add(mdps.FirstOrDefault(it => it.Name == rp.PrimaryDomain).MD);

                if (rp.SecondDomain != null)
                    originalDomains.Add(mdps.FirstOrDefault(it => it.Name == rp.SecondDomain).MD);

            }

            bool useCustomPrecision = false;

            if (RtcCore.CurrentPrecision != 1)
            {
                var result = MessageBox.Show("Do you want to use Custom Precision for import?", "Use Custom Precision", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                useCustomPrecision = (result == DialogResult.Yes);
            }

            return (GetBlastLayer(originalDomains.ToArray(), Corrupt, rp.SkipBytes, useCustomPrecision));

        }

        private static string getNamefromIMemoryDomainArray(IMemoryDomain[] bank, long address)
        {
            if (bank == null | bank.Length == 0)
                return null;

            long bankStartAddressDrift = 0;

            for (int i = 0; i < bank.Length; i++)
            {
                if (address - bankStartAddressDrift < bank[i].Size)
                    return bank[i].Name;
                else
                    bankStartAddressDrift += bank[i].Size;
            }

            return null;
        }
        private static byte[] getBytefromIMemoryDomainArray(IMemoryDomain[] bank, long address, int precision)
        {
            if (bank == null | bank.Length == 0)
                return new byte[precision];

            long bankStartAddressDrift = 0;

            for (int i = 0; i < bank.Length; i++)
            {
                if (address - bankStartAddressDrift < bank[i].Size)
                    return bank[i].PeekBytes(address - bankStartAddressDrift, precision);
                else
                    bankStartAddressDrift += bank[i].Size;
            }

            return new byte[precision];
        }

        public static BlastLayer GetBlastLayer(IMemoryDomain[] Original, IMemoryDomain Corrupt, long skipBytes, bool useCustomPrecision)
        {
            BlastLayer bl = new BlastLayer();

            long OriginalMaxAddress = Original.Sum(it => it.Size);
            long OriginalFirstDomainMaxAddress = Original[0].Size;

            if (Corrupt.Size != OriginalMaxAddress)
            {
                MessageBox.Show("ERROR, DOMAIN SIZE MISMATCH");
                return null;
            }

            int precision = (useCustomPrecision ? RtcCore.CurrentPrecision : 1);

            for (long i = 0; i < OriginalMaxAddress; i += precision)
            {
                byte[] originalBytes = getBytefromIMemoryDomainArray(Original, i, precision);
                byte[] corruptBytes = Corrupt.PeekBytes(i, precision);



                if (!originalBytes.SequenceEqual(corruptBytes) && i >= skipBytes)
                {
                    if (Original[0].BigEndian)
                        corruptBytes = corruptBytes.FlipBytes();

                    BlastUnit bu;
                    if (i - skipBytes >= OriginalFirstDomainMaxAddress)
                    {
                        bu = RTC_NightmareEngine.GenerateUnit(getNamefromIMemoryDomainArray(Original, i - skipBytes), (i - skipBytes) - OriginalFirstDomainMaxAddress, precision, 0, corruptBytes);
                    }
                    else
                    {
                        bu = RTC_NightmareEngine.GenerateUnit(getNamefromIMemoryDomainArray(Original, i - skipBytes), i - skipBytes, precision, 0, corruptBytes);
                    }

                    bu.BigEndian = Original[0].BigEndian;
                    bl.Layer.Add(bu);
                }
            }


            if (bl.Layer.Count == 0)
                return null;
            else
                return bl;



        }

    }
}
