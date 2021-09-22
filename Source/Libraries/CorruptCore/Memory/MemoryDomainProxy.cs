namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;
    using RTCV.CorruptCore.Extensions;

    [Serializable]
    [MemberConfig(TargetMember.All)]
    public sealed class MemoryDomainProxy : MemoryInterface
    {
        [Exclude]
        public IMemoryDomain MD { get; private set; } = null;

        public override long Size { get; set; }
        public bool IsRPC { get; set; }
        public MemoryDomainProxy(IMemoryDomain md, bool rpc = false)
        {
            MD = md ?? throw new ArgumentNullException(nameof(md));
            Size = MD.Size;

            Name = MD.ToString();

            WordSize = MD.WordSize;
            Name = MD.ToString();
            BigEndian = MD.BigEndian;
            IsRPC = rpc;
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
            if (IsRPC)
            {
                byte[] ret = MD.PeekBytes(startAddress, (int)(endAddress - startAddress));
                return (raw || BigEndian) ? ret.FlipBytes() : ret;
            }
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
