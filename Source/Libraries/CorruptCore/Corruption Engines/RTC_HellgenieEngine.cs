using System;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_HellgenieEngine
	{
		public static ulong MinValue8Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString(), value);
		}
		public static ulong MaxValue8Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString(), value);
		}

		public static ulong MinValue16Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString(), value);
		}
		public static ulong MaxValue16Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString(), value);
		}

        public static ulong MinValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString(), value);
        }
        public static ulong MaxValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString(), value);
        }

        public static ulong MinValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE64BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE64BIT.ToString(), value);
        }
        public static ulong MaxValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE64BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE64BIT.ToString(), value);
        }

        public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString()] = 0UL;
			partial[RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString()] = 0xFFUL;

			partial[RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString()] = 0UL;
			partial[RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString()] = 0xFFFFUL;

            partial[RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString()] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFUL;

            partial[RTCSPEC.HELLGENIE_MINVALUE64BIT.ToString()] = 0UL;
            partial[RTCSPEC.HELLGENIE_MAXVALUE64BIT.ToString()] = 0xFFFFFFFFFFFFFFFFUL;


            return partial;
		}

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryInterface mi = MemoryDomains.GetInterface(domain);

				Byte[] value = new Byte[precision];

				long safeAddress = address - (address % precision);

                ulong randomValue = 0;
                bool def = false;
				switch (precision)
				{
					case 1:
						randomValue = CorruptCore.RND.RandomULong(MinValue8Bit, MaxValue8Bit);
						break;
					case 2:
						randomValue = CorruptCore.RND.RandomULong(MinValue16Bit, MaxValue16Bit);
						break;
                    case 4:
                        randomValue = CorruptCore.RND.RandomULong(MinValue32Bit, MaxValue32Bit);
                        break;
                    case 8:
                        randomValue = CorruptCore.RND.RandomULong(MinValue64Bit, MaxValue64Bit);
                        break;
                    default:
                        def = true;
                        break;
                }

                if(def)
                    for (int i = 0; i < precision; i++)
                        value[i] = (byte)CorruptCore.RND.Next();
                else
					value = CorruptCore_Extensions.GetByteArrayValue(precision, randomValue, true);

                return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("HellGenie Engine GenerateUnit Threw Up" + ex);
			}
		}

	}
}
