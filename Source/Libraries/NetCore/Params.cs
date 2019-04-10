using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using RTCV.NetCore;

namespace RTCV.NetCore
{
	public static class Params
	{

		public static string paramsDir = //This works on both sides because bizhawk is still in the same
										 //folder as RTC but will break if the emu isn't in the same folder
			Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + 
			"RTC" + Path.DirectorySeparatorChar + 
			"PARAMS" + Path.DirectorySeparatorChar;

		public static void SetParam(string paramName, string data = null)
		{
			if (data == null)
			{
				if (!IsParamSet(paramName))
					SetParam(paramName, "");
			}
			else
				File.WriteAllText(paramsDir + Path.DirectorySeparatorChar + paramName, data);
		}

		public static void RemoveParam(string paramName)
		{
			if (IsParamSet(paramName))
				File.Delete(paramsDir + Path.DirectorySeparatorChar + paramName);
		}

		public static string ReadParam(string paramName)
		{
			if (IsParamSet(paramName))
				return File.ReadAllText(paramsDir + Path.DirectorySeparatorChar + paramName);

			return null;
		}

		public static bool IsParamSet(string paramName)
		{
			return File.Exists(paramsDir + Path.DirectorySeparatorChar + paramName);
		}
	}

}