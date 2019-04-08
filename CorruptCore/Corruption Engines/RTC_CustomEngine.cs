using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RTCV.NetCore;


namespace RTCV.CorruptCore
{
	public static class RTC_CustomEngine
	{
		private static Dictionary<String, Type> name2TypeDico = new Dictionary<string, Type>();
		public static Dictionary<String, PartialSpec> Name2TemplateDico = new Dictionary<string, PartialSpec>();

		public static long MinValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE8BIT.ToString(), value);
		}
		public static long MaxValue8Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString(), value);
		}

		public static long MinValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE16BIT.ToString(), value);
		}
		public static long MaxValue16Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString(), value);
		}

		public static long MinValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MINVALUE32BIT.ToString(), value);
		}
		public static long MaxValue32Bit
		{
			get => (long)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString(), value);
		}

		public static BlastUnitSource Source
		{
			get => (BlastUnitSource)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_SOURCE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_SOURCE.ToString(), value);
		}

		public static StoreType StoreType
		{
			get => (StoreType)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORETYPE.ToString(), value);
		}
		public static StoreTime StoreTime
		{
			get => (StoreTime)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETIME.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORETIME.ToString(), value);
		}
		public static CustomStoreAddress StoreAddress
		{
			get => (CustomStoreAddress)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STOREADDRESS.ToString(), value);
		}

		public static int Delay
		{
			get => (int)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_DELAY.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_DELAY.ToString(), value);
		}
		public static int Lifetime
		{
			get => (int)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIFETIME.ToString(), value);
		}

		public static BigInteger TiltValue
		{
			get => (BigInteger)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_TILTVALUE.ToString(), value);
		}

		public static LimiterTime LimiterTime
		{
			get => (LimiterTime)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME.ToString(), value);
		}
		public static bool LimiterInverted
		{
			get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERINVERTED.ToString(), value);
		}
		public static StoreLimiterSource StoreLimiterSource
		{
			get => (StoreLimiterSource)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_STORELIMITERMODE.ToString(), value);
		}

		public static bool Loop
		{
			get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LOOP.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LOOP.ToString(), value);
		}

		public static CustomValueSource ValueSource
		{
			get => (CustomValueSource)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE.ToString(), value);
		}

		public static string LimiterListHash
		{
			//Intentionally nullable cast
			get => RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH.ToString()] as string;
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_LIMITERLISTHASH.ToString(), value);
		}
		public static string ValueListHash
		{
			//Intentionally nullable cast
			get => RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUELISTHASH.ToString()] as string;
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_VALUELISTHASH.ToString(), value);
		}
		


		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
						
				if (domain == null)
					return null;

				MemoryInterface mi = MemoryDomains.GetInterface(domain);


				byte[] value = new byte[precision];
				long safeAddress = address - (address % precision);

			
				BlastUnit bu = new BlastUnit();

				switch (Source)
				{
					case BlastUnitSource.VALUE:
					{
						switch (ValueSource)
						{
							case CustomValueSource.VALUELIST:
							{
								value = Filtering.GetRandomConstant(ValueListHash, precision);
							}
							break;

							case CustomValueSource.RANGE:
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
									value = CorruptCore_Extensions.GetByteArrayValue(precision, randomValue, true);
								else
									for (int i = 0; i < precision; i++)
										value[i] = (byte)CorruptCore.RND.Next();
							}
							break;

							case CustomValueSource.RANDOM:
							{
								for (int i = 0; i < precision; i++)
									value[i] = (byte)CorruptCore.RND.Next();
							}
							break;
								
						}
					}
					break;

					case BlastUnitSource.STORE:
					{
						bu.StoreType = StoreType;
						bu.StoreTime = StoreTime;

						switch (StoreAddress)
						{
							case CustomStoreAddress.RANDOM:
							{
								BlastTarget bt = CorruptCore.GetBlastTarget();
								long safeStartAddress = bt.Address - (bt.Address % precision);
								bu.SourceDomain = bt.Domain;
								bu.SourceAddress = safeStartAddress;
							}
							break;
							case CustomStoreAddress.SAME:
							{
								bu.SourceDomain = domain;
								bu.SourceAddress = safeAddress;
							}
							break;
						}
					}
					break;
				}
				//Precision has to be before Value
				bu.Precision = precision;
				bu.Value = value;

				bu.Address = safeAddress;
				bu.Domain = domain;
				bu.Source = Source;
				bu.ExecuteFrame = Delay;
				bu.Lifetime = Lifetime;
				bu.LimiterTime = LimiterTime;
				bu.Loop = Loop;
				bu.InvertLimiter = LimiterInverted;
				bu.TiltValue = TiltValue;
				bu.StoreLimiterSource = StoreLimiterSource;
				bu.GeneratedUsingValueList = (Source == BlastUnitSource.VALUE && ValueSource == CustomValueSource.VALUELIST);
				bu.BigEndian = mi.BigEndian;

				//Only set a list if it's used
				if (LimiterTime != LimiterTime.NONE)
					bu.LimiterListHash = LimiterListHash;

				//Limiter handling
				if (LimiterTime == LimiterTime.GENERATE)
				{
					if (!bu.LimiterCheck(mi))
						return null;
				}

				return bu;
			}
			catch (Exception ex)
			{
				throw new Exception("Custom Engine GenerateUnit Threw Up\n" + ex);
			}
		}


		public static bool IsConstant(byte[] bytes, string[] list, bool bigEndian)
		{
			if (list == null || bytes == null)
				return true;
			if (!bigEndian)
				return list.Contains(ByteArrayToString(bytes));
			Array.Reverse(bytes);
			return list.Contains(ByteArrayToString(bytes));
		}

		public static string ByteArrayToString(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}

		public static byte[] GetRandomConstant(string[] list)
		{
			if (list == null)
			{
				byte[] buffer = new byte[4];
				CorruptCore.RND.NextBytes(buffer);
				return buffer;
			}

			return StringToByteArray(list[CorruptCore.RND.Next(list.Length)]);
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}

		public static void getDefaultPartial(PartialSpec pSpec)
		{
			pSpec =	InitTemplate_NightmareEngine(pSpec.Name);
			pSpec[RTCSPEC.CUSTOM_PATH.ToString()] = "";
			foreach (var k in pSpec.GetKeys())
			{
				name2TypeDico[k] = pSpec[k].GetType();
			}
				
		}


		public static void InitTemplates()
		{
			var nightmare = InitTemplate_NightmareEngine();
			var hellgenie = InitTemplate_HellgenieEngine();
			var freeze = InitTemplate_FreezeEngine();
			var distortion = InitTemplate_DistortionEngine();
			var pipe = InitTemplate_PipeEngine();
			var vector = InitTemplate_VectorEngine();

			Name2TemplateDico[nightmare[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = nightmare;
			Name2TemplateDico[hellgenie[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = hellgenie;
			Name2TemplateDico[freeze[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = freeze;
			Name2TemplateDico[distortion[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = distortion;
			Name2TemplateDico[pipe[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = pipe;
			Name2TemplateDico[vector[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = vector;

			LoadUserTemplates();

		}

		public static void LoadUserTemplates()
		{
			string[] paths = System.IO.Directory.GetFiles(CorruptCore.engineTemplateDir);
			paths = paths.OrderBy(x => x).ToArray();
			foreach (var p in paths)
			{
				PartialSpec _p = LoadTemplateFile(p);
				Name2TemplateDico[_p[RTCSPEC.CUSTOM_NAME.ToString()].ToString()] = _p;
			}
		}

		//Don't set a Limiter or Value list hash in any of these. We just leave it on whatever is currently set and set that it shouldn't be used.

		//This is because we need to be able to have the UI select some item (the comboboxes don't have an "empty" state)
		public static PartialSpec InitTemplate_NightmareEngine(string name = null)
		{
			PartialSpec pSpec;
			if (name == null)
				pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);
			else
				pSpec = new PartialSpec(name);

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Nightmare Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 1;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.NONE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.VALUE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.RANDOM;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.IMMEDIATE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;
			return pSpec;
		}
		public static PartialSpec InitTemplate_HellgenieEngine()
		{
			PartialSpec pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);
			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Hellgenie Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.NONE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;


			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.VALUE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.RANDOM;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.IMMEDIATE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;
			return pSpec;
		}
		public static PartialSpec InitTemplate_DistortionEngine()
		{
			PartialSpec pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Distortion Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 50;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 1;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.NONE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;


			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.STORE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.SAME;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.IMMEDIATE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;
			return pSpec;
		}
		public static PartialSpec InitTemplate_FreezeEngine()
		{
			PartialSpec pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Freeze Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.NONE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;


			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.STORE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.SAME;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.PREEXECUTE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;
			return pSpec;
		}
		public static PartialSpec InitTemplate_PipeEngine()
		{
			PartialSpec pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);
			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Pipe Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.NONE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;


			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.STORE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.RANDOM;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.PREEXECUTE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.CONTINUOUS;
			return pSpec;

		}
		public static PartialSpec InitTemplate_VectorEngine()
		{
			PartialSpec pSpec = new PartialSpec(NetCore.AllSpec.CorruptCoreSpec.name);

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = "Vector Engine";

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 4;

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = 0;
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = 1;
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = false;

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = new BigInteger(0);

			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = LimiterTime.GENERATE;
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = StoreLimiterSource.ADDRESS;
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = false;


			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = 0L;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.VALUELIST;

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = BlastUnitSource.VALUE;

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.RANDOM;
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = StoreTime.IMMEDIATE;
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;
			return pSpec;

		}

		public static PartialSpec getCurrentConfigSpec()
		{
			PartialSpec pSpec = new PartialSpec(RTCV.NetCore.AllSpec.CorruptCoreSpec.name);

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = pSpec[RTCSPEC.CUSTOM_NAME.ToString()];

			pSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()];

			pSpec[RTCSPEC.CUSTOM_DELAY.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_DELAY.ToString()];
			pSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIFETIME.ToString()];
			pSpec[RTCSPEC.CUSTOM_LOOP.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LOOP.ToString()];

			pSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_TILTVALUE.ToString()];

			pSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERLISTHASH.ToString()];
			pSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERTIME.ToString()];
			pSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_LIMITERINVERTED.ToString()];

			pSpec[RTCSPEC.CUSTOM_VALUELISTHASH.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUELISTHASH.ToString()];

			pSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()];
			pSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()];
			pSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()];
			pSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()];
			pSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()];
			pSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()];

			pSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_VALUESOURCE.ToString()];

			pSpec[RTCSPEC.CUSTOM_SOURCE.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_SOURCE.ToString()];

			pSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STOREADDRESS.ToString()];
			pSpec[RTCSPEC.CUSTOM_STORETIME.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETIME.ToString()];
			pSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORETYPE.ToString()];
			pSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()] = RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_STORELIMITERMODE.ToString()];

			return pSpec;
		}


		public static string CustomPath
		{
			get => RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.CUSTOM_PATH.ToString()].ToString();
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.CUSTOM_PATH.ToString(), value);
		}

		private static SafeJsonTypeSerialization.JsonKnownTypesBinder InitSpecKnownTypes()
		{
			var t = new SafeJsonTypeSerialization.JsonKnownTypesBinder();
			t.KnownTypes.Add(typeof(long));
			t.KnownTypes.Add(typeof(string));
			return t;
		}

		public static bool LoadTemplate(String template)
		{
			PartialSpec spec = Name2TemplateDico[template];
			if (spec == null)
				return false;

			RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(spec);
			return true;
		}

		public static PartialSpec LoadTemplateFile(string Filename = null)
		{
			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "cet",
					Title = "Open Engine Template File",
					Filter = "CET files|*.cet",
					RestoreDirectory = true
				};

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName;
				}
				else
					return null;
			}

			PartialSpec pSpec = new PartialSpec("RTCSpec");
			try
			{
				using (FileStream fs = File.Open(Filename, FileMode.OpenOrCreate))
				{
					Dictionary<String, Object> d = JsonHelper.Deserialize<Dictionary<String,Object>>(fs);


					//We don't want to store type data in the serialized data but specs store object
					//To work around this, we store the type in a dictionary and pass the data through a typeconverter
					foreach (var k in d.Keys)
					{
						var t = d[k];
						//If the type doesn't exist, just use what it's parsed as
						if (!name2TypeDico.ContainsKey(k))
						{
							pSpec[k] = t;
							continue;
						}

						var type = name2TypeDico[k];
						if (t.GetType() != type)
						{
							//There's no typeconverter for bigint so we have to handle it manually. Convert it to a string then bigint it
							if (type == typeof(BigInteger))
							{
								if (BigInteger.TryParse(t.ToString(), out BigInteger a))
									t = a;
								else
									throw new Exception("Couldn't convert " + t.ToString() +
										" to BigInteger! Something is wrong with your template.");
							}
							//handle the enums
							else if (type.BaseType == typeof(Enum))
							{
								//We can't use tryparse here so we have to catch the exception
								try
								{
									t = Enum.Parse(type, t.ToString());
								}catch(ArgumentException e)
								{
									throw new Exception("Couldn't convert " + t.ToString() +
										" to " + type.Name + "! Something is wrong with your template.");
								}
							}
							else
								t = TypeDescriptor.GetConverter(t).ConvertTo(t, type);
						}
						pSpec[k] = t;
					}

				}
			}
			catch (Exception e)
			{
				MessageBox.Show("The Template file could not be loaded" + e);
				return null;
			}

			//Overwrites spec path with loaded path
			pSpec[RTCSPEC.CUSTOM_PATH.ToString()] = Filename;


			return pSpec;
		}

		public static string SaveTemplateFile(bool SaveAs = false)
		{
			PartialSpec pSpec = getCurrentConfigSpec();

			string path;
			string templateName;

			if (SaveAs || CustomPath == null)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog
				{
					DefaultExt = "cet",
					Title = "Save Engine Template File",
					Filter = "CET files|*.cet",
					RestoreDirectory = true,
				};

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					path = saveFileDialog1.FileName;
					templateName = Path.GetFileNameWithoutExtension(path);
				}
				else
					return null;
			}
			else
			{
				path = CustomPath;
				templateName = CustomPath;
			}

			pSpec[RTCSPEC.CUSTOM_NAME.ToString()] = templateName;
			pSpec[RTCSPEC.CUSTOM_PATH.ToString()] = path;
			pSpec[RTCSPEC.CORE_CURRENTPRECISION] = CorruptCore.CurrentPrecision;


			string jsonString = pSpec.GetSerializedDico();
			File.WriteAllText(path, jsonString);
			Name2TemplateDico.Add(templateName, pSpec);
			return templateName;
		}
	}
}
