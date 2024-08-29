﻿namespace RTCV.CorruptCore
{
    using System.Windows.Forms;
    using RTCV.Common.CustomExtensions;
    using RTCV.NetCore;
    using RTCV.CorruptCore.Extensions;

    public static class NightmareEngine
    {
        public static NightmareAlgo Algo
        {
            get => (NightmareAlgo)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_ALGO];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_ALGO, value);
        }

        public static ulong MinValue8Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE8BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE8BIT, value);
        }

        public static ulong MaxValue8Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE8BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE8BIT, value);
        }

        public static ulong MinValue16Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE16BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE16BIT, value);
        }

        public static ulong MaxValue16Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE16BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE16BIT, value);
        }

        public static ulong MinValue32Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE32BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE32BIT, value);
        }

        public static ulong MaxValue32Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE32BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE32BIT, value);
        }

        public static ulong MinValue64Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE64BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE64BIT, value);
        }

        public static ulong MaxValue64Bit
        {
            get => (ulong)AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE64BIT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE64BIT, value);
        }


        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");


            partial[RTCSPEC.NIGHTMARE_MINVALUE8BIT] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE8BIT] = 0xFFUL;

            partial[RTCSPEC.NIGHTMARE_MINVALUE16BIT] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE16BIT] = 0xFFFFUL;

            partial[RTCSPEC.NIGHTMARE_MINVALUE32BIT] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE32BIT] = 0xFFFFFFFFUL;

            partial[RTCSPEC.NIGHTMARE_MINVALUE64BIT] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            partial[RTCSPEC.NIGHTMARE_ALGO] = NightmareAlgo.RANDOM;

            return partial;
        }

        private static NightmareType type = NightmareType.SET;

        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment, bool useAlignment, byte[] replacementValue = null)
        {
            // Randomly selects a memory operation according to the selected algorithm

            switch (Algo)
            {
                case NightmareAlgo.RANDOM: //RANDOM always sets a random value
                    type = NightmareType.SET;
                    break;

                case NightmareAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
                    int result = RtcCore.RND.Next(1, 4);
                    switch (result)
                    {
                        case 1:
                            type = NightmareType.ADD;
                            break;
                        case 2:
                            type = NightmareType.SUBTRACT;
                            break;
                        case 3:
                            type = NightmareType.SET;
                            break;
                        default:
                            MessageBox.Show("Random returned an unexpected value (NightmareEngine switch(Algo) RANDOMTILT)");
                            return null;
                    }

                    break;

                case NightmareAlgo.TILT: //TILT can either add 1 or substract 1
                    result = RtcCore.RND.Next(1, 3);
                    switch (result)
                    {
                        case 1:
                            type = NightmareType.ADD;
                            break;

                        case 2:
                            type = NightmareType.SUBTRACT;
                            break;

                        default:
                            MessageBox.Show("Random returned an unexpected value (NightmareEngine switch(Algo) TILT)");
                            return null;
                    }
                    break;
            }


            if (domain == null)
            {
                return null;
            }

            MemoryInterface mi = MemoryDomains.GetInterface(domain);

            byte[] value = new byte[precision];

            long safeAddress = address;
            if (useAlignment)
                safeAddress = safeAddress - (address % precision) + alignment;
            if (safeAddress > mi.Size - precision && mi.Size > precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

            if (type == NightmareType.SET)
            {
                ulong randomValue = 0;

                bool def = false;
                switch (precision)
                {
                    case 1:
                        randomValue = RtcCore.RND.NextULong(MinValue8Bit, MaxValue8Bit, true);
                        break;
                    case 2:
                        randomValue = RtcCore.RND.NextULong(MinValue16Bit, MaxValue16Bit, true);
                        break;
                    case 4:
                        randomValue = RtcCore.RND.NextULong(MinValue32Bit, MaxValue32Bit, true);
                        break;
                    case 8:
                        randomValue = RtcCore.RND.NextULong(MinValue64Bit, MaxValue64Bit, true);
                        break;
                    default:
                        def = true;
                        break;
                }

                if (replacementValue == null)
                {
                    if (def)
                    {
                        for (int i = 0; i < precision; i++)
                        {
                            value[i] = (byte)RtcCore.RND.Next();
                        }
                    }
                    else
                    {
                        value = ByteArrayExtensions.GetByteArrayValue(precision, randomValue, true);
                    }
                }
                else
                {
                    value = replacementValue;
                }

                return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 1);
            }
            BlastUnit bu = new BlastUnit(StoreType.ONCE, StoreTime.PREEXECUTE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian);
            switch (type)
            {
                case NightmareType.ADD:
                    bu.TiltValue = 1;
                    break;
                case NightmareType.SUBTRACT:
                    bu.TiltValue = -1;
                    break;
                default:
                    bu.TiltValue = 0;
                    break;
            }
            return bu;
        }
    }
}
