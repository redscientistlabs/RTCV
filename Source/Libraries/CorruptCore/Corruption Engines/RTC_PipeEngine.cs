using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTCV.CorruptCore
{
	public static class RTC_PipeEngine
	{

		public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				if (domain == null)
					return null;
				BlastTarget pipeStart = CorruptCore.GetBlastTarget();
				MemoryInterface mi = MemoryDomains.GetInterface(domain);
				MemoryInterface startmi = MemoryDomains.GetInterface(pipeStart.Domain);

                long safeAddress = address - (address % precision) + alignment;

				long safePipeStartAddress = pipeStart.Address - (pipeStart.Address % precision) + alignment;
				if (safeAddress > mi.Size - precision)
					safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address

				if (safePipeStartAddress > startmi.Size - precision)
					safePipeStartAddress = startmi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address

                return new BlastUnit(StoreType.CONTINUOUS, StoreTime.PREEXECUTE, domain, safeAddress, pipeStart.Domain, safePipeStartAddress, precision, mi.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("Pipe Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}
