namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class MemoryInterface
    {
        public virtual long Size { get; set; }
        public int WordSize { get; set; }
        public virtual string Name { get; set; }
        public bool BigEndian { get; set; }

        public abstract byte[] GetDump();

        public abstract byte[] PeekBytes(long startAddress, long endAddress, bool raw);
        public abstract void PokeBytes(long startAddress, byte[] value, bool raw = true);

        public abstract byte PeekByte(long address);

        public abstract void PokeByte(long address, byte value);

        private MemoryInterface this[string name] => this;

        public MemoryInterface()
        {
        }
    }
}
