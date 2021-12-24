namespace RTCV.CorruptCore
{
    public interface ICorruptionEngine
    {
        public bool SupportsCustomPrecision { get; set; }
        public bool SupportsAutoCorrupt { get; set; }
        public bool SupportsGeneralParameters { get; set; }
        public bool SupportsMemoryDomains { get; set; }
        public System.Windows.Forms.Form Control { get; set; }
        public BlastLayer GetBlastLayer(long intensity);
        public void ResyncEmuProcess();
    }
}
