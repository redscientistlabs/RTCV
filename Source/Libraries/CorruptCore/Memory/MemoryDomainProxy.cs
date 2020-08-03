namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public sealed class MemoryDomainProxy : MemoryInterface
    {
        [NonSerialized, Ceras.Exclude]
        public IMemoryDomain MD = null;

        public override long Size { get; set; }

        public MemoryDomainProxy(IMemoryDomain _md)
        {
            MD = _md;
            Size = MD.Size;

            Name = MD.ToString();

            WordSize = MD.WordSize;
            Name = MD.ToString();
            BigEndian = MD.BigEndian;
        }

        public MemoryDomainProxy()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public override byte[] GetDump()
        {
            return PeekBytes(0, Size);
        }

        public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
        {
            //endAddress is exclusive
            List<byte> data = new List<byte>();
            for (long i = startAddress; i < endAddress; i++)
            {
                data.Add(PeekByte(i));
            }

            if (raw || BigEndian)
            {
                return data.ToArray();
            }
            else
            {
                return data.ToArray().FlipBytes();
            }
        }

        public override void PokeBytes(long startAddress, byte[] value, bool raw = true)
        {
            if (!raw || !BigEndian)
            {
                value.FlipBytes();
            }

            for (long i = 0; i < value.Length; i++)
            {
                PokeByte(startAddress + i, value[i]);
            }
        }

        public override byte PeekByte(long address)
        {
            if (address > Size - 1)
            {
                return 0;
            }

            try
            {
                return MD.PeekByte(address);
            }
            catch (Exception e)
            {
                throw new Exception($"{Name ?? "NULL"} {Size} PeekByte {address} failed! {(MD == null ? "MemoryDomain is NULL" : "")} ", e);
            }
        }

        public override void PokeByte(long address, byte value)
        {
            if (address > Size - 1)
            {
                return;
            }

            try
            {
                MD.PokeByte(address, value);
            }
            catch (Exception e)
            {
                throw new Exception($"{Name ?? "NULL"} {Size} PokeByte {address},{value} failed! {(MD == null ? "MemoryDomain is NULL" : "")}", e);
            }
        }
    }
}
