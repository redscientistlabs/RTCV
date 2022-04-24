namespace RTCV.CorruptCore
{
    using System;

    [Serializable()]
    public abstract class FileMemoryInterface : IMemoryDomain
    {
        [Ceras.Exclude]
        internal static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public abstract long Size { get; }

        public abstract int WordSize { get; }
        public abstract bool BigEndian { get; }

        public abstract void CloseStream();
        public abstract void getMemoryDump();
        public abstract FileTarget[] GetFileTargets();
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

        public abstract bool SendRealToBackup(bool askConfirmation = true);
        public abstract bool SendBackupToReal(bool announce = true);
        public abstract bool CommitChangesToReal();

        private volatile System.IO.Stream _stream = null;
        public System.IO.Stream stream
        {
            get => _stream;
            set => _stream = value;
        }
    }
}
