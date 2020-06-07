namespace RTCV.NetCore
{
    using System.IO;

    public static class Params
    {
        //Todo - Isolate this out
        public static string ParamsDir
        {
            get
            {
                if (AllSpec.CorruptCoreSpec?["RTCDIR"] is string rtcDir)
                {
                    return Path.Combine(rtcDir, "PARAMS");
                }

                //Check for the normal rtc dir
                if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "..", "RTC", "PARAMS")))
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), "..", "RTC", "PARAMS");
                }

                //Fall back to our current dir
                var path = Path.Combine(Directory.GetCurrentDirectory(), "RTC", "PARAMS");
                Directory.CreateDirectory(path);
                return Path.Combine(Directory.GetCurrentDirectory(), "RTC", "PARAMS");
            }
        }

        public static void SetParam(string paramName, string data = null)
        {
            if (data == null)
            {
                if (!IsParamSet(paramName))
                {
                    SetParam(paramName, "");
                }
            }
            else
            {
                File.WriteAllText(Path.Combine(ParamsDir, paramName), data);
            }
        }

        public static void RemoveParam(string paramName)
        {
            if (IsParamSet(paramName))
            {
                File.Delete(Path.Combine(ParamsDir, paramName));
            }
        }

        public static string ReadParam(string paramName)
        {
            if (IsParamSet(paramName))
            {
                return File.ReadAllText(Path.Combine(ParamsDir, paramName));
            }

            return null;
        }

        public static bool IsParamSet(string paramName) => File.Exists(Path.Combine(ParamsDir, paramName));
    }
}
