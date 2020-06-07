namespace RTCV.CorruptCore
{
    using RTCV.NetCore;

    public static class RTC_HellgenieEngine
    {
        public static ulong MinValue8Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE8BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE8BIT, value);
        }

        public static ulong MaxValue8Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE8BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE8BIT, value);
        }

        public static ulong MinValue16Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE16BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE16BIT, value);
        }

        public static ulong MaxValue16Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE16BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE16BIT, value);
        }

        public static ulong MinValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE32BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE32BIT, value);
        }

        public static ulong MaxValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE32BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE32BIT, value);
        }

        public static ulong MinValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE64BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE64BIT, value);
        }

        public static ulong MaxValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE64BIT];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE64BIT, value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");

            partial[RTCSPEC.HELLGENIE_MINVALUE8BIT] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE8BIT] = 0xFFUL;

            partial[RTCSPEC.HELLGENIE_MINVALUE16BIT] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE16BIT] = 0xFFFFUL;

            partial[RTCSPEC.HELLGENIE_MINVALUE32BIT] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE32BIT] = 0xFFFFFFFFUL;

            partial[RTCSPEC.HELLGENIE_MINVALUE64BIT] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE64BIT] = 0xFFFFFFFFFFFFFFFFUL;

            return partial;
        }

        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment)
        {
            if (domain == null)
            {
                return null;
            }

            MemoryInterface mi = MemoryDomains.GetInterface(domain);

            byte[] value = new byte[precision];

            long safeAddress = address - (address % precision) + alignment;
            if (safeAddress > mi.Size - precision && mi.Size > precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

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

            if (def)
            {
                for (int i = 0; i < precision; i++)
                {
                    value[i] = (byte)RtcCore.RND.Next();
                }
            }
            else
            {
                value = CorruptCore_Extensions.GetByteArrayValue(precision, randomValue, true);
            }

            return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 0);
        }
    }
}
