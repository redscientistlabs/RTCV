using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_VectorEngine
	{

		public static string LimiterListHash
		{
			get => (string)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.VECTOR_LIMITERLISTHASH.ToString(), value);
		}

		public static string ValueListHash
		{
			get => (string)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.VECTOR_VALUELISTHASH.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.VECTOR_VALUELISTHASH.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()] = String.Empty;
			partial[RTCSPEC.VECTOR_VALUELISTHASH.ToString()] = String.Empty;

			return partial;
		}


		public static BlastUnit GenerateUnit(string domain, long address)
		{
			if (domain == null)
				return null;

			long safeAddress = address - (address % 4); //32-bit trunk

			MemoryInterface mi = MemoryDomains.GetInterface(domain);
			if (mi == null)
				return null;

			try
			{
				//Enforce the safeaddress at generation
				if (Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, domain, LimiterListHash, mi))
					return new BlastUnit(Filtering.GetRandomConstant(ValueListHash, 4), domain, safeAddress, 4, mi.BigEndian, 0, 1, null, true, false, true);
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Vector Engine GenerateUnit Threw Up" + ex);
			}
		}


	}
}
