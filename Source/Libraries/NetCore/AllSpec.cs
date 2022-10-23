namespace RTCV.NetCore
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA2211", Justification = "These fields cannot be made private or const because they are used by emulators")]
    public static class AllSpec
    {
        public static volatile FullSpec CorruptCoreSpec;
        public static volatile FullSpec VanguardSpec;
        public static volatile FullSpec UISpec;
        public static volatile FullSpec PluginSpec;
    }
}
