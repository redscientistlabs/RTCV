using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    //Contains everything necessary to analyze data before corrupting it and determine if it should be corrupted or not

    public static class Limiter
    {
        public static byte[][] Execute(EngineConfig config, byte[][] values )
        {
            if (config.Limiter == null || config.Limiter.Length == 0)
                return values;  //No need to execute the limiter if it isn't used.

            if(config.LimiterCache == null) //Build cache if needed (Should be invalidated if engine config changes)
                config.LimiterCache = new LimiterCaching(config);

            for(int i=0; i<values.Length;i++)
            {   //Set value arrays to null if they don't match the cache's content
                var value = values[i];
                if (!config.LimiterCache.IsInCache(value))
                    values[i] = null;
            }

            return values; // we don't really need to return the value but we do it anyway.
        }


    }

    public class LimiterCaching
    {
        public byte[][] SingleValues; //this is a terrible name for this but i don't have any better idea

        //public Tuple<float, float>[] FloatRanges;
        //public Tuple<byte[], byte[]>[] IntegerRanges;
        //Eventually we could cache ranges, rules, whatever.
        //We'd use this for Integers: AAAAAAAA-BBBBBBBB (in hex)
        //We'd use this for floats: 0.005-0.01 (decimal)

        public LimiterCaching(EngineConfig config)
        {
            //Build cache for faster searching (not sure if it's actually faster)

            List<byte[]> SingleValuesList = new List<byte[]>();

            foreach(var line in config.Limiter)
            {
                var cleanLine = line.Trim();

                if (string.IsNullOrWhiteSpace(cleanLine) || cleanLine[0] == '#')//skip blank lines
                    continue; //also if people wanna comment lines they can start it with #

                SingleValuesList.Add(RTCV_Extensions.StringToByteArray(cleanLine));
            }

            SingleValues = SingleValuesList.ToArray();

        }

        public bool IsInCache(byte[] value)
        {

            //If we'd implement checking ranges, it would be better to check them before individual values

            foreach(var cachedValue in SingleValues)
            {
                bool match = true;

                for (int i = 0; i < cachedValue.Length; i++)
                    if (cachedValue[i] != value[i])
                    {
                        match = false;
                        break;
                    }

                if (match)  //all bytes matched, we found it.
                    return true;
            }


            //didn't find it
            return false;
        }
    }
}
