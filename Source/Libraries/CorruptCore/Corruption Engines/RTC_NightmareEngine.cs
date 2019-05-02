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
		public static ulong MinValue8Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString(), value);
		}
		public static ulong MaxValue8Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString(), value);
		}

		public static ulong MinValue16Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString(), value);
		}
		public static ulong MaxValue16Bit
		{
			get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString(), value);
		}

        public static ulong MinValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString(), value);
        }
        public static ulong MaxValue32Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString(), value);
        }
        public static ulong MinValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MINVALUE64BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MINVALUE64BIT.ToString(), value);
        }
        public static ulong MaxValue64Bit
        {
            get => (ulong)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.NIGHTMARE_MAXVALUE64BIT.ToString()];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.NIGHTMARE_MAXVALUE64BIT.ToString(), value);
        }


        public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString()] = 0UL;
			partial[RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString()] = 0xFFUL;

			partial[RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString()] = 0UL;
			partial[RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString()] = 0xFFFFUL;

            partial[RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString()] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFUL;

            partial[RTCSPEC.NIGHTMARE_MINVALUE64BIT.ToString()] = 0UL;
            partial[RTCSPEC.NIGHTMARE_MAXVALUE64BIT.ToString()] = 0xFFFFFFFFFFFFFFFFUL;

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
                            randomValue = CorruptCore.RND.RandomULong(MinValue32Bit, MaxValue32Bit);
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
