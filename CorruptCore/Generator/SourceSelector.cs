using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    //Contains all the stuff to select memory domains and addreses

    public static class SourceSelector
    {
        public static string[] Execute(EngineConfig config)
        {
            var intensity = config.Intensity;
            var targets = config.Targets;
            var targetSelect = config.TargetSource;

            List<string> sources = new List<string>();

            switch (targetSelect)
            {
                case "SPREAD":          //Randomly spread across the Memory Domains

                    for (int i = 0; i < intensity; i++)
                        sources.Add(targets.PickRandom());

                    break;
                case "CHUNK":           //Sent to one single zone that is randomly selected among the selected Memory Domains

                    var ChunkRandomlySelectedSource = targets.PickRandom();
                    for (int i = 0; i < intensity; i++)
                        sources.Add(ChunkRandomlySelectedSource);

                    break;
                case "BURST":           //10 Chunks of 1/10 of the total Intensity

                    var intensityPart = intensity / 10;
                    for (int i = 0; i < 10; i++)
                    {
                        var BurstRandomlySelectedSource = targets.PickRandom();
                        for (int j = 0; j < intensityPart; j++)
                            sources.Add(BurstRandomlySelectedSource);
                    }

                    var remaining = intensity % 10;
                    if(remaining != 0)
                    {
                        //BONUS CHUNK
                        var BurstRandomlySelectedSource = targets.PickRandom();
                        for (int j = 0; j < remaining; j++)
                            sources.Add(BurstRandomlySelectedSource);
                    }

                    break;
                case "EVEN":            //(Put description here)

                    //TODO

                    break;
                case "PROPORTIONAL":    //(Put description here)

                    //TODO

                    break;
                case "NORMALIZED":      //(Put description here)

                    //TODO

                    break;
                case "PROBABILITY":     //Uses set per-domain probability to determine uneven chances of domains getting selected as a source

                    //TODO
                    //We will probably have to add an array of probabilities in the config that can be set for this algorithm

                    break;
            }



            return sources.ToArray();
        }
    }
}
