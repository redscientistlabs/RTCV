namespace RTCV.CorruptCore
{
    public interface ICorruptionEngine
    {
        public bool SupportsCustomPrecision { get; }
        public bool SupportsAutoCorrupt { get; }
        public bool SupportsGeneralParameters { get; }
        public bool SupportsMemoryDomains { get; }
        public System.Windows.Forms.Form Control { get; }
        public BlastLayer GetBlastLayer(long intensity);
        public string ToString();
    }
}
