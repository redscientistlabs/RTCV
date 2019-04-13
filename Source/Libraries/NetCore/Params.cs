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
        //Todo - Isolate this out
		public static string ParamsDir
        {
            get
            {

                if (AllSpec.CorruptCoreSpec?["RTCDIR"] is string rtcDir)
                {
                    return rtcDir + "\\PARAMS\\";
                }

                return Directory.GetCurrentDirectory() + "\\RTC\\PARAMS\\";
            }
        }

        public static void SetParam(string paramName, string data = null)
		{
			if (data == null)
			{
				if (!IsParamSet(paramName))
					SetParam(paramName, "");
			}
			else
				File.WriteAllText(ParamsDir + Path.DirectorySeparatorChar + paramName, data);
		}

		public static void RemoveParam(string paramName)
		{
			if (IsParamSet(paramName))
				File.Delete(ParamsDir + Path.DirectorySeparatorChar + paramName);
		}

		public static string ReadParam(string paramName)
		{
			if (IsParamSet(paramName))
				return File.ReadAllText(ParamsDir + Path.DirectorySeparatorChar + paramName);

			return null;
		}

		public static bool IsParamSet(string paramName)
		{
			return File.Exists(ParamsDir + Path.DirectorySeparatorChar + paramName);
		}
	}

}