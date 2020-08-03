namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public sealed class NullMemoryInterface : MemoryInterface
    {
        [Ceras.Exclude]
        public override long Size { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override byte[] GetDump()
        {
            return null;
        }

        public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
        {
            return new byte[] { 0 };
        }

        public override void PokeBytes(long startAddress, byte[] value, bool raw = true)
        {
        }

        public override byte PeekByte(long address)
        {
            return 0;
        }

        public override void PokeByte(long address, byte value)
        {
        }

        public NullMemoryInterface()
        {
            Size = 64;
            Name = "NULL";
            WordSize = 1;
            BigEndian = false;
        }
    }
}
