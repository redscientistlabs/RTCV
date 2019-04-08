using System;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_HellgenieEngine
	{
		public static long MinValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString(), value);
		}
		public static long MaxValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString(), value);
		}

		public static long MinValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString(), value);
		}
		public static long MaxValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString(), value);
		}

		public static long MinValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString(), value);
		}
		public static long MaxValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString()] = 0L;
			partial[RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString()] = 0xFFL;

			partial[RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString()] = 0L;
			partial[RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString()] = 0xFFFFL;

			partial[RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString()] = 0L;
			partial[RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;


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

				long randomValue = -1;
				switch (precision)
				{
					case 1:
						randomValue = CorruptCore.RND.RandomLong(MinValue8Bit, MaxValue8Bit);
						break;
					case 2:
						randomValue = CorruptCore.RND.RandomLong(MinValue16Bit, MaxValue16Bit);
						break;
					case 4:
						randomValue = CorruptCore.RND.RandomLong(MinValue32Bit, MaxValue32Bit);
						break;
				}

				if (randomValue != -1)
				{
					value = CorruptCore_Extensions.GetByteArrayValue(precision, randomValue, true);
				}
				else
				{
					for (int i = 0; i < precision; i++)
					{
						value[i] = (byte)CorruptCore.RND.Next();
					}
				}

				return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("HellGenie Engine GenerateUnit Threw Up" + ex);
			}
		}

	}
}
