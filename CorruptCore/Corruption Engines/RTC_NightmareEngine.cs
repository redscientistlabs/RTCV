using System;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_NightmareEngine
	{
		public static NightmareAlgo Algo
		{
			get => (NightmareAlgo)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_ALGO.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_ALGO.ToString(), value);
		}
		public static long MinValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString(), value);
		}
		public static long MaxValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString(), value);
		}

		public static long MinValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString(), value);
		}
		public static long MaxValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString(), value);
		}

		public static long MinValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString(), value);
		}
		public static long MaxValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString(), value);
		}


		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString()] = 0L;
			partial[RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString()] = 0xFFL;

			partial[RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString()] = 0L;
			partial[RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString()] = 0xFFFFL;

			partial[RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString()] = 0L;
			partial[RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			partial[RTCSPEC.NIGHTMARE_ALGO.ToString()] = NightmareAlgo.RANDOM;

			return partial;
		}

		private static NightmareType type = NightmareType.SET;

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				switch (Algo)
				{
					case NightmareAlgo.RANDOM: //RANDOM always sets a random value
						type = NightmareType.SET;
						break;

					case NightmareAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
						int result = CorruptCore.RND.Next(1, 4);
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
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) RANDOMTILT)");
								return null;
						}

						break;

					case NightmareAlgo.TILT: //TILT can either add 1 or substract 1
						result = CorruptCore.RND.Next(1, 3);
						switch (result)
						{
							case 1:
								type = NightmareType.ADD;
								break;

							case 2:
								type = NightmareType.SUBTRACT;
								break;

							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) TILT)");
								return null;
						}
						break;
				}


				if (domain == null)
					return null;
				MemoryInterface mi = MemoryDomains.GetInterface(domain);

				byte[] value = new byte[precision];

				long safeAddress = address - (address % precision);

				if (type == NightmareType.SET)
				{
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
			catch (Exception ex)
			{
				throw new Exception("Nightmare Engine GenerateUnit Threw Up", ex);
			}
		}
	}
}
