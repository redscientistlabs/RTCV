using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    //Manages the generator parameters, specs and inter-process param syncs

    public class EngineConfig
    {
        //Any null setting is Unset or non-updated if incremential

        //Generator Config
        public int? Intensity = null;           //Amount of Units generated per Blast
        public int? ErrorDelay = null;          //Cycle skips when AutoCorrupt Running

        public string[] Targets = null;         //Selected MemoryDomains
        public string TargetSource = null;      //Algorithm for spreading Units on targets

        public int? Precision = null;           //Byte precision of generated Units
        //1 : 8bit
        //2 : 16bit
        //4 : 32bit
        //8 : 64bit

        public string[] Limiter = null;         //Use values encoded in Hex, Length/2 must match Custom Precision or Emulator Default Precision
                                                //Set empty Array for no Limiter, otherwise sending null in an update will not remove any set Limiter
        public LimiterCaching LimiterCache = null;
        public ValueProcessor Value = null;    //Sets parameters for fetching/generating values
        public PostProcessor Transformer = null;//Sets parameters for a runtime value transformation
        public IteratorBinding? Binder = null;  //Sets the binding for runtime iteration

    }



}
