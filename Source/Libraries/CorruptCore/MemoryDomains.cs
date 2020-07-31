namespace RTCV.CorruptCore
{
    using System;

    public enum FileInterfaceIdentity
    {
        SELF_DESCRIBE,
        HASHED_PREFIX,
        FULL_PATH,
    }

    public interface IMemoryDomain
    {
        string Name { get; }
        long Size { get; }
        int WordSize { get; }
        bool BigEndian { get; }

        byte PeekByte(long addr);
        byte[] PeekBytes(long address, int length);
        void PokeByte(long addr, byte val);
    }
}
