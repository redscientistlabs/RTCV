namespace RTCV.CorruptCore
{
    public class RomParts
    {
        public string Error { get; set; }
        public string PrimaryDomain { get; set; }
        public string SecondDomain { get; set; }
        public int SkipBytes { get; set; } = 0;
    }
}
