namespace RTCV.CorruptCore
{
    using System;

    [Serializable()]
    public abstract class FileMemoryInterface : IMemoryDomain
    {
        [NonSerialized]
        [Ceras.Exclude]
        internal static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public abstract long Size { get; }

        public abstract int WordSize { get; }
        public abstract bool BigEndian { get; }

        public abstract void CloseStream();
        public abstract void getMemoryDump();
        public abstract void wipeMemoryDump();
        public abstract byte[][] lastMemoryDump { get; set; }
        public abstract bool cacheEnabled { get; }

        public abstract long getMemorySize();
        public abstract long? lastMemorySize { get; set; }

        //public abstract Dictionary<String, String> CompositeFilenameDico { get; set; }

        public abstract void PokeByte(long address, byte data);
        public abstract void PokeBytes(long address, byte[] data);
        public abstract byte PeekByte(long address);
        public abstract byte[] PeekBytes(long address, int length);

        public abstract bool SetBackup();
        public abstract bool ResetBackup(bool askConfirmation = true);
        public abstract bool RestoreBackup(bool announce = true);
        public abstract bool ResetWorkingFile();
        public abstract bool ApplyWorkingFile();

        public volatile System.IO.Stream stream = null;
    }
}
