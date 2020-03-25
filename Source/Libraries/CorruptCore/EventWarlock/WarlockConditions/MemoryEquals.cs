using RTCV.CorruptCore.EventWarlock;
using RTCV.CorruptCore.EventWarlock.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock.WarlockConditions
{
    /// <summary>
    /// Example conditional
    /// </summary>
    [Serializable]
    [WarlockEditable]
    public class MemoryEquals : EWConditional
    {
        [WarlockEditorField("Domain")]  string domain;
        [WarlockEditorField("Address")] long address;
        [WarlockEditorField("Precision")] int precision;
        [WarlockEditorField("Value")] byte[] value;


        /// <summary>
        /// Parameterless consturctor for serialization
        /// </summary>
        public MemoryEquals() { }

        public MemoryEquals(string domain, long address, byte value)
        {
            this.domain = domain;
            this.address = address;
            this.precision = 1;
            this.value = new[]{value};
        }
        public MemoryEquals(string domain, long address, int precision, byte[] value)
        {
            this.domain = domain;
            this.address = address;
            this.precision = precision;
            this.value = value;
        }

        public override bool Evaluate(Grimoire grimoire)
        {
            var c = new Ceras.SerializerConfig();

            var mi = MemoryDomains.GetInterface(domain);
            var b = mi.PeekBytes(address, address + precision, true);
            if (b == null)
            {
                RTCV.Common.Logging.GlobalLogger.Trace("MemoryEquals PeekBytes was null!");
                return false;
            }
            return Win32.ByteArrayCompare(b, value);
        }
    }
}
