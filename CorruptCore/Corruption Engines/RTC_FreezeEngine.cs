using System;
using System.Windows.Forms;

namespace RTCV.CorruptCore
{
	public static class RTC_FreezeEngine
	{
		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryInterface mi = MemoryDomains.GetInterface(domain);
				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, StoreTime.PREEXECUTE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("Freeze Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}
