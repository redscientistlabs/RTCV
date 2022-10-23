namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;
    using RTCV.CorruptCore.Extensions;
    using RTCV.NetCore;
    using RTCV.NetCore.Commands;

    [Serializable]
    [MemberConfig(TargetMember.All)]
    public sealed class MemoryDomainProxy : MemoryInterface
    {
        [Exclude]
        public IMemoryDomain MD { get; private set; } = null;

        public override long Size { get; set; }
        //non-rpc constructor
        public MemoryDomainProxy(IMemoryDomain md)
        {
            MD = md ?? throw new ArgumentNullException(nameof(md));

            bool rpc = false;
            bool ro = false;

            Size = MD.Size;
            Name = MD.ToString();
            WordSize = MD.WordSize;
            Name = MD.ToString();
            BigEndian = MD.BigEndian;

            UsingRPC = rpc;
            ReadOnly = ro;
        }

        //rpc constructor
        public MemoryDomainProxy(IMemoryDomain md, bool rpc, bool ro)
        {
            MD = md ?? throw new ArgumentNullException(nameof(md));

            Size = MD.Size;
            Name = MD.ToString();
            WordSize = MD.WordSize;
            Name = MD.ToString();
            BigEndian = MD.BigEndian;

            UsingRPC = rpc;
            ReadOnly = ro;
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
            if (MD == null) //Should not happen but handled.
            {
                //Assume we are in the wrong process, route to Vanguard.
                //This will slowdown the reading cycle due to crossing NetCore
                var objectValue = new object[] { Name, startAddress, endAddress, raw };
                return LocalNetCoreRouter.QueryRoute<byte[]>(Endpoints.CorruptCore, Remote.DomainPeekBytes, objectValue, true);
            }

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
            if (MD == null) //Should not happen but handled.
            {
                //Assume we are in the wrong process, route to Vanguard.
                //This will slowdown the reading cycle due to crossing NetCore
                var objectValue = new object[] { Name, startAddress, value, raw };
                LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.DomainPokeBytes, objectValue, true);
            }
            else
            {

                if (!raw || BigEndian)
                {
                    value.FlipBytes();
                }

                if (UsingRPC)
                {
                    (MD as IRPCMemoryDomain).PokeBytes(startAddress, value);
                    return;
                }

                for (long i = 0; i < value.Length; i++)
                {
                    PokeByte(startAddress + i, value[i]);
                }
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
                if (MD == null) //Should not happen but handled.
                {
                    //Assume we are in the wrong process, route to Vanguard.
                    //This will slowdown the reading cycle due to crossing NetCore
                    var objectValue = new object[] { Name, address };
                    return LocalNetCoreRouter.QueryRoute<byte>(Endpoints.CorruptCore, Remote.DomainPeekByte, objectValue, true);
                }

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
                if (MD == null) //Should not happen but handled.
                {
                    //Assume we are in the wrong process, route to Vanguard.
                    //This will slowdown the reading cycle due to crossing NetCore
                    var objectValue = new object[] { Name, address, value };
                    LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.DomainPokeByte, objectValue, true);
                }
                else
                {
                    MD.PokeByte(address, value);
                }

            }
            catch (Exception e)
            {
                throw new Exception($"{Name ?? "NULL"} {Size} PokeByte {address},{value} failed! {(MD == null ? "MemoryDomain is NULL" : "")}", e);
            }
        }
    }
}
