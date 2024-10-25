namespace RTCV.CorruptCore
{
    using System;
    using System.Linq;
    using System.Numerics;
    using RTCV.Common.CustomExtensions;
    using RTCV.CorruptCore.Extensions;

    public static class ValueGenerator
    {
        private static byte[] param1Bytes;
        private static byte[] param2Bytes;

        internal static BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
            ulong param1, ulong param2, int precision, int lifetime, int executeFrame, bool loop, int seed, BGValueMode mode)
        {
            BlastLayer bl = new BlastLayer();

            Random rand = new Random(seed);

            param1Bytes = null;
            param2Bytes = null;

            //We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
            for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
            {
                BlastUnit bu = GenerateUnit(domain, address, param1, param2, precision, lifetime, executeFrame, loop, mode, note, rand);
                if (bu != null)
                {
                    bl.Layer.Add(bu);
                }
            }

            return bl;
        }

        //As the param is a long, it's little endian. We have to account for this whenever the param is going to be used as a value for a byte array
        //If it's an address, we can leave it as is.
        //If it's something such as SET or Replace X with Y, we always flip as we need to go to big endian
        //If it's something like a bitwise operation, we read the values from left to right when pulling them from memory. As such, we also always convert to big endian
        private static BlastUnit GenerateUnit(string domain, long address, ulong param1, ulong param2, int precision, int lifetime, int executeFrame, bool loop,
            BGValueMode mode, string note, Random rand)
        {
            try
            {
                MemoryInterface mi = null;
                if (domain.Contains("[V]"))
                {
                    if (!MemoryDomains.VmdPool.ContainsKey(domain))
                    {
                        return null;
                    }

                    mi = MemoryDomains.VmdPool[domain];
                }
                else
                {
                    if (!MemoryDomains.MemoryInterfaces.ContainsKey(domain))
                    {
                        return null;
                    }

                    mi = MemoryDomains.MemoryInterfaces[domain];
                }

                byte[] value = new byte[precision];
                byte[] _temp = new byte[precision];
                BigInteger tiltValue = 0;

                if (param1Bytes == null)
                {
                    param1Bytes = ByteArrayExtensions.GetByteArrayValue(precision, param1, true);
                }

                if (param2Bytes == null)
                {
                    param2Bytes = ByteArrayExtensions.GetByteArrayValue(precision, param2, true);
                }

                //Use >= as Size is 1 indexed whereas address is 0 indexed
                if (address + value.Length > mi.Size)
                {
                    return null;
                }

                switch (mode)
                {
                    case BGValueMode.Add:
                        tiltValue = new BigInteger(param1Bytes);
                        break;
                    case BGValueMode.Subtract:
                        tiltValue = new BigInteger(param1Bytes) * -1;
                        break;
                    case BGValueMode.Random:
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = (byte)rand.Next(0, 255);
                        }

                        break;
                    case BGValueMode.RandomRange:
                        ulong temp = rand.NextULong(param1, param2);
                        value = ByteArrayExtensions.GetByteArrayValue(precision, temp, true);
                        break;
                    case BGValueMode.ReplaceXWithY:
                        if (mi.PeekBytes(address, address + precision, mi.BigEndian)
                            .SequenceEqual(param1Bytes))
                        {
                            value = param2Bytes;
                        }
                        else
                        {
                            return null;
                        }

                        break;
                    case BGValueMode.Set:
                        value = ByteArrayExtensions.GetByteArrayValue(precision, param1, true);
                        break;
                    case BGValueMode.ShiftRight:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        address += (long)param1;
                        if (address >= mi.Size)
                        {
                            return null;
                        }

                        break;
                    case BGValueMode.ShiftLeft:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        address -= (long)param1;
                        if (address < 0)
                        {
                            return null;
                        }
                        break;


                    //Bitwise operations
                    case BGValueMode.BitwiseAnd:
                        _temp = param1Bytes;
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = (byte)(value[i] & _temp[i]);
                        }

                        break;
                    case BGValueMode.BitwiseComplement:
                        _temp = param1Bytes;
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = (byte)(value[i] & _temp[i]);
                        }

                        break;
                    case BGValueMode.BitwiseOr:
                        _temp = param1Bytes;
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = (byte)(value[i] | _temp[i]);
                        }

                        break;
                    case BGValueMode.BitwiseXOr:
                        _temp = param1Bytes;
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = (byte)(value[i] ^ _temp[i]);
                        }

                        break;
                    case BGValueMode.BitwiseShiftLeft:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < (long)param1; i++)
                        {
                            ByteArrayExtensions.ShiftLeft(value);
                        }

                        break;
                    case BGValueMode.BitwiseShiftRight:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < (long)param1; i++)
                        {
                            ByteArrayExtensions.ShiftRight(value);
                        }

                        break;
                    case BGValueMode.BitwiseRotateLeft:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < (long)param1; i++)
                        {
                            ByteArrayExtensions.RotateLeft(value);
                        }

                        break;
                    case BGValueMode.BitwiseRotateRight:
                        value = mi.PeekBytes(address, address + precision, mi.BigEndian);
                        for (int i = 0; i < (long)param1; i++)
                        {
                            ByteArrayExtensions.RotateRight(value);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }

                var bu = new BlastUnit(value, domain, address, precision, mi.BigEndian, executeFrame, lifetime, note)
                {
                    TiltValue = tiltValue,
                    Loop = loop
                };

                return bu;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong in the RTC ValueGenerator Generator. " + ex.Message);
            }
        }
    }
}
